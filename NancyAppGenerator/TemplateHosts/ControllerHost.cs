﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NancyAppGenerator.TemplateHosts
{
    public class ControllerHost : HostBase
    {
        public string ClassName { get; set; }
        public List<Controllers.ActionDefinition> Actions { get; set; }
        public string NameSpace { get; set; }
        public string Template { get; set; }
        public ControllerHost():base()
        {            
            Template = "controller.tt";
        }
        public override string TemplateFile()
        {
            return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Templates", "Controllers", Template);
        }
    }
}
