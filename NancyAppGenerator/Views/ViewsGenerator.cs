using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using Mono.TextTemplating;
using NancyAppGenerator.TemplateHosts;
using NancyAppGenerator.ProjectParser;
using System.IO;

namespace NancyAppGenerator.Views
{
    class ViewsGenerator
    {
        private string currentPath;
        Parsercsproj parseproj;
        public ViewsGenerator(string currentPath)
        {
            this.currentPath = currentPath;
            parseproj = new Parsercsproj(currentPath);
        }
        public void  GenerateLayoutView()
        {
            LayoutViewHost host=new LayoutViewHost();
            string output = host.ProcessTemplate();
            string filePath = Path.Combine(currentPath, "Views","Shared","_Layout.cshtml");
            File.WriteAllText(filePath, output, Encoding.UTF8);
            parseproj.AddContentFile("Views\\" + "Shared" + "\\_Layout.cshtml", CopyOutPutOptions.PreserveNewest);
            parseproj.Save();   
        }
    }
}
