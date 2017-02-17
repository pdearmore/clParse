using Microsoft.VisualStudio.TestTools.UnitTesting;
using clParse.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Interfaces;
using clParse.Test;

namespace clParse.CommandLine.Tests
{
    [TestClass()]
    public class ArgumentDictionaryTests
    {
        [TestMethod()]
        public void ParseCommandCorrectlyIdentified_Test()
        {
            var na = new EstimateArgument() { Name = "Estimate" };
            var dc = new SubHelp() { Name = "dummyCommand" };
            var lst = new List<IArgument>() { na, dc };
            var parser = new Parser(lst);
            var testArgs = new string[] { "/estimate:5", "dummyCommand", "/dummySwitchFails" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(dc, rtn.CommandArgument);
        }

        [TestMethod()]
        public void ParseArgumentDoesntExistAddedToUnknown_Test()
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
        public void SwitchArgumentsContainsOnlySwitches_Test()
        {
            var na = new EstimateArgument() { Name = "Estimate" };
            var dc = new Help() { Name = "dummyCommand" };
            var cs = new CompletedSwitch() { Name = "complete" };
            var ss = new StartedSwitch() { Name = "started" };
            var lst = new List<IArgument>() { cs, ss, na, dc };
            var parser = new Parser(lst);
            var testArgs = new string[] { "/estimate:5", "dummyCommand", "/complete", "/started" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(2, rtn.SwitchArguments.Length);
        }
    }
}
