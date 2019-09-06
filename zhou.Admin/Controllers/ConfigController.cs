using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.Code;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zhou.Models.DBModels;
using zhou.Services.Repository;

namespace zhou.Admin.Controllers
{
    public class ConfigController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IArticleRepository _Article;

        public ConfigController(IHostingEnvironment hostingEnvironment, IArticleRepository Article)
        {
            _hostingEnvironment = hostingEnvironment;
            _Article = Article;
        }

        [HttpPost]
        public IActionResult Post()
        {
            return Json(new { a = 1, b = 2 });
        }

        class FieldModel
        {
            public string FieldName { get; set; }
            public string Desc { get; set; }
            public Type type { get; set; }
        }

        public IActionResult test1()
        {
            List<FieldModel> list = new List<FieldModel>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(new FieldModel()
                {
                    FieldName = "Field" + i * 3,
                    Desc = "注释" + i,
                    type = i % 2 == 0 ? typeof(string) : typeof(int)
                });
            }

            //创建一个新的编译单元
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            //定义命名空间
            CodeNamespace samples = new CodeNamespace("zhou.Admin.Controllers");
            //添加引用
            samples.Imports.Add(new CodeNamespaceImport("System"));
            compileUnit.Namespaces.Add(samples);

            //定义类型 类，接口，枚举等
            CodeTypeDeclaration class1 = new CodeTypeDeclaration($"User{DateTime.Now.ToString("HHmmss")}");
            class1.IsClass = true;
            samples.Types.Add(class1);

            //声明空的构造函数
            CodeConstructor constructor1 = new CodeConstructor();
            constructor1.Attributes = MemberAttributes.Public;
            class1.Members.Add(constructor1);

            foreach (var item in list)
            {
                string FieldName = "_" + item.FieldName;

                CodeSnippetTypeMember field = new CodeSnippetTypeMember($@"
        /// <summary>
        /// {item.Desc}
        /// </summary>
        public string a_{item.FieldName} {{ get; set; }}");
                class1.Members.Add(field);
            }

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            FileStream file = new FileStream($"C:/zhouliang/a代码/Blog190901/zhou/zhou.Models/DBModels/User{DateTime.Now.ToString("HHmmss")}.cs", FileMode.CreateNew);
            using (StreamWriter sourceWriter = new StreamWriter(file))
            {
                provider.GenerateCodeFromCompileUnit(compileUnit, sourceWriter, options);
            }

            return Json(new { a = 1, b = 2 });
        }

        public IActionResult test2()
        {
            string c = _hostingEnvironment.ContentRootPath;

            return Json(new { c });
        }

        [HttpPost]
        public IActionResult AddArticle()
        {
            int id = _Article.Add(new Article()
            {
                A_Title = "这是标题",
                A_Content = "这是内容",
                A_Author = 1
            });

            return Json(new { id });
        }
    }
}
