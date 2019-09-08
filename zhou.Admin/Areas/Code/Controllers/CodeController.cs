using Common.Code;
using Common.Code.CodeModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using zhou.Admin.AppCode;
using zhou.Models.DBModels;

namespace zhou.Admin.Areas.Code.Controllers
{
    public class CodeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public CodeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public IActionResult GetTable()
        {
            List<ColumnInfo> columns = new List<ColumnInfo>();
            columns.Add(new ColumnInfo() { colName = "Menu", colRemark = "菜单表" });
            columns.Add(new ColumnInfo() { colName = "User", colRemark = "用户表" });
            columns.Add(new ColumnInfo() { colName = "Config", colRemark = "配置表" });

            return ApiHelper.ReturnJson(columns);
        }

        [HttpPost]
        public IActionResult CodeHelper()
        {
            string Path = _hostingEnvironment.ContentRootPath;
            Path = Path.Substring(0, Path.LastIndexOf("\\"));

            List<TableColumn> cols = new List<TableColumn>();
            cols.Add(new TableColumn()
            {
                ColName = "MenuName",
                ColType = typeof(string),
                ColRemark = "菜单名称"
            });
            cols.Add(new TableColumn()
            {
                ColName = "Sort",
                ColType = typeof(int),
                ColRemark = "排序"
            });

            Table table = new Table()
            {
                Path = Path + "/",
                Model = new Project() { LibraryName = "zhou.Models", FilePath = "DBModels" },
                Server = new Project() { LibraryName = "zhou.Services", FilePath = "Repository" },
                ServerImpl = new Project() { LibraryName = "zhou.Services", FilePath = "RepositoryImpl" },
                Ctrl = new Project() { LibraryName = "zhou.Admin", FilePath = "Controllers" },
                ClassName = "Menu" + DateTime.Now.ToString("HHmmss"),
                Columns = cols,
            };

            CodeCompileTool tool = new CodeCompileTool();
            tool.Create(table);

            return ApiHelper.ReturnJson();
        }
    }
}