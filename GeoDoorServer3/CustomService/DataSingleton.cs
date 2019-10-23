using System.Collections.Concurrent;
using GeoDoorServer3.CustomService.Models;
using GeoDoorServer3.Models.DataModels;

namespace GeoDoorServer3.CustomService
{
    public class DataSingleton : IDataSingleton
    {
        private OpenHabStatus _openHabStatus;
        private ConcurrentQueue<ErrorLog> _concurrentQueue;
        private readonly string _getGate = "http://192.168.1.114:8080/rest/items/eg_buero/state";
        private readonly  string _setGate = "http://192.168.1.114:8080/rest/items/eg_buero";

        public DataSingleton()
        {
            _openHabStatus = new OpenHabStatus();
            _concurrentQueue = new ConcurrentQueue<ErrorLog>();
        }

        public string GetGatePath()
        {
            return _getGate;
        }

        public string SetGatePath()
        {
            return _setGate;
        }

        public OpenHabStatus GetOpenHabStatus()
        {
            return _openHabStatus;
        }

        public ConcurrentQueue<ErrorLog> GetQueue()
        {
            return _concurrentQueue;
        }

        public void AddErrorLog(ErrorLog errorLog)
        {
            _concurrentQueue.Enqueue(errorLog);
        }
    }
}
