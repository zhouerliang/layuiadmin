using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Common.Code
{
    public class Sample
    {
        /// <summary>
        /// 定义用于代码生成的编译单元. 
        /// </summary>
        CodeCompileUnit targetUnit;

        /// <summary>
        /// The only class in the compile unit. This class contains 2 fields,
        /// 3 properties, a constructor, an entry point, and 1 simple method. 
        /// </summary>
        CodeTypeDeclaration targetClass;

        /// <summary>
        /// The name of the file to contain the source code.
        /// </summary>
        private const string outputFileName = "SampleCode.cs";

        /// <summary>
        /// Define the class.
        /// </summary>
        public Sample()
        {
            targetUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace("CodeDOMSample");
            samples.Imports.Add(new CodeNamespaceImport("System"));
            targetClass = new CodeTypeDeclaration("CodeDOMCreatedClass");
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            samples.Types.Add(targetClass);
            targetUnit.Namespaces.Add(samples);
        }

        /// <summary>
        /// Adds two fields to the class.
        /// </summary>
        public void AddFields()
        {
            // Declare the widthValue field.
            CodeMemberField widthValueField = new CodeMemberField();
            widthValueField.Attributes = MemberAttributes.Public;
            widthValueField.Name = "widthValue";
            widthValueField.Type = new CodeTypeReference(typeof(double));
            widthValueField.Comments.Add(new CodeCommentStatement("The width of the object."));

            targetClass.Members.Add(widthValueField);

            // Declare the heightValue field
            CodeMemberField heightValueField = new CodeMemberField();
            heightValueField.Attributes = MemberAttributes.Public;
            heightValueField.Name = "heightValue";
            heightValueField.Type = new CodeTypeReference(typeof(double));
            heightValueField.Comments.Add(new CodeCommentStatement("The height of the object."));
            targetClass.Members.Add(heightValueField);


            CodeSnippetTypeMember field = new CodeSnippetTypeMember("public string abc {get;set;}");
        }
        /// <summary>
        /// Add three properties to the class.
        /// </summary>
        public void AddProperties()
        {
            // Declare the read-only Width property.
            CodeMemberProperty widthProperty = new CodeMemberProperty();
            widthProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            widthProperty.Name = "Width";
            widthProperty.HasGet = true;
            widthProperty.Type = new CodeTypeReference(typeof(double));
            widthProperty.Comments.Add(new CodeCommentStatement("The Width property for the object."));
            widthProperty.GetStatements.Add(
                new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "widthValue")));
            targetClass.Members.Add(widthProperty);

            // Declare the read-only Height property.
            CodeMemberProperty heightProperty = new CodeMemberProperty();
            heightProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            heightProperty.Name = "Height";
            heightProperty.HasGet = true;
            heightProperty.Type = new CodeTypeReference(typeof(double));
            heightProperty.Comments.Add(new CodeCommentStatement("The Height property for the object."));
            heightProperty.GetStatements.Add(
                new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "heightValue")));
            targetClass.Members.Add(heightProperty);

            // Declare the read only Area property.
            CodeMemberProperty areaProperty = new CodeMemberProperty();
            areaProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            areaProperty.Name = "Area";
            areaProperty.HasGet = true;
            areaProperty.Type = new CodeTypeReference(typeof(double));
            areaProperty.Comments.Add(new CodeCommentStatement("The Area property for the object."));

            // Create an expression to calculate the area for the get accessor 
            // of the Area property.
            CodeBinaryOperatorExpression areaExpression = new CodeBinaryOperatorExpression(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "widthValue"),
                    CodeBinaryOperatorType.Multiply,
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "heightValue"));
            areaProperty.GetStatements.Add(new CodeMethodReturnStatement(areaExpression));
            targetClass.Members.Add(areaProperty);

            //CodeMemberProperty property1 = new CodeMemberProperty();
            //property1.Name = "StringProperty";
            //property1.Type = new CodeTypeReference("System.String");
            //property1.Attributes = MemberAttributes.Public;
            //areaProperty.Type = new CodeTypeReference(typeof(double));
            //property1.HasGet = true;
            //property1.HasSet = true;

            ////property1.GetStatements.Add();
            ////property1.SetStatements.Add(new CodeMethodReturnStatement());


            //targetClass.Members.Add(property1);


        }

        /// <summary>
        /// Adds a method to the class. This method multiplies values stored 
        /// in both fields.
        /// </summary>
        public void AddMethod()
        {
            // Declaring a ToString method
            CodeMemberMethod toStringMethod = new CodeMemberMethod();
            toStringMethod.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            toStringMethod.Name = "ToString";
            toStringMethod.ReturnType = new CodeTypeReference(typeof(string));

            CodeFieldReferenceExpression widthReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Width");
            CodeFieldReferenceExpression heightReference =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Height");
            CodeFieldReferenceExpression areaReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Area");

            // Declaring a return statement for method ToString.
            CodeMethodReturnStatement returnStatement =
                new CodeMethodReturnStatement();

            // This statement returns a string representation of the width,
            // height, and area.
            string formattedOutput = "The object:" + Environment.NewLine +
                " width = {0}," + Environment.NewLine +
                " height = {1}," + Environment.NewLine +
                " area = {2}";
            returnStatement.Expression = new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression("System.String"), "Format",
                    new CodePrimitiveExpression(formattedOutput), widthReference, heightReference, areaReference);
            toStringMethod.Statements.Add(returnStatement);
            targetClass.Members.Add(toStringMethod);
        }
        /// <summary>
        /// Add a constructor to the class.
        /// </summary>
        public void AddConstructor()
        {
            // Declare the constructor
            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            // Add parameters.
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(double), "width"));
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(double), "height"));

            // Add field initialization logic
            CodeFieldReferenceExpression widthReference =
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "widthValue");
            constructor.Statements.Add(new CodeAssignStatement(widthReference,
                new CodeArgumentReferenceExpression("width")));
            CodeFieldReferenceExpression heightReference =
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "heightValue");
            constructor.Statements.Add(new CodeAssignStatement(heightReference,
                new CodeArgumentReferenceExpression("height")));
            targetClass.Members.Add(constructor);
        }


        /// <summary>
        /// Generate CSharp source code from the compile unit.
        /// </summary>
        /// <param name="filename">Output file name</param>
        public void GenerateCSharpCode(string fileName)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter(fileName))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }
        }

        /// <summary>
        /// Create the CodeDOM graph and generate the code.
        /// </summary>
        public static void Main()
        {
            Sample sample = new Sample();
            sample.AddFields();
            sample.AddProperties();
            sample.AddMethod();
            sample.AddConstructor();
            sample.GenerateCSharpCode($"SampleCode{DateTime.Now.ToString("HHmmss")}.cs");
        }
    }
}