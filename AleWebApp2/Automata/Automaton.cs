using System;
using System.Collections.Generic;
using System.Text;

namespace Automata
{
    public class Automaton
    {
        public string Comment { get; set; }
        public List<Symbol> Alphabet { get; set; }
        public List<State> States { get; set; }
        public List<State> FinalStates { get; set; }
        public List<Transition> Transitions { get; set; }
    }
}
