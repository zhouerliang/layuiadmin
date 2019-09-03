using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Code;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace zhou.Admin.Controllers
{
    public class ConfigController : ControllerBase
    {
        [HttpPost]
        public string Post()
        {
            Sample.Main();

            return "123456";
        }
    }
}
