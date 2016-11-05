using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clParse.CommandLine.Interfaces
{
    public interface IArgumentWithValue : IArgument
    {
        string Value { get; set; }
    }
}
