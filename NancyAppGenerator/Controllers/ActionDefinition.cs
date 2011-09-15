using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NancyAppGenerator.Controllers
{
    public class ActionDefinition
    {
        public string Route { get; set; }
        public HTTPVerbs HttpMethod { get; set; }
        public string View { get; set; }
        public ActionDefinition()
        {
            Route = string.Empty;
            HttpMethod = HTTPVerbs.Get;
            View = string.Empty;
        }
    }
    
}
