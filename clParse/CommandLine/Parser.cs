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

        /// <summary>
        /// Delimiter is the character that separates a named argument from its value
        /// </summary>
        public char Delimiter { get; set; }
        public bool CaseInsensitive { get; set; }

        private string _commandSuffix;

        public string CommandSuffix
        {
            get { return CaseInsensitive ? _commandSuffix.ToLower() : _commandSuffix; }
            set { _commandSuffix = value; }
        }

        private string _argumentSuffix;

        public string ArgumentSuffix
        {
            get { return CaseInsensitive ? _argumentSuffix.ToLower() : _argumentSuffix; }
            set { _argumentSuffix = value; }
        }



        public IEnumerable<IArgument> Arguments { get; set; }

        public Parser(IEnumerable<IArgument> args)
        {
            _args = args;
            Delimiter = ':';
            CaseInsensitive = true;
        }

        /// <summary>
        /// Populates the Arguments IEnumerable property from the args parameter.
        /// * Get a Hashtable of the arguments for each name for easier matching
        /// * Loop through the args string and match command line strings up with argument objects
        /// * Create "Argument" of appropriate type for each matched string and add to Arguments
        /// * If an arg is found that doesn't match to an argument object, throw an exception
        /// </summary>
        /// <param name="argumentsFromCommandLine">A straight copy of the args[] from the command line of the program</param>
        public List<IArgument> Parse(string[] argumentsFromCommandLine)
        {
            var argObjectsHash = new Hashtable();
            var parsedArguments = new List<IArgument>();

            // Create a Hashtable of Arguments passed in via constructor so we can look them up
            // by name without having to loop through them.
            foreach (var arg in _args)
            {
                argObjectsHash.Add(CaseInsensitive ? arg.GetType().Name.ToLower() : arg.GetType().Name, arg);
            }
            
            // Loop through command line array
            foreach (var str in argumentsFromCommandLine)
            {
                var strArgAsArray = str.Split(Delimiter);
                var strArgName = CaseInsensitive ? strArgAsArray[0].ToLower() : strArgAsArray[0];

                // If there's a delimiter, it's a named argument
                if (strArgAsArray.Length > 1)
                {
                    var strArgValue = strArgAsArray[1];
                    var hashKey = strArgName + ArgumentSuffix;
                    if (argObjectsHash.Contains(hashKey))
                    {
                        var arg = (IArgumentWithValue)argObjectsHash[hashKey];
                        arg.Name = strArgAsArray[0];
                        arg.Value = strArgAsArray[1];
                        parsedArguments.Add(arg);
                    }
                }
                else
                {
                    var hashKey = strArgName + CommandSuffix;
                    if (argObjectsHash.Contains(hashKey))
                    {
                        var arg = (CommandArgument)argObjectsHash[hashKey];
                        arg.Name = strArgAsArray[0];
                        parsedArguments.Add(arg);
                    }
                }
            }
            return parsedArguments;
        }
    }
}
