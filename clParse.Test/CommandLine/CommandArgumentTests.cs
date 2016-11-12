using Microsoft.VisualStudio.TestTools.UnitTesting;
using clParse.CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Enums;
using clParse.CommandLine.Interfaces;

namespace clParse.CommandLine.Tests
{
    [TestClass()]
    public class CommandArgumentTests
    {
        private class FailedTestCommand : CommandArgument
        {
            public override void Command(ArgumentDictionary args)
            {
                Status = CommandStatus.Failed;
            }
        }

        private class SuccessfulTestCommand : CommandArgument
        {
            public override void Command(ArgumentDictionary args)
            {
            }
        }

        [TestMethod()]
        public void ProcessArgument_StatusSuccess()
        {
            SuccessfulTestCommand sc = new SuccessfulTestCommand();
            var args = new ArgumentDictionary();
            args.Add("test", sc);

            Assert.AreEqual(CommandStatus.NotRun, sc.Status);

            sc.ProcessCommand(args);

            Assert.AreEqual(CommandStatus.Successful, sc.Status);
        }

        [TestMethod()]
        public void ProcessArgument_StatusFailed()
        {
            FailedTestCommand tc = new FailedTestCommand();
            var args = new ArgumentDictionary();
            args.Add("test", tc);

            Assert.AreEqual(CommandStatus.NotRun, tc.Status);

            tc.ProcessCommand(args);

            Assert.AreEqual(CommandStatus.Failed, tc.Status);
        }
    }
}