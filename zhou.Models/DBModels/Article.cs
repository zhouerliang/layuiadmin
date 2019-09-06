using System;
using System.Collections.Generic;
using System.Text;

namespace zhou.Models.DBModels
{
    public class Article : BaseEntity
    {
        public string A_Title { get; set; }
        public string A_Content { get; set; }
        public int A_Author { get; set; }
        public int A_Likes { get; set; }
        public int A_Readings { get; set; }
        public int A_Type { get; set; }
    }
}
