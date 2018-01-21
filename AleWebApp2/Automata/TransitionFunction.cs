using System;
using System.Collections.Generic;
using System.Linq;
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

        public TransitionFunction()
        {

        }
        public TransitionFunction(string startState, char inputSymbol, string end)
        {
            StartState = startState;
            InputSymbol = inputSymbol;
            StackTopSymbol = Constants.Epsilon;
            PushSymbols = new List<char>() { Constants.Epsilon };
            EndState = end;
        }

        public TransitionFunction(string startState, char inputSymbol, char stackTop, List<char> pushSymbols, string end)
        {
            StartState = startState;
            InputSymbol = inputSymbol;
            StackTopSymbol = stackTop;
            PushSymbols = pushSymbols;
            EndState = end;
        }

        public override string ToString()
        {
            return $"{StartState},{InputSymbol} [{StackTopSymbol}/{String.Join(",", PushSymbols.Select(o => o.ToString()).ToArray())}] --> {EndState}";
        }

        public string ToPrefixString()
        {
            char inputSymbol = InputSymbol == Constants.Epsilon ? '_' : InputSymbol;
            char stackTopSymbol = StackTopSymbol == Constants.Epsilon ? '_' : StackTopSymbol;
            List<char> pushSymbols = new List<char>();
            if (PushSymbols.Count == 1 && PushSymbols.Contains(Constants.Epsilon))
            {
                pushSymbols.Add('_');
            }
            else
            {
                pushSymbols = PushSymbols;
            }
            return $"{StartState},{inputSymbol} [{stackTopSymbol},{String.Join(",", pushSymbols.Select(o => o.ToString()).ToArray())}] --> {EndState}";
        }

        public string ToTransitionCaption()
        {
            return $"{InputSymbol}[{StackTopSymbol}/{String.Join(",", PushSymbols.Select(o => o.ToString()).ToArray())}]";
        }
    }
}
