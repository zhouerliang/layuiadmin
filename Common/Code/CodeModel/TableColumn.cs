using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Code.CodeModel
{
    public class TableColumn
    {
        public TableColumn(string ColName, ColType ColType, string ColRemark = "")
        {
            this.ColName = ColName;
            this.ColType = TypeConvert.ChangeToCSharpType(ColType.ToString());
            this.ColRemark = ColRemark;
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string ColType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string ColRemark { get; set; }
    }

    public enum ColType
    {
        nvarcher,
        @int,
        datetime
    }
}
