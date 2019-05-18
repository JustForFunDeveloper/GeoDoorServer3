using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GeoDoorServer3.API.Model;
using GeoDoorServer3.CustomService;
using GeoDoorServer3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GeoDoorServer3.API
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly UserDbContext _context;
        private readonly IOpenHabMessageService _openHab;

        public ValuesController(UserDbContext context, ILogger<ValuesController> logger, IOpenHabMessageService openHab)
        {
            _context = context;
            _logger = logger;
            _openHab = openHab;

            _items.Add(new CommandItem()
            {
                Id = 102,
                CommandName = "whatever",
                Command = Command.CheckUser
            });
        }

        private List<CommandItem> _items = new List<CommandItem>();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] {"value1", "value2"};
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommandItem>> GetCommandItem(int id)
        {
            _logger.LogError($"--- {DateTime.Now}: GetCommandItem id=> {id}");
            await _openHab.GetData("dsa");
            var commandItem = await GetItemAsync(id);
            if (null != commandItem)
                return commandItem;
            else
            {
                return NotFound();
            }
        }

        private Task<CommandItem> GetItemAsync(int id)
        {
            return Task.Run(() => _items.Find(e => e.Id.Equals(id)));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<CommandItem>> PostCommandItem(CommandItem item)
        {
            _logger.LogError(
                $"--- {DateTime.Now}: Post id=> {item.Id} | CommandName=> {item.CommandName} | Command=> {item.Command}");
            await PutItemAsync(item);
            await _openHab.PostData("whatever");

            return Accepted(new CommandItem()
            {
                Id = 815,
                Command = Command.CheckUser,
                CommandName = "got it!!!"
            });
        }

        private Task PutItemAsync(CommandItem item)
        {
            return Task.Run(() => _items.Add(item));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, string name, [FromBody] string value)
        {
            _logger.LogError($"--- {DateTime.Now}: Put name=> {name} | id=> {id}");
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id, string name)
        {
            _logger.LogError($"--- {DateTime.Now}: Delete name=> {name} | id=> {id}");
        }
    }
}
