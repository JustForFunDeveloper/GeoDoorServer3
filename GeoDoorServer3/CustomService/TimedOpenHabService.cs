using System;
using System.Threading;
using System.Threading.Tasks;
using GeoDoorServer3.CustomService.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GeoDoorServer3.CustomService
{
    internal class TimedOpenHabService : IHostedService, IDisposable
    {

        public IServiceProvider Services { get; }

        private readonly ILogger _logger;
        private Timer _timer;

        public TimedOpenHabService(IServiceProvider services ,ILogger<TimedOpenHabService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

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
                        .GetRequiredService<IScopedOpenHabService>();

                string result = await scopedProcessingService.DoWork();

                using (var dataScope = Services.CreateScope())
                {
                    var scopedDataSingleton =
                        scope.ServiceProvider
                            .GetRequiredService<IDataSingleton>();

                    if (result.Equals("ON"))
                        scopedDataSingleton.GetData().GateStatus = GateStatus.GateClosed;
                    else if (result.Equals("OFF"))
                        scopedDataSingleton.GetData().GateStatus = GateStatus.GateOpen;

                    scopedDataSingleton.GetData().OnlineTimeSpan =
                        DateTime.Now.Subtract(scopedDataSingleton.GetData().StartTime);
                }
                
                _logger.LogInformation($"-- DoWork: Result=> {result}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
