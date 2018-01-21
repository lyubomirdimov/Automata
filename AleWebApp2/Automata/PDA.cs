using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata
{
    public class PDA
    {
        public string Comment { get; set; } 
        public List<char> InputAlphabet { get; set; } = new List<char>();
        public List<char> StackAlphabet { get; set; } = new List<char>();
        public List<string> States { get; set; } = new List<string>();
        public List<TransitionFunction> Transitions { get; set; } = new List<TransitionFunction>();
        public string InitialState { get; set; } 
        public List<string> FinalStates { get; set; } = new List<string>();
        public Stack<char> InitStack { get; set; } = new Stack<char>();

        public bool Accepts(string input)
        {
            // Check if input is in alphabet
            foreach (char c in input)
            {
                if (InputAlphabet.Contains(c) == false)
                    return false;
            }

            return Accepts(input, InitialState, InitStack);
        }

        private bool Accepts(string input, string currentState, Stack<char> stack)
        {
            TransitionFunction function;
            char popedSymbol;
            List<char> pushSymbols;
            char currentInput;
            currentInput = string.IsNullOrEmpty(input) ? Constants.Epsilon : input[0];

            if (stack.Any() && currentInput != Constants.Epsilon)
            // 1. transition whose [symbol + pop stack] matches the [current input symbol + current top stack] (if the stack is not empty)
            {
                popedSymbol = stack.Pop();
                function = Transitions.Find(x => x.StartState == currentState && x.InputSymbol == currentInput && x.StackTopSymbol == popedSymbol);
                if (function != null)
                {
                    pushSymbols = function.PushSymbols.Where(c => c != Constants.Epsilon).ToList();
                    pushSymbols.ForEach(stack.Push);
                    return Accepts(input.Substring(1), function.EndState, stack);
                }

                // Push the poped symbol back to the stack
                stack.Push(popedSymbol);
            }

            if (currentInput != Constants.Epsilon)
            {
                // 2.transition with epsilon pop stack whose symbol matches the current input symbol
                function = Transitions.Find(x => x.StartState == currentState && x.InputSymbol == currentInput && x.StackTopSymbol == Constants.Epsilon);
                if (function != null)
                {
                    pushSymbols = function.PushSymbols.Where(c => c != Constants.Epsilon).ToList();
                    pushSymbols.ForEach(stack.Push);
                    return Accepts(input.Substring(1), function.EndState, stack);
                }
            }


            if (stack.Any())
            // 3.transition with epsilon symbol whose pop stack matches the current top stack(if the stack is not empty)
            {
                popedSymbol = stack.Pop();
                function = Transitions.Find(x => x.StartState == currentState && x.InputSymbol == Constants.Epsilon && x.StackTopSymbol == popedSymbol);
                if (function != null)
                {
                    pushSymbols = function.PushSymbols.Where(c => c != Constants.Epsilon).ToList();
                    pushSymbols.ForEach(stack.Push);
                    return Accepts(input, function.EndState, stack);
                }

                // Push the poped symbol back to the stack
                stack.Push(popedSymbol);
            }

            // 4.transition with epsilon symbol and epsilon pop stack
            function = Transitions.Find(x => x.StartState == currentState && x.InputSymbol == Constants.Epsilon && x.StackTopSymbol == Constants.Epsilon);
            if (function != null)
            {
                pushSymbols = function.PushSymbols.Where(c => c != Constants.Epsilon).ToList();
                pushSymbols.ForEach(stack.Push);
                return Accepts(input, function.EndState, stack);
            }

            // Iterations Completed, check if it is accepted
            if (input.Length <= 0)
            {
                if (IsFinalState(currentState) && !stack.Any())
                    return true;

                //function = Transitions.Find(x => x.StartState == currentState && x.InputSymbol == Constants.Epsilon && x.StackTopSymbol == Constants.Epsilon);
                //if (function == null)
                //    return IsFinalState(currentState) && !stack.Any();

                //pushSymbols = function.PushSymbols.Where(c => c != Constants.Epsilon).ToList();
                //pushSymbols.ForEach(stack.Push);
                //return Accepts(input, function.EndState, stack);
            }

            // Halted
            return false;

        }


        private bool IsFinalState(string state) => FinalStates.Contains(state);


        //public override string ToString()
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendLine(Comment);
        //    builder.AppendLine();
        //    builder.AppendLine($"alphabet: {string.Join(",", InputAlphabet)}");
        //    builder.AppendLine($"states: {String.Join(",", States.ToArray())}");
        //    builder.AppendLine($"final: {String.Join(",", FinalStates.ToArray())}");
        //    builder.AppendLine($"transitions:");
        //    foreach (var transition in Transitions)
        //    {
        //        builder.AppendLine(transition.ToString());
        //    }
        //    builder.AppendLine("end.");

        //    builder.AppendLine();
        //    builder.AppendLine("dfa:n");
        //    builder.AppendLine("finite:n");


        //    return builder.ToString();
        //}
    }
}
