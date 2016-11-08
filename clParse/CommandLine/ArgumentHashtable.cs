using clParse.CommandLine.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clParse.CommandLine
{
    public class ArgumentDictionary : Dictionary<string, IArgument>
    {
        public string[] UnknownArguments { get; set; }
        public CommandArgument CommandArgument {
            get {
                var rtn = from ca 
                          in Values
                          where ca.GetType().ToString().Contains("Command")
                          select ca;
                return (CommandArgument)rtn;
            }
        }
    }
}
