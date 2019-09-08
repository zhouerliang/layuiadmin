using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zhou.Models.DtoModels;

namespace zhou.Admin.AppCode
{
    public class ApiHelper
    {
        public static ActionResult ReturnJson(int code = 0)
        {
            var Data = new ReturnData<object>(code);
            ContentResult res = new ContentResult()
            {
                Content = JsonConvert.SerializeObject(Data),
                ContentType = "text/json;charset=utf-8;"
            };
            return res;
        }

        //通用异步返回json封装
        public static ActionResult ReturnJson<T>(T data, int count = 0) where T : class
        {
            var Data = new ReturnData<T>(data, count);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            ContentResult res = new ContentResult()
            {
                Content = JsonConvert.SerializeObject(Data, settings),
                ContentType = "text/json;charset=utf-8;"
            };
            return res;
        }
    }
}
