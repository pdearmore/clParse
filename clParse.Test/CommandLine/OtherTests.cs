using Microsoft.VisualStudio.TestTools.UnitTesting;
using clParse.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Interfaces;
using clParse.Test;
using clParse.CommandLine.Exceptions;

namespace clParse.CommandLine.Tests
{
    [TestClass()]
    public class OtherTests
    {
        [TestMethod()]
        public void TestAliasesGetFullNames_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var est = new EstimateArgument() { Name = "estimate", Aliases = new List<string>() { "est" } };
            var startCmd = new StartCommand() { Name = "start", RequiredArguments = new List<IArgument>() { idArg }, Aliases = new string[] { "st" }, ArgumentSequence = new List<IArgument>() { idArg } };
            var dc = new CompletedSwitch() { Name = "Completed", Aliases = new List<string>() { "c", "comp" } };
            var lst = new List<IArgument>() { est, idArg, startCmd, dc };
            var parser = new Parser(lst);

            // Start command not passed, but should be used anyway
            var testArgs = new string[] { "st", "c" };

            parser.ReplaceAliasesWithFullNames(testArgs);

            Assert.AreEqual("start", testArgs[0]);
            Assert.AreEqual("Completed", testArgs[1]);
        }
    }
}