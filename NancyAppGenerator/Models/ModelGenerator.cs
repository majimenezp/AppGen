using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using Mono.TextTemplating;
using NancyAppGenerator.TemplateHosts;
using NancyAppGenerator.ProjectParser;
using System.IO;

namespace NancyAppGenerator.Models
{
    public class ModelGenerator
    {
        private string className;
        string[] classFields;
        List<ClassField> fields;
        public ModelGenerator(string[] fields,string className)
        {
            classFields = fields;
            this.className = className;
            this.fields = new List<ClassField>();            
        }
        public ModelDefinition Definition
        {
            get { return new ModelDefinition() {  Name=className , Fields=fields}; }
        }
        public bool ParseFields()
        {
            if (classFields.Where(x=>x.ToLower().Contains("id")).Count()==0)
            {
                // add an Id field for default identifier
                fields.Add(new ClassField() { Name = "Id", SystemType = typeof(int), TypeClass = "integer" });
            }
            foreach (string field in classFields)
            {
                string[] parts = field.Split(':');
                var tmpfield=new ClassField() { Name = parts[0], TypeClass = parts[1] };
                ParseType(tmpfield);
                fields.Add(tmpfield);
            }            
            return true;
        }

        private void ParseType(ClassField tmpfield)
        {
            switch (tmpfield.TypeClass.ToLower())
            {
                case "integer":
                    tmpfield.SystemType = typeof(int);
                    break;
                case "decimal":
                    tmpfield.SystemType = typeof(decimal);
                    break;
                case "double":
                    tmpfield.SystemType = typeof(double);
                    break;
                case "string":
                    tmpfield.SystemType = typeof(string);
                    break;
                case "date":
                case "datetime":
                    tmpfield.SystemType = typeof(DateTime);
                    break;
                case "bool":
                case "boolean":
                    tmpfield.SystemType = typeof(bool);
                    break;
                
            }
        }

        internal void GenerateClass(string currentPath)
        {
            ModelHost host1 = new ModelHost();
            Parsercsproj parseproj = new Parsercsproj(currentPath);            
            host1.ClassName = className;
            host1.ClassFields = fields;
            host1.IdField= fields.FirstOrDefault(x => x.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase));
            host1.NameSpace = parseproj.RootNameSpace + ".Models";
            string output = host1.ProcessTemplate();
            string filePath=Path.Combine(currentPath, "Models", className + ".cs");
            File.WriteAllText(filePath, output,Encoding.UTF8);
            parseproj.AddCompileFile("Models\\" + className + ".cs");
            parseproj.Save();
        }
    }
}
