using clParse.CommandLine;
using clParse.CommandLine.Interfaces;
using clParse.Timer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace clParse.Timer
{
    public static class ConsoleMethods
    {
        public static List<IArgument> GetArguments()
        {
            var idArgument = new NamedArgument() { Name = "id" };
            var newItemCommand = new NewItemCommand() { Name = "newitem" };
            var nameArgument = new NamedArgument() { Name = "name" };
            var fullSwitch = new SwitchArgument() { Name = "full" };
            var exampleSwitch = new SwitchArgument() { Name = "example" };
            var cmdCmd = new CmdCommand() { Name = "command", Aliases = new string[] { "cmd" } };
            var helpCommand = new HelpCommand() { Name = "help", ArgumentSequence = new List<IArgument> { nameArgument } };
            var startCommand = new StartCommand() { Name = "start", RequiredArguments = new List<IArgument>() { idArgument } };

            var lst = new List<IArgument>() { helpCommand, startCommand, idArgument, newItemCommand, nameArgument, fullSwitch, exampleSwitch, cmdCmd };
            return lst;
        }

        public static string[] SplitArgs(string unsplitArgLine)
        {
            int numOfArgs;
            IntPtr ptrToSplitArgs;
            string[] splitArgs;

            if (string.IsNullOrEmpty(unsplitArgLine))
                return new string[] { };

            ptrToSplitArgs = CommandLineToArgvW(unsplitArgLine, out numOfArgs);
            if (ptrToSplitArgs == IntPtr.Zero)
                throw new ArgumentException("Unable to split",
                  new Win32Exception());
            try
            {
                splitArgs = new string[numOfArgs];
                for (int i = 0; i < numOfArgs; i++)
                    splitArgs[i] = Marshal.PtrToStringUni(
                        Marshal.ReadIntPtr(ptrToSplitArgs, i * IntPtr.Size));
                return splitArgs;
            }
            finally
            {
                LocalFree(ptrToSplitArgs);
            }
        }

        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine,
            out int pNumArgs);

        [DllImport("kernel32.dll")]
        static extern IntPtr LocalFree(IntPtr hMem);
    }
}
