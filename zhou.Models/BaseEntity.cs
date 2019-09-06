using System;
using System.Collections.Generic;
using System.Text;

namespace zhou.Models
{
    /// <summary>
    /// 实体数据基类
    /// </summary>
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Status = 1;
            CreatedBy = 1;
            CreatedDate = DateTime.Now;
            UpdatedBy = 1;
            UpdatedDate = DateTime.Now;
        }

        public int ID { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
