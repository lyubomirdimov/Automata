using System;
using System.Collections.Generic;
using System.Text;

namespace Automata
{
    public class TransitionFunction
    {
        public string StartState { get; set; }
        public char InputSymbol { get; set; }
        public char StackTopSymbol { get; set; }
        public List<char> PushSymbol { get; set; }
        public string EndState { get; set; }

    }
}
