using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Microsoft.VisualStudio.TextTemplating;
using NancyAppGenerator.ProjectParser;
using NancyAppGenerator.Models;
using NancyAppGenerator.Controllers;
namespace NancyAppGenerator
{
    class Program
    {
        static string currentPath;
        static void Main(string[] args)
        {
            currentPath = Environment.CurrentDirectory;
            if (args.Length > 0)
            {
                if (args[0].Equals("struct", StringComparison.InvariantCultureIgnoreCase))
                {

                    GenerateFolderStruct(currentPath);
                }
                InvokeGenerator(args);
            }
            else
            {
                // help text
            }
        }

        private static void InvokeGenerator(string[] args)
        {
            if (args[0].Equals("generate", StringComparison.InvariantCultureIgnoreCase) || args[0].Equals("g", StringComparison.InvariantCultureIgnoreCase))
            {
                if (args.Length > 1)
                {
                    switch (args[1].ToLower())
                    {
                        case "controller":
                            InvokeControllerGenerator(args);               
                            break;
                        case "model":
                            InvokeModelGenerator(args);
                            break;
                        case "resource":
                            break;
                        case "scaffold":
                            InvokeScaffoldGenerator(args);
                            break;
                        case "scaffold_controller":
                            break;
                        case "css":
                        case "stylesheets":
                            break;
                    }
                }
            }
        }

       

        private static void InvokeControllerGenerator(string[] args)
        {
            string className;
            List<Controllers.ActionDefinition> actions=new List<Controllers.ActionDefinition>();
            List<string> routes=new List<string>();
            if (args.Length > 2)
            {
                className = args[2];
                for (var i = 3; i < args.Length; i++)
                {
                    Controllers.ActionDefinition action= new Controllers.ActionDefinition();
                    action.HttpMethod = Controllers.HTTPVerbs.Get;
                    action.Route = args[i];
                    action.View = args[i];
                    actions.Add(action);
                }
                Controllers.ControllerGenerator generator = new Controllers.ControllerGenerator(currentPath, actions, className);
                VerifyControllersFolder(currentPath);
                VerifyControllerSubFolder(currentPath, className);
                generator.GenerateClass();
                VerifyViewSubFolder(currentPath, className);
                VerifiViewLayout(currentPath);
                generator.GenerateViews();
            }
            
        }

        private static ModelDefinition InvokeModelGenerator(string[] args)
        {
            string className;
            ModelDefinition model=new ModelDefinition();
            List<string> fields = new List<string>();
            if (args.Length > 2)
            {
                className = args[2];
                for (var i = 3; i < args.Length; i++)
                {
                    if (args[i].Contains(':'))
                    {
                        fields.Add(args[i]);
                    }
                }
                Models.ModelGenerator generator = new Models.ModelGenerator(fields.ToArray(), className);
                generator.ParseFields();
                VerfifyModelsFolder(currentPath);
                generator.GenerateClass(currentPath);
                model = generator.Definition;
            }
            return model;
        }

        private static void InvokeScaffoldGenerator(string[] args)
        {
            ModelDefinition model;
            model=InvokeModelGenerator(args);
            InvokeControllerScaffoldGenerator(model);
        }

        private static void InvokeControllerScaffoldGenerator(ModelDefinition model)
        {
             List<Controllers.ActionDefinition> actions=new List<Controllers.ActionDefinition>();
             Controllers.ControllerGenerator generator = new Controllers.ControllerGenerator(currentPath, actions, model.Name);
             VerifyControllersFolder(currentPath);
             VerifyAssetsController(currentPath);
             //VerifyControllerSubFolder(currentPath, model.Name);
             VerifyAssetsFolder(currentPath);
             generator.GenerateScaffoldClass(model);
             VerifyViewSubFolder(currentPath, model.Name);
             VerifiViewLayout(currentPath);
             generator.GenerateScaffoldViews(model);
            
        }

        private static void VerifiViewLayout(string currentPath)
        {
            if (!Directory.Exists(Path.Combine(currentPath, "Views", "Shared")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Views", "Shared"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, "Views", "Shared","_Layout.cshtml")))
            {
                Views.ViewsGenerator generator = new Views.ViewsGenerator(currentPath);
                generator.GenerateLayoutView();
            }
        }

        private static void VerifyViewSubFolder(string currentPath, string className)
        {
            if (!Directory.Exists(Path.Combine(currentPath, "Views", className)))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Views", className));
            }
        }

        private static void VerifyControllerSubFolder(string currentPath, string className)
        {
            if (!Directory.Exists(Path.Combine(currentPath, "Controllers", className)))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Controllers", className));
            }
        }        

        private static void GenerateFolderStruct(string currentPath)
        {
            if (!Directory.Exists(Path.Combine(currentPath, "Views")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Views"));
                Directory.CreateDirectory(Path.Combine(currentPath, "Views", "Shared"));
            }
            else
            {
                VerifyViewShared(currentPath);
            }
            VerifyControllersFolder(currentPath);
            VerfifyModelsFolder(currentPath);
        }

        private static void VerifyViewShared(string currentPath)
        {
            if (!Directory.Exists(Path.Combine(currentPath, "Views", "Shared")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Views", "Shared"));
            }
        }

        private static void VerifyControllersFolder(string currentPath)
        {
            if (!Directory.Exists(Path.Combine(currentPath, "Controllers")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Controllers"));
            }
        }

        private static void VerfifyModelsFolder(string currentPath)
        {
            if (!Directory.Exists(Path.Combine(currentPath, "Models")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Models"));
            }
        }

        private static void VerifyAssetsFolder(string currentPath)
        {
            if (!Directory.Exists(Path.Combine(currentPath, "Assets")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Assets"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, "Assets","js")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Assets","js"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, "Assets", "img")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Assets", "img"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, "Assets", "css")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Assets", "css"));
            }
        }

        private static void VerifyAssetsController(string currentPath)
        {
            VerifyControllersFolder(currentPath);
            if (!File.Exists(Path.Combine(currentPath, "Controller","AssetsController.cs")))
            {
                Controllers.ControllerGenerator generator = new Controllers.ControllerGenerator(currentPath);
                generator.GenerateAssetClass();
            }
        }
    }
}
