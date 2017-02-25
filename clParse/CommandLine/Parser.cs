using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Interfaces;
using clParse.CommandLine.Exceptions;

namespace clParse.CommandLine
{
    public class Parser
    {
        private IEnumerable<IArgument> _args;

        /// <summary>
        /// Delimiter is the character that separates a named argument from its value
        /// </summary>
        public char[] Delimiter { get; set; }

        /// <summary>
        /// Prefix is the character that precedes an argument (not a command)
        /// </summary>
        public char Prefix { get; set; }
        public bool CaseSensitive { get; set; }
        
        public IEnumerable<IArgument> Arguments { get; set; }

        public Parser(IEnumerable<IArgument> args)
        {
            _args = args;
            Delimiter = new char[] { ':' };
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
            var codeObjectsHash = new Hashtable();
            var matchedArguments = new ArgumentDictionary();
            var unknownArguments = new List<string>();
            IEnumerable<IArgument> activeSequence;

            // Create a ArgumentDictionary of Arguments passed in via constructor so we can look them up
            // by name without having to loop through them.
            foreach (var arg in _args)
            {
                codeObjectsHash.Add(CaseSensitive ? arg.Name : arg.Name.ToLower(), arg);
                matchedArguments.AllArguments.Add(arg.Name, arg);

                // Add a copy for each alias for this argument into the collection
                foreach (var alias in arg.Aliases)
                {
                    codeObjectsHash.Add(CaseSensitive ? alias : alias.ToLower(), arg);
                }
            }

            // Find the command object first so that we can parse unnamed sequence command line styles.
            var possibleCommandArgs = _args.OfType<CommandArgument>();
            var matchedCommandArgs = new ArgumentDictionary();

            // Find a command argument, or if not, use the default
            var defaultCommand = from c in possibleCommandArgs where c.Default == true select c;
            if (defaultCommand.Count() > 1)
                throw new DefaultArgumentException("There can be only one default argument.");

            foreach (var str in argumentsFromCommandLine)
            {
                var hashKey = CaseSensitive ? str : str.ToLower();
                if (codeObjectsHash.Contains(hashKey))
                {
                    var arg = (CommandArgument)codeObjectsHash[hashKey];
                    matchedArguments.Add(arg.Name, arg);
                    matchedCommandArgs.Add(arg.Name, arg);
                }
            }
            if (matchedCommandArgs.Count > 1)
                throw new ArgumentException("There cannot be more than one command.");
            else if (matchedCommandArgs.Count < 1)
            {
                if (defaultCommand.Count() < 1)
                    throw new DefaultArgumentException("No commands supplied and no default specified.");
                else
                {
                    var arg = defaultCommand.FirstOrDefault();
                    matchedArguments.Add(arg.Name, arg);
                    matchedCommandArgs.Add(arg.Name, arg);
                }
            }

            activeSequence = ((CommandArgument)matchedCommandArgs.First().Value).ArgumentSequence;
            if (matchedCommandArgs.Count == 1
                && (activeSequence != null))
            {
                // Using an argument sequence, so remove command argument for proper indexing
                argumentsFromCommandLine = argumentsFromCommandLine.Where(val => val != matchedCommandArgs.First().Key).ToArray();
            }

            // Loop through command line array
            for (int commandIndex = 0; commandIndex < argumentsFromCommandLine.Length; commandIndex++)
            {
                var str = argumentsFromCommandLine[commandIndex];
                string[] strArgAsArray;

                strArgAsArray = str.Split(Delimiter[0]);
                if (Delimiter.Length > 1)
                {
                    for (int i = 0; i < Delimiter.Length; i++)
                    {
                        if (str.Contains(Delimiter[i]))
                        {
                            strArgAsArray = str.Split(Delimiter[i]);
                            break;
                        }
                    }
                }

                var strArgName = CaseSensitive ? strArgAsArray[0] : strArgAsArray[0].ToLower();
                var argumentWasFound = false;
                strArgName = strArgName.Replace(Convert.ToString(Prefix), "");
                
                // If there's a delimiter, it's a named argument
                if (strArgAsArray.Length > 1)
                {
                    var strArgValue = strArgAsArray[1];
                    var hashKey = strArgName;
                    if (codeObjectsHash.Contains(hashKey))
                    {
                        var arg = (NamedArgument)codeObjectsHash[hashKey];
                        argumentWasFound = true;
                        arg.Value = strArgAsArray[1];
                        matchedArguments.Add(arg.Name, arg);
                    }
                }
                // If there's no prefix character, and no sequence, it's a command
                else if (!strArgAsArray[0].Contains(Prefix) && activeSequence == null)
                {
                    var hashKey = strArgName;
                    if (codeObjectsHash.Contains(hashKey))
                    {
                        var arg = (CommandArgument)codeObjectsHash[hashKey];
                        argumentWasFound = true;
                        if (!matchedArguments.ContainsKey(arg.Name))
                            matchedArguments.Add(arg.Name, arg);
                    }
                }
                // if it's neither a named argument or command, and it contains a prefix, it must be a switch
                else if (strArgAsArray[0].Contains(Prefix))
                {
                    var hashKey = strArgName;
                    if (codeObjectsHash.Contains(hashKey))
                    {
                        var arg = (SwitchArgument)codeObjectsHash[hashKey];
                        argumentWasFound = true;
                        arg.Value = true;
                        matchedArguments.Add(arg.Name, arg);
                    }
                }
                // If there's a sequence, it's probably a sequential argument
                else if (activeSequence != null)
                {
                    var hashKey = activeSequence.ElementAt(commandIndex).Name;
                    if (codeObjectsHash.Contains(hashKey))
                    {
                        var arg = (IArgumentWithValue)codeObjectsHash[hashKey];
                        argumentWasFound = true;
                        arg.Value = strArgAsArray[0];
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
                        throw new RequiredArgumentException($"Required argument '{arg.Name}' was not specified.");
                }
            }

            matchedArguments.UnknownArguments = unknownArguments.ToArray();
            return matchedArguments;
        }
    }
}
