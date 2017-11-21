using System;
using System.Collections.Generic;
using System.Text;

namespace Automata
{
    public abstract class StateMachine
    {
        public string Comment { get; set; }

        // a finite set of states (Q)
        public List<string> States { get; set; }

        // a finite set of input symbols called the alphabet(Σ)
        public List<char> Alphabet { get; set; }

        public string InitialState { get; set; }

        public List<string> FinalStates { get; set; }

        public abstract bool IsFinite();
        public abstract bool IsDFA();
        public abstract bool IsNFA();
        public abstract bool IsPDA();
    }
}
