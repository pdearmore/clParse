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
        private class Help : CommandArgument
        {
            public override void Command(IEnumerable<IArgument> args)
            {
            }
        }

        private class EstimateArgument : NamedArgument
        {

        }

        private class CompletedSwitch : SwitchArgument
        {

        }


        [TestMethod()]
        public void ParseSingleCommand_CI_Test()
        {
            var hc = new Help();
            var lst = new List<IArgument>() { hc };
            var parser = new Parser(lst);
            var testArgs = new string[] { "help" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(1, rtn.Count);
        }

        [TestMethod()]
        public void ParseSingleCommand_CS_ShouldMatch_Test()
        {
            var hc = new Help();
            var lst = new List<IArgument>() { hc };
            var parser = new Parser(lst) { CaseSensitive = true };
            var testArgs = new string[] { "Help" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(1, rtn.Count);
        }
        
        [TestMethod()]
        public void ParseSingleCommand_CS_ShouldNotMatch_Test()
        {
            var hc = new Help();
            var lst = new List<IArgument>() { hc };
            var parser = new Parser(lst) { CaseSensitive = true };
            var testArgs = new string[] { "help" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(1, rtn.UnknownArguments.Length);
        }

        [TestMethod()]
        public void ParseNamedArgumentWithValue_Test()
        {
            var na = new EstimateArgument() { Name = "Estimate" };
            var dc = new Help() { Name = "dummyCommand" };
            var lst = new List<IArgument>() { na, dc };
            var parser = new Parser(lst);
            var testArgs = new string[] { "/estimate:5", "dummyCommand" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(2, rtn.Count);
            Assert.AreEqual("Estimate", ((NamedArgument)rtn["Estimate"]).Name);
            Assert.AreEqual("5", ((NamedArgument)rtn["Estimate"]).Value);
        }

        [TestMethod()]
        public void ParseSwitchIsTrue_Test()
        {
            var na = new EstimateArgument() { Name = "Estimate" };
            var dc = new CompletedSwitch() { Name = "Completed" };
            var ic = new CompletedSwitch() { Name = "Incomplete" };
            var lst = new List<IArgument>() { na, dc };
            var parser = new Parser(lst);
            var testArgs = new string[] { "/estimate:5", "/completed" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(false, rtn.ContainsKey("Incomplete"));
            Assert.AreEqual(true, ((SwitchArgument)rtn["Completed"]).Value);
        }

        [TestMethod()]
        public void ParseArgumentDoesntExistFails_Test()
        {
            var na = new EstimateArgument() { Name = "Estimate" };
            var dc = new Help() { Name = "dummyCommand" };
            var lst = new List<IArgument>() { na, dc };
            var parser = new Parser(lst);
            var testArgs = new string[] { "/estimate:5", "dummyCommand", "/dummySwitchFails" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(1, rtn.UnknownArguments.Length);
        }

        [TestMethod()]
        public void ParseCommandCorrectlyIdentified_Test()
        {
            var na = new EstimateArgument() { Name = "Estimate" };
            var dc = new Help() { Name = "dummyCommand" };
            var lst = new List<IArgument>() { na, dc };
            var parser = new Parser(lst);
            var testArgs = new string[] { "/estimate:5", "dummyCommand", "/dummySwitchFails" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(dc, rtn.CommandArgument);
        }

        [TestMethod()]
        public void ParseCommandReturnsNull_Test()
        {
            var na = new EstimateArgument() { Name = "Estimate" };
            var lst = new List<IArgument>() { na };
            var parser = new Parser(lst);
            var testArgs = new string[] { "/estimate:5", "/dummySwitchFails" };

            var rtn = parser.Parse(testArgs);

            Assert.IsNull(rtn.CommandArgument);
        }
    }
}