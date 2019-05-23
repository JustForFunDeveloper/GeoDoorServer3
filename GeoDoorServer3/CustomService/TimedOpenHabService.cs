using System;
using System.Threading;
using System.Threading.Tasks;
using GeoDoorServer3.CustomService.Models;
using GeoDoorServer3.Models.DataModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GeoDoorServer3.CustomService
{
    internal class TimedOpenHabService : IHostedService, IDisposable
    {
        public IServiceProvider Services { get; }

        private Timer _timer;

        public TimedOpenHabService(IServiceProvider services)
        {
            Services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedService>();

                scopedProcessingService.AddQueueMessage(new ErrorLog()
                {
                    LogLevel = LogLevel.Information,
                    MsgDateTime = DateTime.Now,
                    Message = "Timed Background Service is starting."
                });
            }

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedService>();

                string result = await scopedProcessingService.GetDoorStatus();


                var scopedDataSingleton =
                    scope.ServiceProvider
                        .GetRequiredService<IDataSingleton>();

                if (result.Equals("ON"))
                    scopedDataSingleton.GetOpenHabStatus().GateStatus = GateStatus.GateClosed;
                else if (result.Equals("OFF"))
                    scopedDataSingleton.GetOpenHabStatus().GateStatus = GateStatus.GateOpen;

                scopedDataSingleton.GetOpenHabStatus().OnlineTimeSpan =
                    DateTime.Now.Subtract(scopedDataSingleton.GetOpenHabStatus().StartTime);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedService>();

                scopedProcessingService.AddQueueMessage(new ErrorLog()
                {
                    LogLevel = LogLevel.Information,
                    MsgDateTime = DateTime.Now,
                    Message = "Timed Background Service is stopping."
                });
            }

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
