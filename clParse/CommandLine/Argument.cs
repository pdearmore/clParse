﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clParse.CommandLine.Interfaces;

namespace clParse.CommandLine
{
    public class Argument : IArgument
    {
        private string _name;
        public string Name { get { return _name ?? this.GetType().Name; } set { _name = value; } }
        public string CiName { get { return Name.ToLower(); } }
        public string FriendlyName { get; set; }
        public string HelpDetail { get; set; }
        public string HelpExample { get; set; }
        public string Summary { get; set; }

        public Argument()
        {

        }
    }
}
