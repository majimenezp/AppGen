using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using Mono.TextTemplating;
using NancyAppGenerator.TemplateHosts;
using NancyAppGenerator.ProjectParser;
using System.IO;

namespace NancyAppGenerator.Controllers
{
    public class ControllerGenerator
    {
        private List<ActionDefinition> actions;
        
        private string className;
        Parsercsproj parseproj;
        private string currentPath;
        public ControllerGenerator(string currentPath, List<ActionDefinition> actions, string className)
        {
            // TODO: Complete member initialization
            this.actions = actions;
            this.className = className;
            this.currentPath = currentPath;
            parseproj = new Parsercsproj(currentPath);
        }

        internal void GenerateClass()
        {
            ControllerHost host = new ControllerHost();            
            host.ClassName = className;
            host.Actions = actions;
            host.NameSpace = parseproj.RootNameSpace;
            string output = host.ProcessTemplate();
            string filePath = Path.Combine(currentPath, "Controllers",className, className + ".cs");
            File.WriteAllText(filePath, output, Encoding.UTF8);
            parseproj.AddCompileFile("Controllers\\"+className +"\\"+ className + ".cs");
            parseproj.Save();            
        }
        internal void GenerateScaffoldClass(Models.ModelDefinition model)
        {
            ScaffoldControllerHost host = new ScaffoldControllerHost();
            host.ClassName = className;
            host.Model = model;
            host.NameSpace = parseproj.RootNameSpace; ;
            string output = host.ProcessTemplate();
            string filePath = Path.Combine(currentPath, "Controllers", className, className + ".cs");
            File.WriteAllText(filePath, output, Encoding.UTF8);
            parseproj.AddCompileFile("Controllers\\" + className + "\\" + className + ".cs");
            parseproj.Save();            
        }

        internal void GenerateViews()
        {
            List<string> filesList = new List<string>();
            SimpleViewHost host = new SimpleViewHost();
            foreach(var action in actions)
            {
                host.BaseName = className;
                host.ViewName = action.Route;
                string output = host.ProcessTemplate();
                string filePath = Path.Combine(currentPath, "Views", className, action.Route + ".cshtml");
                File.WriteAllText(filePath, output, Encoding.UTF8);
                parseproj.AddContentFile("Views\\" + className + "\\" + action.Route + ".cshtml", CopyOutPutOptions.PreserveNewest);
            }
            parseproj.Save();
        }
        internal void GenerateScaffoldViews(Models.ModelDefinition model)
        {
            ScaffoldViewHost host = new ScaffoldViewHost();
            host.BaseName = className;
            host.ViewName = "Index";
            host.Template = "Index.tt";
            host.Model = model;
            string output = host.ProcessTemplate();
            string filePath = Path.Combine(currentPath, "Views", className, "Index.cshtml");
            File.WriteAllText(filePath, output, Encoding.UTF8);
            parseproj.AddContentFile("Views\\" + className + "\\" + "Index.cshtml", CopyOutPutOptions.PreserveNewest);


            parseproj.Save();
        }
    }
}
