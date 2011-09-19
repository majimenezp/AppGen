using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NancyAppGenerator.Models
{
    public class ClassField
    {
        public string Name { get; set; }
        public string TypeClass { get; set; }
        public bool PrimaryKey { get; set; }
        public Type SystemType { get; set; }
        public string ShortName { get { return SystemType.Name; } }
        public string MigrationColSyntax { get; set; }
        public ClassField()
        {
            Name = string.Empty;
            TypeClass = string.Empty;
            PrimaryKey = false;
            MigrationColSyntax = string.Empty;
        }
    }
}
