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
        public List<char> PushSymbols { get; set; }
        public string EndState { get; set; }

        public TransitionFunction(string startState,char inputSymbol,string end)
        {
            StartState = startState;
            InputSymbol = inputSymbol;
            StackTopSymbol = Constants.Epsilon;
            PushSymbols = new List<char>(){Constants.Epsilon};
            EndState = end;
        }

        public TransitionFunction(string startState, char inputSymbol,char stackTop, List<char> pushSymbols, string end)
        {
            StartState = startState;
            InputSymbol = inputSymbol;
            StackTopSymbol = stackTop;
            PushSymbols = pushSymbols;
            EndState = end;
        }

    }
}
