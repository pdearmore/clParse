using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Interfaces;

namespace clParse.CommandLine
{
    public abstract class Argument : IArgument
    {
        public string Name { get; set; }
        public string Help { get; set; }
        public string Example { get; set; }
        public string Summary { get; set; }

        public abstract void ProcessArgument(IEnumerable<IArgument> args);
    }
}
