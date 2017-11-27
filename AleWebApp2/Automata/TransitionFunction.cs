using System;
using System.Collections.Generic;
using System.Text;

namespace Automata
{
    public class TransitionFunction
    {
        public string StartState { get; set; }
        public string EndState { get; set; }
        public char Symbol { get; set; }
        public TransitionFunction(string startState, string endState, char symbol)
        {
            StartState = startState;
            EndState = endState;
            Symbol = symbol;
        }
        public override string ToString()
        {
            return string.Format("({0}, {1}) -> {2}", StartState, Symbol, EndState);
        }
    }
}
