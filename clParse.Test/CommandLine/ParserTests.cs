using Microsoft.VisualStudio.TestTools.UnitTesting;
using clParse.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Interfaces;

namespace clParse.CommandLine.Tests
{
    [TestClass()]
    public class ParserTests
    {
        private class HelpCommand : CommandArgument
        {
            public override void Command(IEnumerable<IArgument> args)
            {
                var x = 1;
            }
        }


        [TestMethod()]
        public void ParseSingleCommandTest()
        {
            var hc = new HelpCommand();
            var lst = new List<IArgument>() {hc};
            var parser = new Parser(lst);
            var testArgs = new string[] {"help"};

            parser.Parse(testArgs);

            // TODO: Write assert for number of args
        }
    }
}