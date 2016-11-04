using System;
using System.Collections;
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

        /// <summary>
        /// Populates the Arguments IEnumerable property from the args parameter.
        /// * Get a Hashtable of the arguments for each name for easier matching
        /// * Loop through the args string and match command line strings up with argument objects
        /// * Create "Argument" of appropriate type for each matched string and add to Arguments
        /// * If an arg is found that doesn't match to an argument object, throw an exception
        /// </summary>
        /// <param name="args">A straight copy of the args[] from the command line of the program</param>
        public void Parse(string[] args)
        {
            var argHash = new Hashtable();

            foreach (var arg in _args)
            {
                argHash.Add(arg.GetType().ToString(), arg);
            }

            // Match CommandArguments
            foreach (var str in args)
            {
                
            }
        }
    }
}
