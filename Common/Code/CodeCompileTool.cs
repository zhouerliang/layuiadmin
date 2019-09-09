using Common.Code.CodeModel;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Code
{
    /// <summary>
    /// 代码生成
    /// </summary>
    public class CodeCompileTool
    {
        /// <summary>
        /// 创建Model
        /// 创建Services接口
        /// 创建Services实现
        /// 创建Controllers控制器
        /// </summary>
        public void Create(Table Table)
        {
            CreateModel(Table);
            CreateServer(Table);
            CreateServerImpl(Table);
        }

        //创建Model
        private void CreateModel(Table Table)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace(Table.Model.LibraryName + "." + Table.Model.FilePath);
            samples.Imports.Add(new CodeNamespaceImport("System"));
            compileUnit.Namespaces.Add(samples);

            //定义类型 类，接口，枚举等
            CodeTypeDeclaration class1 = new CodeTypeDeclaration($"{Table.ClassName}");
            class1.IsClass = true;
            class1.BaseTypes.Add("BaseEntity");//在这里声明继承关系 (基类 , 接口)
            samples.Types.Add(class1);

            //添加字段
            foreach (var item in Table.Columns)
            {
                CodeSnippetTypeMember field = new CodeSnippetTypeMember($@"
        /// <summary>
        /// {item.ColRemark}
        /// </summary>
        public {item.ColType.ToString()} {item.ColName} {{ get; set; }}");
                class1.Members.Add(field);
            }

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            //FileStream($"D:\2019\layuiadmin-zhou\zhou.Admin/User{DateTime.Now.ToString("HHmmss")}.cs", FileMode.CreateNew);
            FileStream file = new FileStream($"{Table.Path}/{Table.Model.LibraryName}/{Table.Model.FilePath}/{Table.ClassName}.cs", FileMode.CreateNew);
            using (StreamWriter sourceWriter = new StreamWriter(file))
            {
                provider.GenerateCodeFromCompileUnit(compileUnit, sourceWriter, options);
            }
        }

        private void CreateServer(Table Table)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            //命名空间
            CodeNamespace samples = new CodeNamespace(Table.Server.LibraryName + "." + Table.Server.FilePath);
            //添加引用
            samples.Imports.Add(new CodeNamespaceImport(Table.Model.LibraryName + "." + Table.Model.FilePath));
            compileUnit.Namespaces.Add(samples);

            //定义类型 类，接口，枚举名称
            CodeTypeDeclaration class1 = new CodeTypeDeclaration($"I{Table.ClassName}Repository");
            class1.IsInterface = true;
            class1.BaseTypes.Add("IDependency");//在这里声明继承关系 (基类 , 接口)
            class1.BaseTypes.Add($"IBaseRepository<{Table.ClassName}>");//在这里声明继承关系 (基类 , 接口)
            samples.Types.Add(class1);


            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            //FileStream($"D:\2019\layuiadmin-zhou\zhou.Admin/User{DateTime.Now.ToString("HHmmss")}.cs", FileMode.CreateNew);
            FileStream file = new FileStream($"{Table.Path}/{Table.Server.LibraryName}/{Table.Server.FilePath}/I{Table.ClassName}Repository.cs", FileMode.CreateNew);
            using (StreamWriter sourceWriter = new StreamWriter(file))
            {
                provider.GenerateCodeFromCompileUnit(compileUnit, sourceWriter, options);
            }
        }

        private void CreateServerImpl(Table Table)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            //命名空间
            CodeNamespace samples = new CodeNamespace(Table.ServerImpl.LibraryName + "." + Table.ServerImpl.FilePath);
            //添加引用
            samples.Imports.Add(new CodeNamespaceImport(Table.Model.LibraryName + "." + Table.Model.FilePath));
            samples.Imports.Add(new CodeNamespaceImport(Table.Server.LibraryName + "." + Table.Server.FilePath));
            compileUnit.Namespaces.Add(samples);

            //定义类型 类，接口，枚举名称
            CodeTypeDeclaration class1 = new CodeTypeDeclaration($"{Table.ClassName}Repository");
            class1.IsClass = true;
            class1.BaseTypes.Add($"BaseRepository<{Table.ClassName}>");//在这里声明继承关系 (基类 , 接口)
            class1.BaseTypes.Add($"I{Table.ClassName}Repository");//在这里声明继承关系 (基类 , 接口)
            samples.Types.Add(class1);

            CodeConstructor stringConstructor = new CodeConstructor();
            stringConstructor.Attributes = MemberAttributes.Public;
            stringConstructor.Parameters.Add(new CodeParameterDeclarationExpression("IUnitOfWork", "uow"));
            stringConstructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("uow"));
            class1.Members.Add(stringConstructor);

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            //FileStream($"D:\2019\layuiadmin-zhou\zhou.Admin/User{DateTime.Now.ToString("HHmmss")}.cs", FileMode.CreateNew);
            FileStream file = new FileStream($"{Table.Path}/{Table.ServerImpl.LibraryName}/{Table.ServerImpl.FilePath}/{Table.ClassName}Repository.cs", FileMode.CreateNew);
            using (StreamWriter sourceWriter = new StreamWriter(file))
            {
                provider.GenerateCodeFromCompileUnit(compileUnit, sourceWriter, options);
            }
        }
    }
}
