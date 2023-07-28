using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using RulesEngine.Models;

namespace RulesLambda.Models
{
    public class ExecuteWorkflow
    {
        public string RuleSet { get; set; }
        public string Workflow { get; set; }

        public ExpandoObject Parameters { get; set; }
    }
}
