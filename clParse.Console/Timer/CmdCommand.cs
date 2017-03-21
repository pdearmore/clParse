using clParse.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Exceptions;

namespace clParse.Timer
{
    public class CmdCommand : CommandArgument
    {
        public override void Command(ArgumentDictionary args)
        {
            var inputLine = "";

            System.Console.WriteLine("Going into command mode.  This mode allows you to interpret all of your \n" +
                "command arguments in the program without having to restart it.\n" +
                "Enter Q to quit.\n");

            while ((inputLine = System.Console.ReadLine()).ToLower() != "q" && inputLine.ToLower() != "quit")
            {
                var intputLineArgs = ConsoleMethods.SplitArgs(inputLine);
                var lst = ConsoleMethods.GetArguments();
                var parser = new Parser(lst);
                try
                {
                    var rtn = parser.Parse(intputLineArgs);
                    if (rtn.UnknownArguments.Length > 0)
                    {
                        System.Console.WriteLine($"Unknown argument(s) received: {String.Join(",", rtn.UnknownArguments)}");
                    }
                    else
                    {
                        rtn.CommandArgument?.ProcessCommand(rtn);
                    }
                }
                catch (DefaultArgumentException ex)
                {
                    System.Console.WriteLine("Pardon?\n");
                }
            }
        }
    }
}
