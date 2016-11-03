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
        string HelpDetail { get; set; }
        string HelpExample { get; set; }
        string Summary { get; set; }
    }
}
