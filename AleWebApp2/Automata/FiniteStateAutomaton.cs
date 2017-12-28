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
        public List<string> States { get; set; } = new List<string>();

        // a finite set of input symbols called the alphabet(Σ)
        public List<char> Alphabet { get; set; } = new List<char>();

        public string InitialState { get; set; }

        public List<string> FinalStates { get; set; } = new List<string>();

        public List<Transition> Transitions { get; set; } = new List<Transition>();

        public FiniteStateAutomaton()
        {

        }
        public FiniteStateAutomaton(
           IEnumerable<char> alphabet,
           IEnumerable<string> states,
           string initState,
           IEnumerable<string> finalStates,
           IEnumerable<Transition> transitions,
           string comment = ""
           )
        {
            Comment = comment;

            States = states.ToList();
            Alphabet = alphabet.ToList();
            Transitions = transitions.ToList();
            InitialState = initState;
            FinalStates = finalStates.ToList();
            //AddStates(states);
            //AddAlphabet(alphabet);
            //AddTransitions(transitions);
            //AddInitialState(initState);
            //AddFinalStates(finalStates);

        }
        //private void AddStates(IEnumerable<string> states)
        //{
        //    States = new List<string>();

        //    foreach (string state in states)
        //    {
        //        if (StateIsValid(state) == false)
        //            throw new Exception("Invalid DFA");

        //        States.Add(state);
        //    }
        //}

        //private void AddAlphabet(IEnumerable<char> alphabet)
        //{
        //    Alphabet = new List<char>();

        //    foreach (char c in alphabet)
        //    {
        //        if (SymbolIsValid(c) == false)
        //            throw new Exception("Invalid DFA");

        //        Alphabet.Add(c);
        //    }
        //}
        //private void AddTransitions(IEnumerable<TransitionFunction> transitions)
        //{
        //    foreach (var transition in transitions.Where(ValidTransition))
        //    {
        //        Transitions.Add(transition);
        //    }
        //}
        //private void AddInitialState(string initState)
        //{
        //    if (StateExists(initState) == false)
        //        throw new Exception("Invalid DFA");

        //    InitialState = initState;
        //}

        //private void AddFinalStates(IEnumerable<string> finStates)
        //{
        //    FinalStates = new List<string>();

        //    foreach (string finState in finStates)
        //    {
        //        if (StateExists(finState) == false)
        //            throw new Exception("Invalid DFA");

        //        FinalStates.Add(finState);
        //    }
        //}


        //private bool StateIsValid(string state)
        //{
        //    if (StateExists(state))
        //        return false;

        //    return true;

        //}

        private bool StateExists(string state)
        {
            return States.Exists(x => x == state);
        }

        //private bool SymbolIsValid(char symbol)
        //{
        //    Regex regex = new Regex("^[a-z0-9]$");
        //    return regex.IsMatch(symbol.ToString());
        //}


        //private bool ValidTransition(TransitionFunction transitionFunction)
        //{
        //    return States.Contains(transitionFunction.StartState) &&
        //           States.Contains(transitionFunction.EndState) &&
        //           Alphabet.Contains(transitionFunction.Symbol) &&
        //           TransitionExists(transitionFunction) == false;
        //}

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


        public bool Accepts(string input)
        {
            // Check if input is in alphabet
            foreach (char c in input)
            {
                if (Alphabet.Contains(c) == false)
                    return false;
            }

            return Accepts(input, new List<string> { InitialState });
        }

        private bool Accepts(string input, List<string> currentStates)
        {
            // Take all epsilon Transitions
            currentStates = EpsilonReacheableStates(currentStates);

            // Iterations Completed, check if it is accepted
            if (input.Length <= 0)
                return FinalStateExists(currentStates);

            char currentInput = input[0];

            // Take all current input transitions
            currentStates = InputReacheableStates(currentStates, currentInput);

            return Accepts(input.Substring(1), currentStates);
        }

        private bool FinalStateExists(List<string> currentStates)
        {
            return FinalStates.Intersect(currentStates.Select(x => x.ToString())).Any();
        }

        private List<string> EpsilonReacheableStates(List<string> currentStates)
        {
            List<string> result = new List<string>();

            foreach (string currentState in currentStates)
            {
                EpsilonClosureForState(currentState, result);
            }

            result = result.Distinct().ToList();

            return result;
        }

        private void EpsilonClosureForState(string currentState, List<string> states)
        {
            if (states.Contains(currentState)) return;

            states.Add(currentState);

            List<Transition> transForCurrentState = Transitions.FindAll(x => x.StartState == currentState && x.Symbol == Constants.Epsilon);
            foreach (Transition trans in transForCurrentState)
            {
                EpsilonClosureForState(trans.EndState, states);
            }
        }

        private List<string> InputReacheableStates(List<string> currentStates, char input)
        {
            List<string> result = new List<string>();

            foreach (var currentState in currentStates)
            {
                List<Transition> transitions = Transitions.FindAll(x => x.StartState == currentState && x.Symbol == input);
                foreach (var transition in transitions)
                {
                    if (result.Contains(transition.EndState) == false)
                        result.Add(transition.EndState);
                }
            }
            return result;
        }



        private static readonly string _sinkState = "Sink";
        /// <summary>
        /// Conversion NFA to DFA using Powerset construction algorithm
        /// </summary>
        public FiniteStateAutomaton ToDfa()
        {
            if (IsDFA()) return this;

            FiniteStateAutomaton dfa = new FiniteStateAutomaton();
            dfa.Alphabet = Alphabet;
            List<string> currentStates = new List<string>();
            List<string> resultingStates = new List<string>();
            List<string> notProcessedStates = new List<string>();
            string resultState;
            string currentState;
            Transition tf;

            // Initial State of the DFA is the EpsilonClosure of the Initial state of the NFA
            EpsilonClosureForState(InitialState, currentStates);
            currentStates.Sort();

            // Create Initial state as q0
            currentState = CombineStatesToString(currentStates);
            dfa.States.Add(currentState);
            dfa.InitialState = currentState;


            notProcessedStates.Add(currentState);
            bool isComplete = false;
            while (isComplete == false)
            {
                notProcessedStates.Remove(currentState);

                foreach (char c in Alphabet)
                {
                    resultingStates = InputReacheableStates(currentStates, c);

                    if (resultingStates.Any() == false)
                    // If no transition points to state with the current alphabet symbol, then it goes to the Sink
                    {
                        if (dfa.StateExists(_sinkState) == false)
                        {
                            dfa.States.Add(_sinkState);
                            notProcessedStates.Add(_sinkState);
                        }

                        tf = new Transition(currentState, _sinkState, c);
                        dfa.Transitions.Add(tf);
                        continue;
                    }

                    // Epsilon Closure for the resulting states 
                    resultingStates = EpsilonReacheableStates(resultingStates);
                    resultingStates.Sort();
                    resultState = CombineStatesToString(resultingStates);

                    if (dfa.StateExists(resultState) == false)
                    // Add new state if it doesn't already exist
                    {
                        dfa.States.Add(resultState);
                        if (resultingStates.Exists(x => FinalStates.Contains(x)))
                            dfa.FinalStates.Add(resultState);

                        notProcessedStates.Add(resultState);
                    }

                    // Create Transition
                    tf = new Transition(currentState, resultState, c);
                    dfa.Transitions.Add(tf);
                }

                if (notProcessedStates.Any())
                {
                    currentStates = DecoupleStateToListOfStrings(notProcessedStates.FirstOrDefault());
                    currentState = CombineStatesToString(currentStates);

                }
                else
                {
                    isComplete = true;
                }
            }

            return dfa;
        }


        private string CombineStatesToString(List<string> states) => string.Join("-", states);

        private List<string> DecoupleStateToListOfStrings(string state) => state.Split('-').ToList();

        public bool IsInfinite()
        {
            List<string> states = States;

            // Set A - All reacheable States:
            // state q' is said to be reachable from another state q if, 
            // there is an input string which may take us from state q to state q'.
            states = GetReacheableStates(InitialState);

            // Set B - States that reach Final States
            states = GetStatesReachingFinalState(states);

            // Set C - States that has non-epsilon loops


            return false;
        }

        /// <summary>
        /// Get All States which are reacheable from the state given as parameter
        /// </summary>
        private List<string> GetReacheableStates(string state)
        {
            // Algorithm
            // 1. Initialise the set of reachable states R to the set containing only the initial state.
            // 2. Calculate the set of states M which can be reached from a state in R in one transition. 
            //    Thus M will include all states q' such that for some input symbol a and state q in R, (q,a)  q' is a production rule.
            // 3. Update R to be the union of M and the old value of R.
            // 4. If new elements have been added to R in the last step, jump to step 2.
            // 5. Remove all states in the automaton not in the final value of R and transitions from or into them.

            List<string> reacheableStates = new List<string> { state };

            List<Transition> transitions = Transitions.Where(x => x.StartState == state).ToList();
            foreach (Transition transition in transitions)
            {
                reacheableStates.AddRange(GetReacheableStates(transition.EndState));
            }

            return reacheableStates;
        }


        /// <summary>
        /// Get states which have a transition to a Final State (reaching final state)
        /// </summary>
        private List<string> GetStatesReachingFinalState(List<string> states)
        {
            return states.Where(x => Transitions.Exists(y => y.StartState == x && FinalStates.Contains(y.EndState))).ToList();
        }

        private List<string> GetStatesWithNonEpsilonLoops(List<string> states)
        {
            // There is a path, which lead 
            return new List<string>();
        }

        public List<string> AcceptedWords()
        {
            if (IsInfinite())
                return new List<string>();

            return new List<string>();
        }

    }

}
