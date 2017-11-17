using System;
using System.Collections.Generic;
using System.Text;

namespace Automata
{
    public class Symbol
    {
        public char Name { get; set; }

        public Symbol(char name)
        {
            Name = name;
        }

        public override string ToString() => Name.ToString();
    }
}
