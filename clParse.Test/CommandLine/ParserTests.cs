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
    public class ParserTests
    {
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
            var est = new EstimateArgument() { Name = "estimate" };
            var lst = new List<IArgument>() { hc };
            var parser = new Parser(lst) { CaseSensitive = true };
            var testArgs = new string[] { "Help", "/Estimate:20" };

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
            var startCmd = new StartCommand() { Name = "start", Default = true };
            var lst = new List<IArgument>() { na, dc, startCmd };
            var parser = new Parser(lst);
            var testArgs = new string[] { "/estimate:5", "/completed" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(false, rtn.ContainsKey("Incomplete"));
            Assert.AreEqual(true, ((SwitchArgument)rtn["Completed"]).Value);
        }

        [TestMethod()]
        public void ParseCommandWithRequiredArgumentPasses_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var startCmd = new StartCommand() { Name = "start", RequiredArguments = new List<IArgument>() { idArg } };
            var lst = new List<IArgument>() { startCmd, idArg };
            var parser = new Parser(lst);
            var testArgs = new string[] { "start", "/id:5", "/dummySwitch" };

            var rtn = parser.Parse(testArgs);

            // (doesn't raise an exception)
            Assert.AreEqual(startCmd, rtn.CommandArgument);
        }

        [TestMethod()]
        public void ParseWithMultipleDelimitersPasses_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var est = new EstimateArgument() { Name = "estimate" };
            var startCmd = new StartCommand() { Name = "start", Default = true};
            var lst = new List<IArgument>() { est, idArg, startCmd };
            var parser = new Parser(lst) { Delimiter = new char[] {':', '='} };

            // Note that both : and = are used as delimiters
            var testArgs = new string[] { "/id:20", "/estimate=5" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual("5", ((NamedArgument)rtn["estimate"]).Value);
            Assert.AreEqual("20", ((NamedArgument)rtn["id"]).Value);
        }

        [TestMethod()]
        [ExpectedException(typeof(RequiredArgumentException))]
        public void ParseCommandWithoutRequiredArgumentFails_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var startCmd = new StartCommand() { Name = "start", RequiredArguments = new List<IArgument>() { idArg } };
            var hc = new Help();
            var lst = new List<IArgument>() { startCmd, hc, idArg };
            var parser = new Parser(lst);
            var testArgs = new string[] { "start", "/dummySwitch" };

            var rtn = parser.Parse(testArgs);
        }

        [TestMethod()]
        [ExpectedException(typeof(DefaultArgumentException))]
        public void MultipleDefaultArgumentFails_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var startCmd = new StartCommand() { Name = "start", Default = true };
            var hc = new Help() { Default = true };
            var lst = new List<IArgument>() { startCmd, hc, idArg };
            var parser = new Parser(lst);
            var testArgs = new string[] { "start", "/dummySwitch" };

            var rtn = parser.Parse(testArgs);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void MultipleCommandArgumentFails_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var startCmd = new StartCommand() { Name = "start", Default = true };
            var hc = new Help();
            var lst = new List<IArgument>() { startCmd, hc, idArg };
            var parser = new Parser(lst);
            var testArgs = new string[] { "start", "help", "/dummySwitch" };

            var rtn = parser.Parse(testArgs);
        }

        [TestMethod()]
        [ExpectedException(typeof(DefaultArgumentException))]
        public void ThrowErrorIfNoCommandOrDefaultFails_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var startCmd = new StartCommand() { Name = "start" };
            var hc = new Help();
            var lst = new List<IArgument>() { startCmd, hc, idArg };
            var parser = new Parser(lst);
            var testArgs = new string[] { "/dummySwitch" };

            var rtn = parser.Parse(testArgs);
        }

        [TestMethod()]
        public void DefaultArgumentCorrectlyUsed_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var est = new EstimateArgument() { Name = "estimate" };
            var startCmd = new StartCommand() { Name = "start", Default = true };
            var lst = new List<IArgument>() { est, idArg, startCmd };
            var parser = new Parser(lst) { Delimiter = new char[] { ':', '=' } };

            // Start command not passed, but should be used anyway
            var testArgs = new string[] { "/id:20", "/estimate=5" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(startCmd, (rtn.CommandArgument));
        }

        [TestMethod()]
        public void SequenceInterpretedCorrecty_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var est = new EstimateArgument() { Name = "estimate" };
            var startCmd = new StartCommand() { Name = "start", Default = true, ArgumentSequence = new List<IArgument>() { idArg, est } };
            var lst = new List<IArgument>() { est, idArg, startCmd };
            var parser = new Parser(lst) { Delimiter = new char[] { ':', '=' } };

            // Start command not passed, but should be used anyway
            var testArgs = new string[] { "start", "20", "5" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual("5", ((NamedArgument)rtn["estimate"]).Value);
            Assert.AreEqual("20", ((NamedArgument)rtn["id"]).Value);
        }

        [TestMethod()]
        public void SequenceInterpretedCorrectyWithDefault_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var est = new EstimateArgument() { Name = "estimate" };
            var startCmd = new StartCommand() { Name = "start", Default = true, ArgumentSequence = new List<IArgument>() { idArg, est } };
            var lst = new List<IArgument>() { est, idArg, startCmd };
            var parser = new Parser(lst) { Delimiter = new char[] { ':', '=' } };

            // Start command not passed, but should be used anyway
            var testArgs = new string[] { "20", "5" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual("5", ((NamedArgument)rtn["estimate"]).Value);
            Assert.AreEqual("20", ((NamedArgument)rtn["id"]).Value);
        }


        [TestMethod()]
        public void SeeThatAliasesAreWorking_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var est = new EstimateArgument() { Name = "estimate", Aliases = new List<string>() { "est" } };
            var startCmd = new StartCommand() { Name = "start", Aliases = new List<string>() { "st", "s"} };
            var dc = new CompletedSwitch() { Name = "Completed", Aliases = new List<string>() { "c", "comp" } };
            var lst = new List<IArgument>() { est, idArg, startCmd, dc };
            var parser = new Parser(lst);

            // Start command not passed, but should be used anyway
            var testArgs = new string[] { "s", "/est:25", "/comp" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual("25", ((NamedArgument)rtn["estimate"]).Value);
            Assert.AreEqual(true, ((SwitchArgument)rtn["completed"]).Value);
            Assert.AreEqual(startCmd, (rtn.CommandArgument));
        }

        [TestMethod()]
        public void TestAliasesWithArgumentSequence_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var est = new EstimateArgument() { Name = "estimate", Aliases = new List<string>() { "est" } };
            var startCmd = new StartCommand() { Name = "start", RequiredArguments = new List<IArgument>() { idArg }, Aliases = new string[] { "st" }, ArgumentSequence = new List<IArgument>() { idArg } };
            var dc = new CompletedSwitch() { Name = "Completed", Aliases = new List<string>() { "c", "comp" } };
            var lst = new List<IArgument>() { est, idArg, startCmd, dc };
            var parser = new Parser(lst);

            // Start command not passed, but should be used anyway
            var testArgs = new string[] { "st", "5036" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual("5036", ((NamedArgument)rtn["id"]).Value);
        }

        [TestMethod()]
        public void MultipleCommandsInArgumentSequenceShouldWork_Test()
        {
            var idArg = new IdArgument() { Name = "id" };
            var nameArg = new NamedArgument() { Name = "name" };
            var startCmd = new StartCommand() { Name = "start", RequiredArguments = new List<IArgument>() { idArg }, Aliases = new string[] { "st" }, ArgumentSequence = new List<IArgument>() { idArg } };
            var hc = new Help() { Name="help", ArgumentSequence = new List<IArgument>() { nameArg } };
            var lst = new List<IArgument>() { hc, startCmd, nameArg };
            var parser = new Parser(lst);

            // These args say "show me the help for the start command"
            // So "start" should be picked up as a value of nameArg, not another command
            var testArgs = new string[] { "help", "start" };

            var rtn = parser.Parse(testArgs);

            Assert.AreEqual(hc, rtn.CommandArgument);
            Assert.AreEqual("start", ((IArgumentWithValue)rtn["name"]).Value);
        }
    }
}