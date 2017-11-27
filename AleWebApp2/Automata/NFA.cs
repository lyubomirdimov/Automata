using System;
using System.Collections.Generic;
using System.Text;

namespace Automata
{
    public class NFA
    {
        public string Comment { get; set; }

        // a finite set of states (Q)
        public List<string> States { get; set; }

        // a finite set of input symbols called the alphabet(Σ)
        public List<char> Alphabet { get; set; }

        public string InitialState { get; set; }

        public List<string> FinalStates { get; set; }

        // a transition function(δ : Q × Σ → Q)
        public List<Transition> Transitions { get; set; } = new List<Transition>();

        public NFA()
        {
            
        }


     

       
    }
}
