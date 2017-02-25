using clParse.CommandLine;
using clParse.CommandLine.Interfaces;
using clParse.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clParse
{
    public class Program
    {
        /// <summary>
        /// Remember to edit command line arguments in Debug tab of the clParse project properties!
        /// </summary>
        /// <param name="args">Set these prior to testing</param>
        public static void Main(string[] args)
        {
            // Test combos:
            // start /id:1
            // help
            // newitem /name:"New Item" /estimate:5 /complete

            var idArgument = new NamedArgument() { Name = "id" };
            var newItemCommand = new NewItemCommand() { Name = "newitem" };
            var nameArgument = new NamedArgument() { Name = "Name" };
            var fullSwitch = new SwitchArgument() { Name = "full" };
            var exampleSwitch = new SwitchArgument() { Name = "example" };
            var helpCommand = new HelpCommand() { Name = "help", ArgumentSequence = new List<IArgument> { nameArgument } };
            var startCommand = new StartCommand() { Name = "start", RequiredArguments = new List<IArgument>() { idArgument } };
             
            var lst = new List<IArgument>() { helpCommand, startCommand, idArgument, newItemCommand, nameArgument, fullSwitch, exampleSwitch };

            var parser = new Parser(lst);

            var rtn = parser.Parse(args);

            if (rtn.CommandArgument != null)
                rtn.CommandArgument.ProcessCommand(rtn);
        }
    }
}
