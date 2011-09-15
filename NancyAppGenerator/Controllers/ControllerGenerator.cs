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
            this.actions = actions;
            this.className = className;
            this.currentPath = currentPath;
            parseproj = new Parsercsproj(currentPath);
        }
        public ControllerGenerator(string currentPath)
        {
            this.actions = new List<ActionDefinition>();
            this.className = string.Empty;
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
            string filePath = Path.Combine(currentPath, "Controllers", className + ".cs");
            File.WriteAllText(filePath, output, Encoding.UTF8);
            parseproj.AddCompileFile("Controllers\\" + className + ".cs");
            parseproj.Save();            
        }
        internal void GenerateScaffoldClass(Models.ModelDefinition model)
        {
            ScaffoldControllerHost host = new ScaffoldControllerHost();
            host.ClassName = className;
            host.Model = model;
            host.NameSpace = parseproj.RootNameSpace; ;
            string output = host.ProcessTemplate();
            string filePath = Path.Combine(currentPath, "Controllers", className + ".cs");
            File.WriteAllText(filePath, output, Encoding.UTF8);
            parseproj.AddCompileFile("Controllers\\"  + className + ".cs");
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
            CreateScaffoldView(model, "Index", "Index.tt");
            CreateScaffoldView(model, "New", "New.tt");
            CreateScaffoldView(model, "Edit", "Edit.tt");
            CreateScaffoldView(model, "_form", "_form.tt");
            parseproj.Save();
        }

        private void CreateScaffoldView(Models.ModelDefinition model,string ViewName,string TemplateName)
        {
            ScaffoldViewHost host = new ScaffoldViewHost();
            host.BaseName = className;
            host.ViewName = ViewName;
            host.Template = TemplateName;
            host.Model = model;
            string output = host.ProcessTemplate();
            string filePath = Path.Combine(currentPath, "Views", className, ViewName+".cshtml");
            File.WriteAllText(filePath, output, Encoding.UTF8);
            parseproj.AddContentFile("Views\\" + className + "\\" + ViewName+".cshtml", CopyOutPutOptions.PreserveNewest);
        }

        internal void GenerateAssetClass()
        {
            ControllerHost host = new ControllerHost();
            host.Template = "assets.tt";
            host.NameSpace = parseproj.RootNameSpace;            
            string filePath = Path.Combine(currentPath, "Controllers", "AssetsController.cs");
            if (!File.Exists(filePath))
            {
                string output = host.ProcessTemplate();
                File.WriteAllText(filePath, output, Encoding.UTF8);
                parseproj.AddCompileFile("Controllers\\AssetsController.cs");
                parseproj.Save();
            }
        }
    }
}
