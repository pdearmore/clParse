using clParse.CommandLine.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clParse.CommandLine
{
    /// <summary>
    /// A collection of arguments keyed to the argument name.  Contains subcollections for easier access to different argument types.
    /// </summary>
    public class ArgumentDictionary : Dictionary<string, IArgument>, IDictionary
    {
        /// <summary>
        /// Arguments passed in to the parser from the command line that are not recognized in the collection of defined arguments.
        /// </summary>
        public string[] UnknownArguments { get; set; }

        public IDictionary<string, IArgument> AllArguments { get; set; }

        public ArgumentDictionary() : base(StringComparer.InvariantCultureIgnoreCase)
        {
            AllArguments = new Dictionary<string, IArgument>();
        }

        public SwitchArgument[] SwitchArguments
        {
            get
            {
                var rtn = from sa
                          in Values
                          where sa.GetType().IsSubclassOf(typeof(SwitchArgument))
                          select (SwitchArgument)sa;
                return rtn.ToArray();
            }
        }

        public CommandArgument CommandArgument {
            get {
                var rtn = from ca 
                          in Values
                          where ca.GetType().IsSubclassOf(typeof(CommandArgument))
                          select ca;
                return (CommandArgument)rtn.FirstOrDefault();
            }
        }
    }
}
