﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace zhou.Admin.Controllers
{
    public class HomeController : ControllerBase
    {
        public string Index()
        {
            return "Index";
        }
    }
}