using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clParse.CommandLine.Exceptions
{
    public class DefaultArgumentException : ArgumentException
    {
        public DefaultArgumentException() : base()
        {

        }
        public DefaultArgumentException(string strMsg) : base(strMsg)
        {

        }
    }
}
