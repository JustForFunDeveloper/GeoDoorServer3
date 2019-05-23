using System.Collections.Concurrent;
using System.Threading.Tasks;
using GeoDoorServer3.CustomService.Models;
using GeoDoorServer3.Data;
using GeoDoorServer3.Models.DataModels;

namespace GeoDoorServer3.CustomService
{
    internal interface IScopedService
    {
        Task<string> GetDoorStatus();
        OpenHabStatus GetOpenHabStatus();
        void WriteErrorLogs();
        void AddQueueMessage(ErrorLog errorLog);
    }

    internal class ScopedService : IScopedService
    {
        private readonly IOpenHabMessageService _openHab;
        private readonly IDataSingleton _dataSingleton;
        private readonly string _doorPath = "http://192.168.1.114:8080/rest/items/eg_buero/state";
        private readonly UserDbContext _context;

        public ScopedService(IOpenHabMessageService openHab, IDataSingleton dataSingleton, UserDbContext context)
        {
            _openHab = openHab;
            _dataSingleton = dataSingleton;
            _context = context;
        }

        public async Task<string> GetDoorStatus()
        {
            return await _openHab.GetData(_doorPath);     
        }

        public OpenHabStatus GetOpenHabStatus()
        {
            return _dataSingleton.GetOpenHabStatus();
        }

        public void WriteErrorLogs()
        {
            ConcurrentQueue<ErrorLog> concurrentQueue = _dataSingleton.GetConcurrentQueue();

            foreach (var item in concurrentQueue)
            {
                if (concurrentQueue.TryDequeue(out ErrorLog errorLog))
                    _context.ErrorLogs.Add(errorLog);
                
            }
            _context.SaveChanges();
        }

        public void AddQueueMessage(ErrorLog errorLog)
        {
            _dataSingleton.GetConcurrentQueue().Enqueue(errorLog);
        }
    }
}
