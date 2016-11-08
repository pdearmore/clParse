using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Interfaces;

namespace clParse.CommandLine
{
    public abstract class SwitchArgument : IArgument
    {
        public string Name { get; set; }
        public string HelpDetail { get; set; }
        public string HelpExample { get; set; }
        public string Summary { get; set; }
        public bool Value { get; set; }

        public SwitchArgument()
        {
            Value = false;
        }
    }
}
