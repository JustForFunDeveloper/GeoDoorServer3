using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GeoDoorServer3.API.Model
{
    public class CommandItem
    {
        public int Id { get; set; }
        public string CommandName { get; set; }
        public Command Command { get; set; }
    }

    public enum Command
    {
        CheckUser,
        OpenDoor,
        OpenGate
    }
}
