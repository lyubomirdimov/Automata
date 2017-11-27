using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Automata
{
    public class DFA : IFiniteStateAutomaton
    {
        public string Comment { get; set; }
        public List<string> States { get; set; }
        public List<char> Alphabet { get; set; }
        public string InitialState { get; set; }
        public List<string> FinalStates { get; set; }
        public List<TransitionFunction> TransitionFunctions { get; set; }

        public DFA()
        {

        }

        public DFA(string comment,
          IEnumerable<string> states,
          IEnumerable<char> alphabet,
          IEnumerable<TransitionFunction> transitions,
          string initState,
          IEnumerable<string> finalStates)
        {
            Comment = comment;

            AddStates(states);
            AddAlphabet(alphabet);
            AddTransitions(transitions);
            AddInitialState(initState);
            AddFinalStates(finalStates);

        }
        private void AddStates(IEnumerable<string> states)
        {
            States = new List<string>();

            foreach (string state in states)
            {
                if (StateIsValid(state) == false)
                    throw new Exception("Invalid DFA");

                States.Add(state);
            }
        }

        private void AddAlphabet(IEnumerable<char> alphabet)
        {
            Alphabet = new List<char>();

            foreach (char c in alphabet)
            {
                if (SymbolIsValid(c) == false)
                    throw new Exception("Invalid DFA");

                Alphabet.Add(c);
            }
        }
        private void AddTransitions(IEnumerable<TransitionFunction> transitions)
        {
            foreach (var transition in transitions.Where(ValidTransition))
            {
                TransitionFunctions.Add(transition);
            }
        }
        private void AddInitialState(string initState)
        {
            if (StateExists(initState) == false)
                throw new Exception("Invalid DFA");

            InitialState = initState;
        }

        private void AddFinalStates(IEnumerable<string> finStates)
        {
            FinalStates = new List<string>();

            foreach (string finState in finStates)
            {
                if (StateExists(finState) == false)
                    throw new Exception("Invalid DFA");

                FinalStates.Add(finState);
            }
        }


        private bool StateIsValid(string state)
        {
            return StateExists(state) == false;
        }

        private bool StateExists(string state)
        {
            return States.Exists(x => x == state);
        }

        private bool SymbolIsValid(char symbol)
        {
            Regex regex = new Regex("^[a-z0-9]$");
            return regex.IsMatch(symbol.ToString());
        }


        private bool ValidTransition(TransitionFunction transitionFunction)
        {
            return States.Contains(transitionFunction.StartState) &&
                   States.Contains(transitionFunction.EndState) &&
                   Alphabet.Contains(transitionFunction.Symbol) &&
                   TransitionExists(transitionFunction) == false;
        }

        private bool TransitionExists(TransitionFunction transitionFunction)
        {
            return TransitionFunctions.Any(t => t.StartState == transitionFunction.StartState &&
                                  t.Symbol == transitionFunction.Symbol);
        }

        public bool IsDFA()
        {
            if (States.Any() == false) return false;
            if (Alphabet.Any() == false) return false;
            if (TransitionFunctions.Any() == false) return false;
            if (InitialState == null) return false;
            if (FinalStates.Any() == false) return false;

            foreach (string state in States)
            {
                List<TransitionFunction> transitionFromState = TransitionFunctions.Where(x => x.StartState == state).ToList();

                if (transitionFromState.Any() == false)
                    return false;

                // Check if there exist two transitions, which have the same symbol, hence non-determistic Automata
                if (transitionFromState.Exists(x => transitionFromState.Exists(y => y != x && x.Symbol == y.Symbol)))
                    return false;


                foreach (char c in Alphabet)
                {
                    // Check if there is an outgoing arrow for each symbol from each state
                    if (transitionFromState.Exists(x => x.Symbol == c) == false)
                        return false;
                }
            }

            return true;
        }

        public bool IsFinite()
        {
            /*
               * 
               * Theorem. The language accepted by a DFA M with n states is infinite 
               * if and only if M accepts a string of length k, where n ≤ k< 2n
               *
               * This makes the decision problem simple:
               * try all strings of length at least n and less than 2n and answer
               * "yes" if M accepts one of them and
               * "no" if there's no string in that range that's accepted.
              */

            throw new NotImplementedException();
        }

        public bool Accepts(string currentState, string input/*, out string steps*/)
        {
            // Record the steps
            StringBuilder stepsBuilder = new StringBuilder();

            foreach (var symbol in input.ToCharArray())
            {
                // Find Transition, which allows step
                TransitionFunction transitionFunction = TransitionFunctions.Find(t => t.StartState == currentState && t.Symbol == symbol);

                if (transitionFunction == null)
                {
                    //steps = stepsBuilder.ToString();
                    return false;
                }

                // Go to next State
                currentState = transitionFunction.EndState;

                stepsBuilder.Append(transitionFunction + "\n");
            }

            if (FinalStates.Contains(currentState))
            {
                //steps = stepsBuilder.ToString();
                return true;
            }

            // Failure
            //steps = stepsBuilder.ToString();
            return false;
        }


    }
}
