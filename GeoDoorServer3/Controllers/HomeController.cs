﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GeoDoorServer3.CustomService;
using GeoDoorServer3.CustomService.Models;
using GeoDoorServer3.Data;
using Microsoft.AspNetCore.Mvc;
using GeoDoorServer3.Models;
using GeoDoorServer3.Models.DataModels;
using Microsoft.Extensions.Logging;

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


            List<User> users = _context.Users.OrderByDescending(u => u.LastConnection).ToList();
            _myDataModel.LastUsers = new List<string>();
            foreach (var user in users)
            {
                if (_myDataModel.LastUsers.Count <= 3)
                    _myDataModel.LastUsers.Add(user.Name);
            }

            _myDataModel.LastConnection = users[0].LastConnection;
            _myDataModel.UsersAllowed = _context.Users.Where(u => u.AccessRights.Equals(AccessRights.Allowed)).ToList().Count;
            _myDataModel.UsersRegistered = _context.Users.ToList().Count;


            _myDataModel.Errors = _context.ErrorLogs.Where(e => e.LogLevel.Equals(LogLevel.Error)).ToList().Count();
            _myDataModel.LogMessagesList = new List<string>();
            List<ErrorLog> errorLogs = _context.ErrorLogs.OrderByDescending(e => e.MsgDateTime).TakeLast(100).ToList();
            foreach (var errorLog in errorLogs)
            {
                _myDataModel.LogMessagesList.Add(errorLog.ToString());
            }
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

            public string OnlineTime
            {
                get
                {
                    TimeSpan timeSpan = _iDataSingleton.GetData().OnlineTimeSpan;
                    return timeSpan.ToString(@"hh\:mm\:ss");
                }
            }

            public int UsersAllowed { get; set; }
            public int UsersRegistered { get; set; }
            public List<string> LastUsers { get; set; }
            public DateTime LastConnection { get; set; }
            public int Errors { get; set; }

            public List<string> LogMessagesList { get; set; }
        }
    }
}
