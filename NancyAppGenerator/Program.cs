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
                if (args[0].Equals("migrate", StringComparison.InvariantCultureIgnoreCase))
                {
                    InvokeMigration(args);
                }
                if (args[0].Equals("generate", StringComparison.InvariantCultureIgnoreCase) || args[0].Equals("g", StringComparison.InvariantCultureIgnoreCase))
                {
                    InvokeGenerator(args);
                }
            }
            else
            {
                // help text
            }
        }

        private static void InvokeGenerator(string[] args)
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

        private static void InvokeMigration(string[] args)
        {
            string dbType = string.Empty, dbconnStr = string.Empty;
            if (args.Length == 1)
            {
                Console.WriteLine("Migrator Module(Using FluentMigrator )");
                System.Console.WriteLine("  http://github.com/schambers/fluentmigrator/network");
                Console.WriteLine("Example:");
                Console.WriteLine("migrate db:sqlite conn:\"data source=localdb.sqlite\"");
                Console.WriteLine("Options:");
                Console.WriteLine("db:");
                Console.WriteLine("     REQUIRED. The database type to migrate, options: " + FluentMigrator.Runner.Processors.ProcessorFactory.ListAvailableProcessorTypes());
                Console.WriteLine("conn:");
                Console.WriteLine("     REQUIRED. The connectionString to connect to the server and execute the migration,must be inside quotations marks.");
                return;
            }
            foreach(var arg in args)
            {
                if (arg.StartsWith("db:", StringComparison.InvariantCultureIgnoreCase))
                {
                    dbType = arg.Split(':')[1];
                }
                if (arg.StartsWith("conn:", StringComparison.InvariantCultureIgnoreCase))
                {
                    dbconnStr = arg.Split(':')[1];
                }
            }
            string dirPath = Path.Combine(currentPath, "Migrations");
            if (Directory.Exists(dirPath))
            {
                string[] files = Directory.GetFiles(dirPath, "*.cs");
                if (files.Length > 0)
                {
                    MigrationCompiler compiler = new MigrationCompiler(files);
                    if (compiler.compile())
                    {
                        MigratorExecuter executer = new MigratorExecuter(compiler.PathOutPutAssembly, dbType, dbconnStr, dirPath);
                        executer.StartMigration();
                    }
                }

            }
        }


        private static void InvokeControllerGenerator(string[] args)
        {
            string className;
            List<Controllers.ActionDefinition> actions = new List<Controllers.ActionDefinition>();
            List<string> routes = new List<string>();
            if (args.Length > 2)
            {
                className = args[2];
                for (var i = 3; i < args.Length; i++)
                {
                    Controllers.ActionDefinition action = new Controllers.ActionDefinition();
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
            ModelDefinition model = new ModelDefinition();
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
                VerfifyMigrationsFolder(currentPath);
                generator.GenerateClass(currentPath);
                model = generator.Definition;
            }
            return model;
        }

        private static void InvokeScaffoldGenerator(string[] args)
        {
            ModelDefinition model;
            model = InvokeModelGenerator(args);
            InvokeControllerScaffoldGenerator(model);
        }

        private static void InvokeControllerScaffoldGenerator(ModelDefinition model)
        {
            List<Controllers.ActionDefinition> actions = new List<Controllers.ActionDefinition>();
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
            if (!Directory.Exists(Path.Combine(currentPath, "Views", "Shared", "_Layout.cshtml")))
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
        private static void VerfifyMigrationsFolder(string currentPath)
        {
            if (!Directory.Exists(Path.Combine(currentPath, "Migrations")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Migrations"));
            }
        }

        private static void VerifyAssetsFolder(string currentPath)
        {
            if (!Directory.Exists(Path.Combine(currentPath, "Assets")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Assets"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, "Assets", "js")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Assets", "js"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, "Assets", "img")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Assets", "img"));
            }
            if (!Directory.Exists(Path.Combine(currentPath, "Assets", "css")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Assets", "css"));
            }
            VerifyJsAssets(currentPath);
            VerifyCssAssets(currentPath);
        }

        private static void VerifyCssAssets(string currentPath)
        {
            string[] cssAssets = new string[] { "style.css" };
            VerifyAndCheckAssets(currentPath, "css", cssAssets);
        }

        private static void VerifyJsAssets(string currentPath)
        {
            string[] jsAssets = new string[] { "common.js", "jquery-1.6.2.min-vsdoc.js", "jquery-1.6.2.min.js" };
            VerifyAndCheckAssets(currentPath, "js", jsAssets);
        }

        private static void VerifyAndCheckAssets(string currentPath, string assetFolder, string[] files)
        {
            string AssetsPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates", "Assets", assetFolder);

            Parsercsproj parserProj = new Parsercsproj(currentPath);
            foreach (string asset in files)
            {
                if (!File.Exists(Path.Combine(currentPath, "Assets", assetFolder, asset)))
                {
                    File.Copy(Path.Combine(AssetsPath, asset), Path.Combine(currentPath, "Assets", assetFolder, asset));
                    parserProj.AddContentFile("Assets\\" + assetFolder + "\\" + asset, CopyOutPutOptions.PreserveNewest);
                }
            }
            parserProj.Save();
        }

        private static void VerifyAssetsController(string currentPath)
        {
            VerifyControllersFolder(currentPath);
            if (!File.Exists(Path.Combine(currentPath, "Controller", "AssetsController.cs")))
            {
                Controllers.ControllerGenerator generator = new Controllers.ControllerGenerator(currentPath);
                generator.GenerateAssetClass();
            }
        }
    }
}
