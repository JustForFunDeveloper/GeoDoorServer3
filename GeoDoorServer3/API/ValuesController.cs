using System;
using System.Linq;
using System.Threading.Tasks;
using GeoDoorServer3.API.Model;
using GeoDoorServer3.CustomService;
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
            _iDataSingleton.AddErrorLog(new ErrorLog()
            {
                LogLevel = LogLevel.Debug,
                MsgDateTime = DateTime.Now,
                Message = $"{typeof(ValuesController)}:PostCommandItem => {item}"
            });

            if (!_context.Users.Any(u => u.PhoneId.Equals(item.Id)))
                return NotFound();

            User user = await _context.Users.SingleAsync(u => u.PhoneId.Equals(item.Id));

            user.LastConnection = DateTime.Now;

            if (user.AccessRights.Equals(AccessRights.NotAllowed))
                return Accepted(new CommandItem()
                {
                    Id = item.Id,
                    Command = item.Command,
                    CommandValue = AccessRights.NotAllowed.ToString()
                });
            
            await _openHab.PostData(_iDataSingleton.SetGatePath(), "ON");

            return Accepted(new CommandItem()
            {
                Id = item.Id,
                Command = item.Command,
                CommandValue = "OK"
            });
        }

        private void CommandItemHandler(CommandItem item)
        {

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
