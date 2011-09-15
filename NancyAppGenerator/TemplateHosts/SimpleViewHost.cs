using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NancyAppGenerator.TemplateHosts
{
    public class SimpleViewHost:HostBase
    {
        public string BaseName { get; set; }
        public string ViewName { get; set; }
        public override string TemplateFile()
        {
            return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates", "Views", "simpleview.tt");
        }
    }
}
