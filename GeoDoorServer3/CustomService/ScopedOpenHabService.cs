using System;
using System.Threading.Tasks;
using GeoDoorServer3.CustomService.Models;

namespace GeoDoorServer3.CustomService
{
    internal interface IScopedOpenHabService
    {
        Task<string> DoWork();
        OpenHabStatus GetData();
    }

    internal class ScopedOpenHabService : IScopedOpenHabService
    {
        private readonly IOpenHabMessageService _openHab;
        private readonly IDataSingleton _dataSingleton;
        private readonly string _doorPath = "http://192.168.1.114:8080/rest/items/eg_buero/state";

        public ScopedOpenHabService(IOpenHabMessageService openHab, IDataSingleton dataSingleton)
        {
            _openHab = openHab;
            _dataSingleton = dataSingleton;
        }

        public async Task<string> DoWork()
        {
            return await _openHab.GetData(_doorPath);     
        }

        public OpenHabStatus GetData()
        {
            return _dataSingleton.GetData();
        }
    }
}
