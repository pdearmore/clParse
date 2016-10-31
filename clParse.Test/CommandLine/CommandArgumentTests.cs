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
    public class CommandArgumentTests
    {
        private class TestCommand : CommandArgument
        {
            public bool Success { get; set; }

            public TestCommand()
            {
                Success = false;
            }

            public override void ProcessArgument(IEnumerable<IArgument> args)
            {
                Success = true;
            }
        }

        [TestMethod()]
        public void ProcessArgumentTest()
        {
            var tc = new TestCommand();
            var args = new List<IArgument>() {tc};

            tc.ProcessArgument(args);

            Assert.AreEqual(true, tc.Success);
        }
    }
}