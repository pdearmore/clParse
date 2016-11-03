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
            public override void Command(IEnumerable<IArgument> args)
            {
                Status = CommandStatus.Failed;
            }
        }

        private class SuccessfulTestCommand : CommandArgument
        {
            public override void Command(IEnumerable<IArgument> args)
            {
                // Just some dummy code
                var x = 1;
            }
        }

        [TestMethod()]
        public void ProcessArgument_Status()
        {
            FailedTestCommand tc = new FailedTestCommand();
            SuccessfulTestCommand sc = new SuccessfulTestCommand();

            var args = new List<IArgument>() {tc};

            Assert.AreEqual(CommandStatus.NotRun, tc.Status);

            tc.ProcessCommand(args);
            sc.ProcessCommand(args);

            Assert.AreEqual(CommandStatus.Failed, tc.Status);
            Assert.AreEqual(CommandStatus.Successful, sc.Status);
        }
    }
}