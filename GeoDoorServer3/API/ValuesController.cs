using System;
using System.Linq;
using System.Threading.Tasks;
using GeoDoorServer3.API.Model;
using GeoDoorServer3.CustomService;
using GeoDoorServer3.CustomService.Models;
using GeoDoorServer3.Data;
using GeoDoorServer3.Models.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GeoDoorServer3.API
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly UserDbContext _context;
        private readonly IOpenHabMessageService _openHab;
        private readonly IDataSingleton _iDataSingleton;

        public ValuesController(UserDbContext context, IOpenHabMessageService openHab, IDataSingleton iDataSingleton)
        {
            _context = context;
            _openHab = openHab;
            _iDataSingleton = iDataSingleton;
        }

        // GET: api/<controller>
        //[HttpGet]
        public async Task<ActionResult<CommandItem>> Get()
        {
            CommandItem commandItem = new CommandItem()
            {
                Id = "66488732346",
                Command = 0,
                CommandValue = "OK"
            };

            return Accepted(commandItem);
        }

        // GET api/<controller>/command/5
        //[HttpGet("command/{id}")]
        //public async Task<ActionResult<CommandItem>> GetCommandItem(int id)
        //{
        //    _iDataSingleton.AddErrorLog(new ErrorLog()
        //    {
        //        LogLevel = LogLevel.Debug,
        //        MsgDateTime = DateTime.Now,
        //        Message = $"{typeof(ValuesController)}:GetCommandItem => doorStatus"
        //    });

        //    string doorStatus = await _openHab.GetData(_iDataSingleton.GetGatePath());
        //    if (null != commandItem)
        //        return commandItem;
        //    else
        //    {
        //        return NotFound();
        //    }
        //}


        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<CommandItem>> PostCommandItem([FromBody]CommandItem item)
        {
            try
            {
                _iDataSingleton.AddErrorLog(new ErrorLog()
                {
                    LogLevel = LogLevel.Debug,
                    MsgDateTime = DateTime.Now,
                    Message = $"{typeof(ValuesController)}:PostCommandItem => {item}"
                });

                if (!CheckCommandItem(item))
                    return NotFound();

                if (!await CheckUserOrCreate(item))
                {
                    return Accepted(new CommandItem()
                    {
                        Id = item.Id,
                        Command = item.Command,
                        CommandValue = "User not allowed!"
                    });
                }

                return Accepted(new CommandItem()
                {
                    Id = item.Id,
                    Command = item.Command,
                    CommandValue = "OK"
                });
            }
            catch (Exception e)
            {
                _iDataSingleton.AddErrorLog(new ErrorLog()
                {
                    LogLevel = LogLevel.Error,
                    MsgDateTime = DateTime.Now,
                    Message = $"{typeof(ValuesController)}:PostCommandItem Exception => {e}"
                });
                return NotFound();
            }
        }

        private async Task<ActionResult<CommandItem>> CommandItemHandler(CommandItem item)
        {
            switch (item.Command)
            {
                case Command.OpenDoor:
                    break;
                case Command.OpenGate:
                    if (_iDataSingleton.GetSystemStatus().GateStatus.Equals(GateStatus.GateOpen))
                    {
                        if (item.CommandValue.Equals("Open") ||
                            item.CommandValue.Equals("ForceOpen"))
                        {
                            return Accepted(new CommandItem()
                            {
                                Id = item.Id,
                                Command = item.Command,
                                CommandValue = "Already Open"
                            });
                        }
                        else if (item.CommandValue.Equals("Close") ||
                                 item.CommandValue.Equals("ForceClose"))
                        {

                        }
                    }
                    else if (_iDataSingleton.GetSystemStatus().GateStatus.Equals(GateStatus.GateOpening))
                    {

                        await _openHab.PostData(_iDataSingleton.GatePathValueChange(), item.CommandValue);
                    }
                    else if (_iDataSingleton.GetSystemStatus().GateStatus.Equals(GateStatus.GateClosed))
                    {

                    }
                    else if (_iDataSingleton.GetSystemStatus().GateStatus.Equals(GateStatus.GateClosing))
                    {

                    }
                    break;
            }
        }

        /// <summary>
        /// Check only allowed values for the CommandItem.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Returns true if values are correct otherwise false.</returns>
        private bool CheckCommandItem(CommandItem item)
        {
            if (item.Command.Equals(Command.CheckUser) ||
                item.Command.Equals(Command.OpenDoor) ||
                item.Command.Equals(Command.OpenGate))
            {
                if (item.CommandValue.Equals("Open") ||
                    item.CommandValue.Equals("Close") ||
                    item.CommandValue.Equals("ForceOpen") ||
                    item.CommandValue.Equals("ForceClose") ||
                    item.CommandValue.Equals("0"))
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> CheckUserOrCreate(CommandItem item)
        {
            if (_context.Users.Any(u => u.PhoneId.Equals(item.Id)))
            {
                User user = await _context.Users.SingleAsync(u => u.PhoneId.Equals(item.Id));
                user.LastConnection = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            else if (item.Command.Equals(Command.CheckUser))
            {
                User user = new User()
                {
                    PhoneId = item.Id,
                    Name = "Unknown",
                    AccessRights = AccessRights.NotAllowed,
                    LastConnection = DateTime.Now
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            return false;
        }

        // PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, string name, [FromBody] string value)
        //{
        //}

        // DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id, string name)
        //{
        //}
    }
}
