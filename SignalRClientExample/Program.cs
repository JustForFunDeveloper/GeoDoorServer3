using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClientConsole
{
    class Program
    {
        private static HubConnection connection;
        private static void Main(string[] args)
        {
            Init();
            Connect();
            bool end = true;
            string argument = Console.ReadLine();
            while (true)
            {
                SendMessage(argument);
                Thread.Sleep(1000);
            }
        }

        private static async void Init()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44349/hubs/clock")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        private static async void Connect()
        {
            connection.On<DateTime>("ShowTime", dateTime =>
            {
                Console.WriteLine(dateTime);
            });
            connection.On<string>("SendUser", answer =>
            {
                Console.WriteLine(answer);
            });
            try
            {
                await connection.StartAsync();
                Console.WriteLine("Connection started");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async void SendMessage(string argument)
        {
            try
            {
                await connection.InvokeAsync("SendTimeToClients",
                    argument);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
