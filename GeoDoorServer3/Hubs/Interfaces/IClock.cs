using System;
using System.Threading.Tasks;

namespace GeoDoorServer3.Hubs.Interfaces
{
    public interface IClock
    {
        Task ShowTime(DateTime currentTime);
        Task SendUser(string answer);
    }
}
