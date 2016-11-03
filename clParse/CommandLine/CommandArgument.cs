using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Enums;
using clParse.CommandLine.Interfaces;


namespace clParse.CommandLine
{
    public abstract class CommandArgument : Argument
    {
        public CommandStatus Status { get; set; }

        public abstract void Command(IEnumerable<IArgument> args);

        public CommandArgument()
        {
            Status = CommandStatus.NotRun;
        }

        public void ProcessCommand(IEnumerable<IArgument> args)
        {
            Status = CommandStatus.Executing;

            Command(args);

            if (Status == CommandStatus.Executing)
                Status = CommandStatus.Successful;
        }
    }
}
