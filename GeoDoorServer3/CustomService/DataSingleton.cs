using System.Collections.Concurrent;
using GeoDoorServer3.CustomService.Models;
using GeoDoorServer3.Models.DataModels;

namespace GeoDoorServer3.CustomService
{
    public class DataSingleton : IDataSingleton
    {
        // TODO: Path should be configurable

        private OpenHabStatus _openHabStatus;
        private ConcurrentQueue<ErrorLog> _concurrentQueue;
        private readonly string _getGate = "http://192.168.1.114:8080/rest/items/eg_esstisch/state";
        private readonly  string _setGate = "http://192.168.1.114:8080/rest/items/eg_esstisch";

        public DataSingleton()
        {
            _openHabStatus = new OpenHabStatus();
            _concurrentQueue = new ConcurrentQueue<ErrorLog>();
        }

        public string GatePathStatus()
        {
            return _getGate;
        }

        public string GatePathValueChange()
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
