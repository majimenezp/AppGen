using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NancyAppGenerator.Models;

namespace NancyAppGenerator.TemplateHosts
{
    public class ScaffoldViewHost:HostBase
    {
        public string BaseName { get; set; }
        public string ViewName { get; set; }
        public string Template { get; set; }
        public ModelDefinition Model { get; set; }
        public override string TemplateFile()
        {
            return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates", "Views", "scaffold", Template);
        }
    }
}
