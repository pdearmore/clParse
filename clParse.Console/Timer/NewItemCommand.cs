using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine;
using clParse.CommandLine.Interfaces;

namespace clParse.Timer
{
    public class NewItemCommand : CommandArgument
    {
        public NewItemCommand() 
        {
            RequiredArguments = new List<IArgument> { new Argument() { Name = "name" } };
        }
        public override void Command(ArgumentDictionary args)
        {
            var name = ((NamedArgument)args["name"]).Value;
            Console.WriteLine($"Created a new item called {name}.");
        }
    }
}
