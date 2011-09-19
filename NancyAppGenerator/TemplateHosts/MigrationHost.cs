using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using System.IO;
using System.CodeDom.Compiler;
using Mono.TextTemplating;

namespace NancyAppGenerator.TemplateHosts
{
    public class MigrationHost: HostBase
    {
        public string ClassName { get; set; }
        public List<Models.ClassField> ClassFields { get; set; }
        public Models.ClassField IdField { get; set; }
        public string NameSpace { get; set; }
        public string MigrationSet { get; set; }
        public override string TemplateFile()
        {
            return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates", "Migrations", "create.tt");
        }
    }
}
