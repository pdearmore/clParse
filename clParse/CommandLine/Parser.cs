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

        /// <summary>
        /// Prefix is the character that precedes an argument (not a command)
        /// </summary>
        public char Prefix { get; set; }
        public bool CaseSensitive { get; set; }
        
        private string _commandSuffix;

        /// <summary>
        /// CommandSuffix is what to call a class that should be interpreted as a command-style argument
        /// </summary>
        public string CommandSuffix
        {
            get { return CaseSensitive ? _commandSuffix : _commandSuffix.ToLower() ; }
            set { _commandSuffix = value; }
        }

        private string _argumentSuffix;

        public string ArgumentSuffix
        {
            get { return CaseSensitive ? _argumentSuffix : _argumentSuffix.ToLower(); }
            set { _argumentSuffix = value; }
        }



        public IEnumerable<IArgument> Arguments { get; set; }

        public Parser(IEnumerable<IArgument> args)
        {
            _args = args;
            Delimiter = ':';
            Prefix = '/';
            CaseSensitive = false;
            CommandSuffix = "Command";
            ArgumentSuffix = "Argument";
        }

        /// <summary>
        /// Populates the Arguments IEnumerable property from the args parameter.
        /// * Get a Hashtable of the arguments for each name for easier matching
        /// * Loop through the args string and match command line strings up with argument objects
        /// * Create "Argument" of appropriate type for each matched string and add to Arguments
        /// * If an arg is found that doesn't match to an argument object, throw an exception
        /// </summary>
        /// <param name="argumentsFromCommandLine">A straight copy of the args[] from the command line of the program</param>
        public Hashtable Parse(string[] argumentsFromCommandLine)
        {
            var argObjectsHash = new Hashtable();
            var parsedArguments = new Hashtable();

            // Create a Hashtable of Arguments passed in via constructor so we can look them up
            // by name without having to loop through them.
            foreach (var arg in _args)
            {
                argObjectsHash.Add(CaseSensitive ? arg.Name : arg.Name.ToLower(), arg);
            }
            
            // Loop through command line array
            foreach (var str in argumentsFromCommandLine)
            {
                var strArgAsArray = str.Split(Delimiter);
                var strArgName = CaseSensitive ? strArgAsArray[0] : strArgAsArray[0].ToLower();
                var argumentWasFound = false;
                strArgName = strArgName.Replace(Convert.ToString(Prefix), "");
                
                // If there's a delimiter, it's a named argument
                if (strArgAsArray.Length > 1)
                {
                    var strArgValue = strArgAsArray[1];
                    var hashKey = strArgName;
                    if (argObjectsHash.Contains(hashKey))
                    {
                        var arg = (NamedArgument)argObjectsHash[hashKey];
                        argumentWasFound = true;
                        arg.Value = strArgAsArray[1];
                        parsedArguments.Add(arg.Name, arg);
                    }
                }
                else
                {
                    var hashKey = strArgName;
                    if (argObjectsHash.Contains(hashKey))
                    {
                        var arg = (CommandArgument)argObjectsHash[hashKey];
                        argumentWasFound = true;
                        arg.Name = strArgAsArray[0];
                        parsedArguments.Add(arg.Name, arg);
                    }
                }
                if (!argumentWasFound)
                    throw new ArgumentException($"{strArgName} was not a valid argument.");
            }
            return parsedArguments;
        }
    }
}
