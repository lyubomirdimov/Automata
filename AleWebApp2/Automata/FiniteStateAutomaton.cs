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

        public List<string> States { get; set; } = new List<string>();

        public List<char> Alphabet { get; set; } = new List<char>();

        public string InitialState { get; set; }

        public List<string> FinalStates { get; set; } = new List<string>();

        public List<Transition> Transitions { get; set; } = new List<Transition>();

     
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

        }
        public FiniteStateAutomaton()
        {

        }

        public bool IsDFA()
        {
            if (States.Any() == false) return false;
            if (InitialState == null) return false;
            if (FinalStates.Any() == false) return false;
            if (Alphabet.Any() == false && Transitions.Any()) return false;

            foreach (string state in States)
            {
                List<Transition> transitionFromState = Transitions.Where(x => x.StartState == state).ToList();

                if (transitionFromState.Any() == false && Alphabet.Any())
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
            if (Alphabet.Any() == false && States.Any())
            {
                // Empty Input accepting DFA
                return new FiniteStateAutomaton()
                {
                    Alphabet = new List<char>(),
                    States = new List<string>() { "0" },
                    InitialState = "0",
                    FinalStates = new List<string>() { "0" },
                };
            }

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
            
            currentState = CombineStatesToString(currentStates);
            dfa.States.Add(currentState);
            dfa.InitialState = currentState;
            if (currentStates.Exists(x => FinalStates.Contains(x)))
                dfa.FinalStates.Add(currentState);

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

            // Fix States Names
            List<string> newStateNames = new List<string>();
            string newInitialState = "";
            List<string> newFinalStates = new List<string>();
            for (var i = 0; i < dfa.States.Count; i++)
            {
                string dfaState = dfa.States[i];
                string newStateName = i.ToString();

                newStateNames.Add(newStateName);

                if (dfa.InitialState == dfaState)
                    newInitialState = newStateName;

                if (dfa.FinalStates.Contains(dfaState))
                    newFinalStates.Add(newStateName);

                List<Transition> transWithStartState = dfa.Transitions.Where(x => x.StartState == dfaState).ToList();
                foreach (Transition transition in transWithStartState)
                {
                    transition.StartState = newStateName;
                }

                List<Transition> transWithEndState = dfa.Transitions.Where(x => x.EndState == dfaState).ToList();
                foreach (Transition transition in transWithEndState)
                {
                    transition.EndState = newStateName;
                }

            }
            dfa.States = newStateNames;
            dfa.InitialState = newInitialState;
            dfa.FinalStates = newFinalStates;
            return dfa;
        }

        private string CombineStatesToString(List<string> states) => string.Join("-", states);

        private List<string> DecoupleStateToListOfStrings(string state) => state.Split('-').ToList();



        public bool IsInfinite()
        {
            // Set A - All reacheable States:
            // state q' is said to be reachable from another state q if, 
            // there is an input string which may take us from state q to state q'.
             List<string> recheable = new List<string>();
            GetReacheableStates(InitialState, recheable);

            // Set B - States that reach Final States
            List<string> terminating = new List<string>(recheable);
            terminating = GetStatesReachingFinalState(terminating);

            // Set C - States that has non-epsilon loops
            return CheckForCycles(terminating);

        }

        /// <summary>
        /// Get All States which are reacheable from the state given as parameter
        /// </summary>
        private void GetReacheableStates(string state, List<string> states)
        {
            // Algorithm
            // 1. Initialise the set of reachable states R to the set containing only the initial state.
            // 2. Calculate the set of states M which can be reached from a state in R in one transition. 
            //    Thus M will include all states q' such that for some input symbol a and state q in R, (q,a)  q' is a production rule.
            // 3. Update R to be the union of M and the old value of R.
            // 4. If new elements have been added to R in the last step, jump to step 2.
            // 5. Remove all states in the automaton not in the final value of R and transitions from or into them.

            if (states.Contains(state))
                return;

            states.Add(state);

            List<Transition> transitions = Transitions.Where(x => x.StartState == state).ToList();
            foreach (Transition transition in transitions)
            {
                GetReacheableStates(transition.EndState, states);
            }
        }


        /// <summary>
        /// Get states which have a path to a Final State (reaching final state)
        /// </summary>
        private List<string> GetStatesReachingFinalState(List<string> states)
        {
            List<string> result = new List<string>();

            List<string> finalStates = states.Where(IsFinalState).ToList();

            foreach (string state in finalStates)
            {
                GetPreceedingStates(state, result, states);
            }

            return result;
        }

        private void GetPreceedingStates(string state, List<string> resultingStates, List<string> reacheableStates)
        {
            if (resultingStates.Contains(state))
                return;

            if (reacheableStates.Contains(state))
                resultingStates.Add(state);

            List<Transition> transitions = TransitionsToState(state);
            foreach (Transition transition in transitions)
            {
                GetPreceedingStates(transition.StartState, resultingStates, reacheableStates);
            }

        }

        private bool CheckForCycles(List<string> states)
        {
            List<string> workingSet = states.ToList();
            List<string> inRecursionSet = new List<string>();
            List<string> totallyVisitedSet = new List<string>();

            int count = 0;
            while (workingSet.Any())
            {
                string current = workingSet[0];
                List<Transition> currentPath = new List<Transition>();
                if (Dfs(current, workingSet, inRecursionSet, totallyVisitedSet, states, currentPath))
                {
                    return true;
                }
            }
            return false;
        }

        private bool Dfs(string current, 
            List<string> workingSet, 
            List<string> inRecursionSet, 
            List<string> totallyVisitedSet, 
            List<string> terminatingStates, 
            List<Transition> currentPath)
        {
            MoveVertex(current, workingSet, inRecursionSet);
            List<Transition> transitionsFromState = Transitions.Where(x => x.StartState == current && terminatingStates.Contains(x.EndState)).ToList();
            foreach (var trans in transitionsFromState)
            {
                currentPath.Add(trans);

                if (inRecursionSet.Contains(trans.EndState))
                {
                    if (!IsEpsilonCycle(trans.EndState, currentPath)) return true;

                    currentPath.Remove(trans);
                    continue;
                }

                if (Dfs(trans.EndState, workingSet, inRecursionSet, totallyVisitedSet, terminatingStates, new List<Transition>(currentPath)))
                {
                    return true;
                }
                else
                {
                    currentPath.Remove(trans);
                }
            }

            MoveVertex(current, inRecursionSet, totallyVisitedSet);
            return false;
        }

        private bool IsEpsilonCycle(string to, List<Transition> currentPath)
        {
            Transition start = currentPath.Find(x => x.IsFrom(to));
            bool fin = true;
            List<Transition> alreadyObserved = new List<Transition>();
            while (fin)
            {
                if (start.Symbol != Constants.Epsilon)
                    return false;

                start = currentPath.Find(x => x.IsFrom(start.EndState) && alreadyObserved.Contains(x) == false);
                alreadyObserved.Add(start);

                if (start.IsFrom(to))
                    fin = false;
            }

            return true;
        }


        private void MoveVertex(string vertex, List<string> source, List<string> destination)
        {
            source.Remove(vertex);
            destination.Add(vertex);
        }

        public List<string> AcceptedWords()
        {

            if (IsInfinite())
                return new List<string>();

            List<string> result = new List<string>();
            string currentInput = "";


            List<string> recheable = new List<string>();
            GetReacheableStates(InitialState, recheable);
            List<string> terminating = new List<string>(recheable);
            terminating = GetStatesReachingFinalState(terminating);


            AcceptedWords(InitialState, currentInput, result, terminating, new List<string>());

            return result;
        }

        /// <summary>
        /// Returns all accepted words. 
        /// Important notice: Method assumes that the NFA is Finite
        /// </summary>
        private void AcceptedWords(string state, string currentInput, List<string> words, List<string> terminating, List<string> visited)
        {
            if (IsFinalState(state) && words.Contains(currentInput) == false)
                words.Add(currentInput);

            visited.Add(state);

            // Start From Initial State
            List<Transition> transitions = Transitions.Where(x => x.IsFrom(state)
                                                            && terminating.Contains(x.StartState)
                                                            && terminating.Contains(x.EndState)).ToList();
            foreach (Transition transition in transitions)
            {
                if(visited.Contains(transition.EndState))
                    continue;

                AcceptedWords(transition.EndState, currentInput + transition.SymbolToString(), words, terminating, new List<string>(visited));
            }

        }



        #region Helpers


        private bool StateExists(string state)
        {
            return States.Exists(x => x == state);
        }
        private bool FinalStateExists(List<string> currentStates)
        {
            return FinalStates.Intersect(currentStates.Select(x => x.ToString())).Any();
        }


        private bool TransitionExists(Transition transition)
        {
            return Transitions.Any(t => t.StartState == transition.StartState &&
                                        t.Symbol == transition.Symbol);
        }
        private List<Transition> TransitionsFromState(string state)
        {
            return Transitions.Where(x => x.IsFrom(state)).ToList();
        }

        private List<Transition> TransitionsToState(string state)
        {
            return Transitions.Where(x => x.IsTo(state)).ToList();
        }

        private bool IsFinalState(string state) => FinalStates.Contains(state);

        #endregion
    }

}
