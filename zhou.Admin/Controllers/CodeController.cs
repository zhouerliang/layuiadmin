using Common.Code;
using Common.Code.CodeModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using zhou.Admin.AppCode;

namespace zhou.Admin.Controllers
{
    public class CodeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public CodeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public IActionResult CodeHelper()
        {
            string Path = _hostingEnvironment.ContentRootPath;
            Path = Path.Substring(0, Path.LastIndexOf("\\"));

            //拼装数据库表
            List<TableColumn> cols = new List<TableColumn>();
            cols.Add(new TableColumn("ParentID", ColType.nvarcher, "父级菜单ID"));
            cols.Add(new TableColumn("Title", ColType.nvarcher, "菜单名称"));
            cols.Add(new TableColumn("Name", ColType.nvarcher, "菜单Name,通常为视图文件名或者视图所在文件夹名"));
            cols.Add(new TableColumn("Jump", ColType.nvarcher, "菜单地址"));
            cols.Add(new TableColumn("Icon", ColType.nvarcher, "菜单图标Icon"));
            cols.Add(new TableColumn("Spread", ColType.@int, "是否默认展开：1=true，2=false"));
            cols.Add(new TableColumn("Sort", ColType.@int, "排序"));

            Table table = new Table()
            {
                Path = Path + "/",
                Model = new Project() { LibraryName = "zhou.Models", FilePath = "DBModels" },
                Server = new Project() { LibraryName = "zhou.Services", FilePath = "Repository" },
                ServerImpl = new Project() { LibraryName = "zhou.Services", FilePath = "RepositoryImpl" },
                Ctrl = new Project() { LibraryName = "zhou.Admin", FilePath = "Controllers" },
                ClassName = "Menu",
                Columns = cols,
            };

            CodeCompileTool tool = new CodeCompileTool();
            tool.Create(table);

            return ApiHelper.ReturnJson();
        }
    }
}