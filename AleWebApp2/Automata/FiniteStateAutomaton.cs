using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Automata
{
    public class FiniteStateAutomaton
    {
        public string Comment { get; set; }

        // a finite set of states (Q)
        public List<string> States { get; set; }

        // a finite set of input symbols called the alphabet(Σ)
        public List<char> Alphabet { get; set; }

        public string InitialState { get; set; }

        public List<string> FinalStates { get; set; }

        public List<Transition> Transitions { get; set; }

        public FiniteStateAutomaton()
        {

        }
        public FiniteStateAutomaton(string comment,
           IEnumerable<string> states,
           IEnumerable<char> alphabet,
           IEnumerable<Transition> transitions,
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
        private void AddTransitions(IEnumerable<Transition> transitions)
        {
            foreach (var transition in transitions.Where(ValidTransition))
            {
                Transitions.Add(transition);
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
            if (StateExists(state))
                return false;

            return true;

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


        private bool ValidTransition(Transition transition)
        {
            return States.Contains(transition.StartState) &&
                   States.Contains(transition.EndState) &&
                   Alphabet.Contains(transition.Symbol) &&
                   TransitionExists(transition) == false;
        }

        private bool TransitionExists(Transition transition)
        {
            return Transitions.Any(t => t.StartState == transition.StartState &&
                                  t.Symbol == transition.Symbol);
        }

        public bool IsDFA()
        {
            if (States.Any() == false) return false;
            if (Alphabet.Any() == false) return false;
            if (Transitions.Any() == false) return false;
            if (InitialState == null) return false;
            if (FinalStates.Any() == false) return false;

            foreach (string state in States)
            {
                List<Transition> transitionFromState = Transitions.Where(x => x.StartState == state).ToList();

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
            return IsDFA() ? IsDFAFinite()
                           : IsNFAFinite();
        }

        private bool IsDFAFinite()
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
        private bool IsNFAFinite()
        {
            throw new NotImplementedException();
        }


        public bool AcceptsInput(string input, out string steps)
        {
            return IsDFA() ? DFAAcceptInput(input, out steps)
                                    : NFAAcceptInput(input, out steps);
        }

        private bool DFAAcceptInput(string input, out string steps)
        {
            string currentState = InitialState;

            // Record the steps
            StringBuilder stepsBuilder = new StringBuilder();

            foreach (var symbol in input.ToCharArray())
            {
                // Find Transition, which allows step
                Transition transition = Transitions.Find(t => t.StartState == currentState && t.Symbol == symbol);

                if (transition == null)
                {
                    steps = stepsBuilder.ToString();
                    return false;
                }

                // Go to next State
                currentState = transition.EndState;

                stepsBuilder.Append(transition + "\n");
            }

            if (FinalStates.Contains(currentState))
            {
                steps = stepsBuilder.ToString();
                return true;
            }

            // Failure
            steps = stepsBuilder.ToString();
            return false;
        }

        private bool NFAAcceptInput(string input, out string steps)
        {
            /*
             * Start From Initial State
             * Check each possible path one by one
             * If there is Match then return True
             * If there was No path which resulted in a Match then NFA does NOT accept the input
             */


            string currentState = InitialState;
            char currentSymbol = input[0];
            List<Transition> paths = Transitions.Where(t => t.StartState == currentState && (t.Symbol == currentSymbol || t.Symbol.ToToken().Type == TokenType.Epsion)).ToList();

            foreach (Transition path in paths)
            {
                
            }

            // Record the steps
            StringBuilder stepsBuilder = new StringBuilder();

            foreach (var symbol in input.ToCharArray())
            {

                // Find Transition, which allows step
                List<Transition> transitions = Transitions.Where(t => t.StartState == currentState && (t.Symbol == symbol || t.Symbol.ToToken().Type == TokenType.Epsion)).ToList();


                foreach (Transition transition in transitions)
                {

                }
                if (transition == null)
                {
                    steps = stepsBuilder.ToString();
                    return false;
                }

                // Go to next State
                currentState = transition.EndState;

                stepsBuilder.Append(transition + "\n");
            }

            // TODO
            steps = null;
            return false;
        }

        private bool CheckPath(string input, string state)
        {
            
        }
    }

}
