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
