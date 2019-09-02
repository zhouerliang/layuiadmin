using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace zhou.Admin.Controllers
{
    public class ConfigController : ControllerBase
    {
        [HttpPost]
        public string Post()
        {
            return "123456";
        }
    }
}
