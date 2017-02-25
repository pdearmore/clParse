using clParse.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clParse.Timer
{
    public class StartCommand : CommandArgument
    {
        public StartCommand()
        {
            Summary = "Goes into a basic command mode.";
            HelpDetail = "Shows an example of how to go into a command loop.";
            HelpExample = "> <exefile> start will start the command loop until Q is pressed. ";
        }

        public override void Command(ArgumentDictionary args)
        {
            Console.WriteLine("Press any key to test.  Press 'Q' to quit.");
            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
                Console.WriteLine("Didn't hit Q!");
            }
        }
    }
}
