using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clParse.CommandLine.Exceptions
{
    public class RequiredArgumentException : ArgumentException
    {
        public RequiredArgumentException() : base()
        {

        }
        public RequiredArgumentException(string strMsg) : base(strMsg)
        {

        }
    }
}
