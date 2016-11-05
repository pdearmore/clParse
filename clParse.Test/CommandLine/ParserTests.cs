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

        private class EstimateArgument : NamedArgument
        {

        }


        [TestMethod()]
        public void ParseSingleCommand_CI_Test()
        {
            var hc = new HelpCommand();
            var lst = new List<IArgument>() { hc };
            var parser = new Parser(lst);
            var testArgs = new string[] { "help" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(1, rtn.Count);
        }

        [TestMethod()]
        public void ParseSingleCommand_CS_ShouldMatch_Test()
        {
            var hc = new HelpCommand();
            var lst = new List<IArgument>() { hc };
            var parser = new Parser(lst) { CaseSensitive = true };
            var testArgs = new string[] { "Help" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(1, rtn.Count);
        }

        [TestMethod()]
        public void ParseSingleCommand_CS_ShouldNotMatch_Test()
        {
            var hc = new HelpCommand();
            var lst = new List<IArgument>() { hc };
            var parser = new Parser(lst) { CaseSensitive = true };
            var testArgs = new string[] { "help" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(0, rtn.Count);
        }
        [TestMethod()]
        public void ParseNamedArgumentWithValue_Test()
        {
            var na = new NamedArgument();
            var lst = new List<IArgument>() { na };
            var parser = new Parser(lst);
            var testArgs = new string[] { "/estimate:5" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(1, rtn.Count);
            Assert.AreEqual("Estimate", ((NamedArgument)rtn[0]).Name);
            Assert.AreEqual("5", ((NamedArgument)rtn[0]).Value);
        }
    }
}