using clParse.CommandLine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clParse.CommandLine
{
    public class NamedArgument : Argument, IArgumentWithValue
    {
        public string Value { get; set; }
    }
}
