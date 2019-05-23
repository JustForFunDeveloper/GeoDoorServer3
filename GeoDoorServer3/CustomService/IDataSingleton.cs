using System.Collections.Concurrent;
using GeoDoorServer3.CustomService.Models;
using GeoDoorServer3.Models.DataModels;

namespace GeoDoorServer3.CustomService
{
    public interface IDataSingleton
    {
        OpenHabStatus GetOpenHabStatus();
        ConcurrentQueue<ErrorLog> GetConcurrentQueue();
    }
}
