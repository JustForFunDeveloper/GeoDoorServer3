using System.Collections.Concurrent;
using GeoDoorServer3.CustomService.Models;
using GeoDoorServer3.Models.DataModels;

namespace GeoDoorServer3.CustomService
{
    public class DataSingleton : IDataSingleton
    {
        // TODO: Path should be configurable

        private SystemStatus _systemStatus;
        private ConcurrentQueue<ErrorLog> _concurrentQueue;
        private readonly string _getGate = "http://192.168.1.114:8080/rest/items/eg_esstisch/state";
        private readonly  string _setGate = "http://192.168.1.114:8080/rest/items/eg_esstisch";

        public DataSingleton()
        {
            _systemStatus = new SystemStatus();
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

        public SystemStatus GetSystemStatus()
        {
            return _systemStatus;
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
