using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GeoDoorServer3.CustomService
{
    public class OpenHabMessageService : IOpenHabMessageService
    {
        private readonly ILogger<OpenHabMessageService> _logger;

        public OpenHabMessageService(ILogger<OpenHabMessageService> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetData(string itemPath)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("text/plain"));

            var result = await client.GetStringAsync(itemPath);

            _logger.LogError($"{DateTime.Now}:  GetData=> {result}");

            return result;
        }

        public async Task<bool> PostData(string itemName)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://192.168.1.114:8080/rest/items/");
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("text/plain"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "eg_buero");
            request.Content = new StringContent("ON",
                Encoding.UTF8,
                "text/plain");

            await client.SendAsync(request)
                .ContinueWith(responseTask =>
                {
                    _logger.LogError($"--- Response: {0}", responseTask.Result);
                });

            _logger.LogError($"--- {DateTime.Now}:  PostData=> {itemName}");
            return false;
            //return await Task.Run(() => false);
        }
    }
}
