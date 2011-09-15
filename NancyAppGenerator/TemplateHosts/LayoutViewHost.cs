using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NancyAppGenerator.TemplateHosts
{
    public class LayoutViewHost:HostBase
    {
        
       public override string TemplateFile()
        {
           return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates", "Views", "layout.tt");
        }
    }
}
