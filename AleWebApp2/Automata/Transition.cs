using System;
using System.Collections.Generic;
using System.Text;

namespace Automata
{
    public class Transition
    {
        public State StartState { get; set; }
        public State EndState { get; set; }
        public Symbol Symbol { get; set; }
        public Transition(State startState, State endState, Symbol symbol)
        {
            StartState = startState;
            EndState = endState;
            Symbol = symbol;
        }
    }
}
