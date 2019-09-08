using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Code.CodeModel
{
    public class Table
    {
        public string ClassName { get; set; }
        public List<TableColumn> Columns { get; set; }

        public string Path { get; set; }
        public Project Model { get; set; }
        public Project Server { get; set; }
        public Project ServerImpl { get; set; }
        public Project Ctrl { get; set; }
    }

    public class Project
    {
        public string LibraryName { get; set; }
        public string FilePath { get; set; }
    }
}
