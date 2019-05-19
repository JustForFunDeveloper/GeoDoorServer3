using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoDoorServer3.CustomService.Models;

namespace GeoDoorServer3.CustomService
{
    public interface IDataSingleton
    {
        OpenHabStatus GetData();
    }
}
