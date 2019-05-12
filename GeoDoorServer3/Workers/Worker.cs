using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Threading.Tasks;
using GeoDoorServer3.Hubs;
using GeoDoorServer3.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GeoDoorServer3.Workers
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<ClockHub, IClock> _clockHub;

        public Worker(ILogger<Worker> logger, IHubContext<ClockHub, IClock> clockHub)
        {
            _logger = logger;
            _clockHub = clockHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation($"Worker running at: {DateTime.Now}");
                //await _clockHub.Clients.All.ShowTime(DateTime.Now);
                
                _logger.LogInformation(GetKnxItem());
                await Task.Delay(1000);
            }
        }

        private string GetKnxItem()
        {
            return GetStatus("/rest/items/ges/state");
        }

        private string RestCall(string Action, string Item)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.1.114:8080");
                var content = new StringContent(Action);
                var result = client.PostAsync(Item, content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                return resultContent;
            }
        }

        private string GetStatus(string Item)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.1.114:8080");
                var result = client.GetAsync(Item).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                return "State => " + resultContent;
            }
        }
    }
}
