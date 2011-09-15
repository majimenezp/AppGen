using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NancyAppGenerator.Models
{
    public class ModelDefinition
    {
        public string Name { get; set; }
        public List<ClassField> Fields { get; set; }
    }
}
