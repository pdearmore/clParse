using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine;
using clParse.CommandLine.Interfaces;

namespace clParse.Timer
{
    public class HelpCommand : CommandArgument
    {
        public override void Command(ArgumentDictionary args)
        {
            Console.WriteLine("Called for help.");
        }
    }
}
