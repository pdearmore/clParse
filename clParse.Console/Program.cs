using clParse.CommandLine;
using clParse.CommandLine.Interfaces;
using clParse.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clParse
{
    public class Program
    {
        /// <summary>
        /// Remember to edit command line arguments in Debug tab of the clParse project properties!
        /// </summary>
        /// <param name="args">Set these prior to testing</param>
        public static void Main(string[] args)
        {
            var hc = new HelpCommand() { Name = "help" };
            var lst = new List<IArgument>() { hc };
            var parser = new Parser(lst);

            var rtn = parser.Parse(args);

            if (rtn.CommandArgument != null)
                rtn.CommandArgument.ProcessCommand(rtn);
        }
    }
}
