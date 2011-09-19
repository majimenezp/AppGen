using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
namespace NancyAppGenerator
{
    public class MigrationCompiler
    {
        string[] sourceFiles;
        List<string> sourceCode=new List<string>();
        string outputFile;

        public string PathOutPutAssembly { get { return outputFile; } }
        public MigrationCompiler(string[] files)
        {
            sourceFiles = files;
        }      
        public bool compile()
        {
            string asmPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            outputFile=Path.GetTempFileName();
            outputFile = Path.Combine(Path.GetDirectoryName(outputFile), Path.GetFileNameWithoutExtension(outputFile) + ".dll");
            foreach(var filePath in sourceFiles)
            {
                var code=File.ReadAllText(filePath);
                sourceCode.Add(code);
            }
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.ReferencedAssemblies.Add(Path.Combine(asmPath,"FluentMigrator.dll"));
            parameters.OutputAssembly = outputFile;
            var results=codeProvider.CompileAssemblyFromSource(parameters,sourceCode.ToArray());
            return results.Errors.Count == 0;
        }
    }
}
