using System;
using System.Threading.Tasks;
using GeoDoorServer3.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace GeoDoorServer3.Hubs
{
    public class ClockHub : Hub<IClock>
    {
        private readonly ILogger<ClockHub> _logger;

        public ClockHub(ILogger<ClockHub> logger)
        {
            _logger = logger;
        }

        public async Task SendTimeToClients(string user)
        {
            await Clients.All.ShowTime(DateTime.Now);
            await Clients.Caller.SendUser("answer" + user);

            _logger.LogInformation($" ====>  {user}");
        }
    }
}
