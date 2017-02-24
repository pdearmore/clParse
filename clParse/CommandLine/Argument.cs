using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Interfaces;

namespace clParse.CommandLine
{
    public class Argument : IArgument
    {
        private string _name;
        public string Name { get { return _name ?? this.GetType().Name; } set { _name = value; } }
        public string CiName { get { return Name.ToLower(); } }
        public string FriendlyName { get; set; }
        public string HelpDetail { get; set; }
        public string HelpExample { get; set; }
        public string Summary { get; set; }

        public IEnumerable<string> Aliases { get; set; }

        public IEnumerable<IArgument> RequiredArguments;
        public IEnumerable<IArgument> PermittedArguments;
        public IEnumerable<IArgument> ArgumentSequence;

        public Argument()
        {
            RequiredArguments = new List<IArgument>();
            PermittedArguments = new List<IArgument>();
            Aliases = new List<string>();
            ArgumentSequence = null;

            HelpDetail = $"{Name} Detailed help.";
            HelpExample = $"{Name} Example help.";
            Summary = $"{Name} Summary";
        }
    }
}
