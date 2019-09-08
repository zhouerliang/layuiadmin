using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Code.CodeModel
{
    public class TableColumn
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public Type ColType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string ColRemark { get; set; }
    }
}
