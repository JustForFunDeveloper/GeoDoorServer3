using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GeoDoorServer3.Models.DataModels;
using Microsoft.Extensions.Logging;

namespace GeoDoorServer3.CustomService
{
    public class OpenHabMessageService : IOpenHabMessageService
    {
        private readonly IDataSingleton _iDataSingleton;

        public OpenHabMessageService(IDataSingleton iDataSingleton)
        {
            _iDataSingleton = iDataSingleton;
        }

        // Gets Data from the OpenHab Rest API
        public async Task<string> GetData(string itemPath)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("text/plain"));

            var result = await client.GetStringAsync(itemPath);

            _iDataSingleton.AddErrorLog(new ErrorLog()
            {
                LogLevel = LogLevel.Debug,
                MsgDateTime = DateTime.Now,
                Message = $"{typeof(OpenHabMessageService)}:GetStringAsync => GetOpenHabStatus => {result}"
            });
            return result;
        }

        // Posts Data to the OpenHab Rest API
        public async Task<bool> PostData(string itemPath, string value)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("text/plain"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, itemPath);
            request.Content = new StringContent(value,
                Encoding.UTF8,
                "text/plain");

            await client.SendAsync(request)
                .ContinueWith(responseTask =>
                {
                    _iDataSingleton.AddErrorLog(new ErrorLog()
                    {
                        LogLevel = LogLevel.Debug,
                        MsgDateTime = DateTime.Now,
                        Message = $"{typeof(OpenHabMessageService)}:PostData => Response: {responseTask.Result}"
                    });
                });
            return false;
        }
    }
}
