using System;
using System.Collections.Generic;
using System.Text;

namespace Automata
{
    public class PDA
    {
        public List<string> States { get; set; }
        public List<char> InputAlphabet { get; set; }
        public List<char> StackAlphabet { get; set; }
        public List<TransitionRelation> TransionRelations { get; set; }
        public string InitialState { get; set; }
        public char InitialStackSymbol { get; set; }
        public List<string> FinalStates { get; set; }

    
        public PDA()
        {
            
        }
    }
}
