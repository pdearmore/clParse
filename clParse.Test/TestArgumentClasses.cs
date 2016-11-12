using clParse.CommandLine;
using clParse.CommandLine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clParse.Test
{
    public class Help : CommandArgument
    {
        public override void Command(ArgumentDictionary args)
        {
        }
    }

    public class StartCommand : CommandArgument
    {
        public override void Command(ArgumentDictionary args)
        {
        }
    }

    public class IdArgument : NamedArgument
    {

    }

    public class EstimateArgument : NamedArgument
    {

    }

    public class CompletedSwitch : SwitchArgument
    {

    }

    public class StartedSwitch : CompletedSwitch
    {

    }

    public class SubHelp : Help
    {

    }


}
