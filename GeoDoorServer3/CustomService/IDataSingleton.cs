﻿using System.Collections.Concurrent;
using GeoDoorServer3.CustomService.Models;
using GeoDoorServer3.Models.DataModels;

namespace GeoDoorServer3.CustomService
{
    public interface IDataSingleton
    {
        string GetGatePath();
        string SetGatePath();

        OpenHabStatus GetOpenHabStatus();

        ConcurrentQueue<ErrorLog> GetQueue();
        void AddErrorLog(ErrorLog errorLog);
    }
}
