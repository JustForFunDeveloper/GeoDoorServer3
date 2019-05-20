using System;
using System.Diagnostics;
using GeoDoorServer3.CustomService;
using GeoDoorServer3.CustomService.Models;
using GeoDoorServer3.Data;
using Microsoft.AspNetCore.Mvc;
using GeoDoorServer3.Models;

namespace GeoDoorServer3.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserDbContext _context;
        private readonly IDataSingleton _iDataSingleton;

        private DataModel _myDataModel { get; set; }

        public HomeController(UserDbContext context, IDataSingleton dataSingleton)
        {
            _context = context;
            _iDataSingleton = dataSingleton;
            _context.Database.EnsureCreated();
            _myDataModel = new DataModel(_iDataSingleton);
        }

        public IActionResult Index()
        {
            return View(_myDataModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class DataModel
        {
            private readonly IDataSingleton _iDataSingleton;

            public DataModel(IDataSingleton iDataSingleton)
            {
                _iDataSingleton = iDataSingleton;
            }

            public OpenHabStatus OpenHabStatus
            {
                get { return _iDataSingleton.GetData(); }
            }

            //public string OnlineTime
            //{
            //    get
            //    {
            //        TimeSpan timeSpan = _iDataSingleton.GetData().OnlineTimeSpan;
            //        return timeSpan.ToString(@"hh\:mm\:ss");
            //    }
            //}
        }
    }
}
