using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GeoDoorServer3.API.Model
{
    public class CommandItem
    {
        public string Id { get; set; }
        public string CommandValue { get; set; }
        public Command Command { get; set; }
    }

    public enum Command
    {
        CheckUser,
        OpenDoor,
        OpenGate
    }
}
