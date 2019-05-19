using GeoDoorServer3.CustomService.Models;

namespace GeoDoorServer3.CustomService
{
    public class DataSingleton : IDataSingleton
    {
        private OpenHabStatus _openHabStatus;

        public DataSingleton()
        {
            _openHabStatus = new OpenHabStatus();
        }

        public OpenHabStatus GetData()
        {
            return _openHabStatus;
        }
    }
}
