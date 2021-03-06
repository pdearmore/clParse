﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Interfaces;

namespace clParse.CommandLine
{
    public class SwitchArgument : IArgument
    {
        public string Name { get; set; }
        public string HelpDetail { get; set; }
        public string HelpExample { get; set; }
        public string Summary { get; set; }
        public bool Value { get; set; }

        public IEnumerable<string> Aliases { get; set; }

        public SwitchArgument()
        {
            Aliases = new List<string>();
            Value = false;
        }
    }
}
