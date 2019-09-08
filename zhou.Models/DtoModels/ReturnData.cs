using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zhou.Models.DtoModels
{
    public class ReturnData<T> where T : class
    {
        public ReturnData(T data, int count = 0) : this(0, data, count)
        {
        }

        public ReturnData(int code, T data, int count)
        {
            this.code = code;
            this.msg = CodeMsg.FirstOrDefault(c => c.Key == code).Value;
            this.data = data;
            this.count = count;
        }

        public ReturnData(int code)
        {
            this.code = code;
            this.msg = CodeMsg.FirstOrDefault(c => c.Key == code).Value;
            this.data = data;
            this.count = 0;
        }

        public int code { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public T data { get; set; }

        /// <summary>
        /// 内部定义code对应msg
        /// </summary>
        private static Dictionary<int, string> CodeMsg = new Dictionary<int, string>()
        {
            {0,"OK" },
            {401,"无权限" },
            {404,"页面不见啦" },
            {500,"内部错误" }
        };
    }
}
