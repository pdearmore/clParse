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
        
        public IEnumerable<IArgument> Arguments { get; set; }

        public Parser(IEnumerable<IArgument> args)
        {
            _args = args;
            Delimiter = ':';
            Prefix = '/';
            CaseSensitive = false;
        }

        /// <summary>
        /// Populates the Arguments IEnumerable property from the args parameter.
        /// * Get a dictionary of the arguments for each name for easier matching
        /// * Loop through the args string and match command line strings up with argument objects
        /// * Create "Argument" of appropriate type for each matched string and add to Arguments
        /// * If an arg is found that doesn't match to an argument object, add it to an unknown arguments collection
        /// </summary>
        /// <param name="argumentsFromCommandLine">A straight copy of the args[] from the command line of the program</param>
        /// <returns>An ArgumentDictionary hash table-like collection with a property containing an array of 
        /// unknown strings from the command line.</returns>
        public ArgumentDictionary Parse(string[] argumentsFromCommandLine)
        {
            var argObjectsFromCommandLineHash = new Hashtable();
            var matchedArguments = new ArgumentDictionary();
            var unknownArguments = new List<string>();

            // Create a ArgumentDictionary of Arguments passed in via constructor so we can look them up
            // by name without having to loop through them.
            foreach (var arg in _args)
            {
                argObjectsFromCommandLineHash.Add(CaseSensitive ? arg.Name : arg.Name.ToLower(), arg);
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
                    if (argObjectsFromCommandLineHash.Contains(hashKey))
                    {
                        var arg = (NamedArgument)argObjectsFromCommandLineHash[hashKey];
                        argumentWasFound = true;
                        arg.Value = strArgAsArray[1];
                        matchedArguments.Add(arg.Name, arg);
                    }
                }
                // If there's no prefix character, it's a command
                else if (!strArgAsArray[0].Contains(Prefix))
                {
                    var hashKey = strArgName;
                    if (argObjectsFromCommandLineHash.Contains(hashKey))
                    {
                        var arg = (CommandArgument)argObjectsFromCommandLineHash[hashKey];
                        argumentWasFound = true;
                        matchedArguments.Add(arg.Name, arg);
                    }
                }
                // if it's neither a named argument or command, it must be a switch
                else
                {
                    var hashKey = strArgName;
                    if (argObjectsFromCommandLineHash.Contains(hashKey))
                    {
                        var arg = (SwitchArgument)argObjectsFromCommandLineHash[hashKey];
                        argumentWasFound = true;
                        arg.Value = true;
                        matchedArguments.Add(arg.Name, arg);
                    }
                }

                if (!argumentWasFound)
                {
                    unknownArguments.Add(strArgName);
                }
            }

            if (matchedArguments.CommandArgument != null)
            {
                foreach (var arg in matchedArguments.CommandArgument.RequiredArguments)
                {
                    if (!matchedArguments.ContainsKey(arg.Name))
                        throw new ArgumentException("Required argument was not specified.");
                }
            }

            matchedArguments.UnknownArguments = unknownArguments.ToArray();
            return matchedArguments;
        }
    }
}
