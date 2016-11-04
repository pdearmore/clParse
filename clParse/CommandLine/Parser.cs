using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Interfaces;

namespace clParse.CommandLine
{
    public class Parser
    {
        private IEnumerable<IArgument> _args;

        public IEnumerable<IArgument> Arguments { get; set; }

        public Parser(IEnumerable<IArgument> args)
        {
            _args = args;
        }

        public void Parse(string[] args)
        {
            
        }
    }
}
