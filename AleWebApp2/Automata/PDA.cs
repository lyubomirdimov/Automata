﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata
{
    public class PDA
    {
        public List<char> InputAlphabet { get; set; }
        public List<char> StackAlphabet { get; set; }
        public List<string> States { get; set; }
        public List<TransitionFunction> Transitions { get; set; }
        public string InitialState { get; set; }
        public List<string> FinalStates { get; set; } = new List<string>();

        public bool Accepts(string input)
        {
            // Check if input is in alphabet
            foreach (char c in input)
            {
                if (InputAlphabet.Contains(c) == false)
                    return false;
            }

            return Accepts(input, InitialState, new Stack<char>());
        }

        private bool Accepts(string input, string currentState, Stack<char> stack)
        {
            TransitionFunction function;
            char popedSymbol;
            List<char> pushSymbols;

            // Iterations Completed, check if it is accepted
            if (input.Length <= 0)
            {
                if (stack.Any())
                    return false;

                if (IsFinalState(currentState))
                    return true;

                // Check for Epsilon Closure
                function = Transitions.Find(x => x.StartState == currentState && x.InputSymbol == Constants.Epsilon && x.StackTopSymbol == Constants.Epsilon);
                if (function == null)
                    return IsFinalState(currentState) && !stack.Any();

                pushSymbols = function.PushSymbols.Where(c => c != Constants.Epsilon).ToList();
                pushSymbols.ForEach(stack.Push);
                return Accepts(input, function.EndState, stack);
            }

            char currentInput = input[0];

            if (stack.Any())
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

            // 2.transition with epsilon pop stack whose symbol matches the current input symbol
            function = Transitions.Find(x => x.StartState == currentState && x.InputSymbol == currentInput);
            if (function != null)
            {
                pushSymbols = function.PushSymbols.Where(c => c != Constants.Epsilon).ToList();
                pushSymbols.ForEach(stack.Push);
                return Accepts(input.Substring(1), function.EndState, stack);
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
                    return Accepts(input.Substring(1), function.EndState, stack);
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

            return Accepts(input, currentState, stack);

        }


        private bool IsFinalState(string state) => FinalStates.Contains(state);
    }
}
