using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine;
using clParse.CommandLine.Interfaces;

namespace clParse.Timer
{
    public class HelpCommand : CommandArgument
    {
        public HelpCommand()
        {
            Summary = "Activates the 'help' feature that describes the different command line methods in detail.";
            HelpDetail = "The 'help' command should show help summaries, details, and examples based on the switches "
                + "included in the command line.  Generally, help commands should support the /full and /example "
                + "switches depending on how much information the user wants at the moment. ";
            HelpExample = "> <exefile> help should show a basic help summary. \n"
                + "> <exefile> help /full should show a more detailed help description. \n"
                + "> <exefile> help <command> should show help specific to the chosen command. ";
        }

        public override void Command(ArgumentDictionary args)
        {
            IArgument showHelpFor;

            if (args.ContainsKey("name"))
            {
                showHelpFor = args.AllArguments[((IArgumentWithValue)args["name"]).Value];
            }
            else
                showHelpFor = this;

            Console.WriteLine(showHelpFor.Summary);

            if (args.ContainsKey("full"))
                Console.WriteLine(showHelpFor.HelpDetail);
            if (args.ContainsKey("example"))
                Console.WriteLine(showHelpFor.HelpExample);

        }
    }
}
