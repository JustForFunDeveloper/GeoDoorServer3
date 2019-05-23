using System.Collections.Concurrent;
using GeoDoorServer3.CustomService.Models;
using GeoDoorServer3.Models.DataModels;

namespace GeoDoorServer3.CustomService
{
    public class DataSingleton : IDataSingleton
    {
        private OpenHabStatus _openHabStatus;
        private ConcurrentQueue<ErrorLog> _concurrentQueue;

        public DataSingleton()
        {
            _openHabStatus = new OpenHabStatus();
            _concurrentQueue = new ConcurrentQueue<ErrorLog>();
        }

        public OpenHabStatus GetOpenHabStatus()
        {
            return _openHabStatus;
        }

        public ConcurrentQueue<ErrorLog> GetConcurrentQueue()
        {
            return _concurrentQueue;
        }
    }
}
