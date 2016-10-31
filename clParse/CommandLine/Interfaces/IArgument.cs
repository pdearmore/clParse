using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clParse.CommandLine.Interfaces
{
    public interface IArgument
    {
        string Name { get; set; }
        string Help { get; set; }
        string Example { get; set; }
        string Summary { get; set; }

        void ProcessArgument(IEnumerable<IArgument> args);
    }
}
