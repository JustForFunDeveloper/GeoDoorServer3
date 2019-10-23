﻿using System;
using GeoDoorServer3.CustomService;
using GeoDoorServer3.Data;
using GeoDoorServer3.Models.DataModels;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GeoDoorServer3
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .UpdateDataDatabase<UserDbContext>()
                .Run();
        }

        public static IWebHost UpdateDataDatabase<T>(this IWebHost webHost) where T : DbContext
        {
            var serviceScopeFactory = (IServiceScopeFactory)webHost
                .Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;

                var dbContext = services.GetRequiredService<T>();
                var dataSingleton = services.GetRequiredService<IDataSingleton>();

                try
                {
                    dbContext.GetService<IMigrator>().Migrate();
                }
                catch (Exception e)
                {
                    dataSingleton.AddErrorLog(new ErrorLog()
                    {
                        LogLevel = LogLevel.Debug,
                        MsgDateTime = DateTime.Now,
                        Message = $"{typeof(Program)}:UpdateDataDatabase: Exception => {e}"
                    });
                }
                dataSingleton.AddErrorLog(new ErrorLog()
                {
                    LogLevel = LogLevel.Debug,
                    MsgDateTime = DateTime.Now,
                    Message = $"{typeof(Program)}:UpdateDataDatabase successful!"
                });
            }

            return webHost;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5000", "http://192.168.1.113:5000");
    }
}