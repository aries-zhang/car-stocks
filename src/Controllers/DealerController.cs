using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStocks.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "dealer1", "dealer2" };
        }
    }
}
