using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NancyAppGenerator.Models;
namespace NancyAppGenerator.TemplateHosts
{
    public class ScaffoldControllerHost : HostBase
    {
        public string ClassName { get; set; }
        public ModelDefinition Model { get; set; }
        public string NameSpace { get; set; }
        public override string TemplateFile()
        {
            return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates", "Controllers", "scaffoldcontroller.tt");
        }
    }
}
