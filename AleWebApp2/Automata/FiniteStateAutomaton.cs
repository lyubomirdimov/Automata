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

        public List<TransitionFunction> Transitions { get; set; }

        public FiniteStateAutomaton()
        {

        }
        public FiniteStateAutomaton(
           string comment,
           IEnumerable<char> alphabet,
           IEnumerable<string> states,
           string initState,
           IEnumerable<string> finalStates,
           IEnumerable<TransitionFunction> transitions
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

        private bool TransitionExists(TransitionFunction transitionFunction)
        {
            return Transitions.Any(t => t.StartState == transitionFunction.StartState &&
                                  t.Symbol == transitionFunction.Symbol);
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
                List<TransitionFunction> transitionFromState = Transitions.Where(x => x.StartState == state).ToList();

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

      


        public bool Accepts(string input, out string steps)
        {
            steps = "";
            return false;
        }

        private bool DFAAccepts(string input, out string steps)
        {
            string currentState = InitialState;

            // Record the steps
            StringBuilder stepsBuilder = new StringBuilder();

            foreach (var symbol in input.ToCharArray())
            {
                // Find Transition, which allows step
                TransitionFunction transitionFunction = Transitions.Find(t => t.StartState == currentState && t.Symbol == symbol);

                if (transitionFunction == null)
                {
                    steps = stepsBuilder.ToString();
                    return false;
                }

                // Go to next State
                currentState = transitionFunction.EndState;

                stepsBuilder.Append(transitionFunction + "\n");
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

        private bool NFAAccepts(string currentState, string input, StringBuilder steps)
        {
            if (input.Length > 0)
            {
                List<TransitionFunction> transitions = GetAllTransitions(currentState, input[0]);
                foreach (var transition in transitions)
                {
                    StringBuilder currentSteps = new StringBuilder(steps.ToString() + transition + "\r\n");
                    if (NFAAccepts(transition.EndState, input.Substring(1), currentSteps))
                    {
                        return true;
                    }
                }
                return false;
            }

            return FinalStates.Contains(currentState);
        }

        private List<TransitionFunction> GetAllTransitions(string currentState, char c)
        {
            return Transitions.FindAll(tf => tf.StartState == currentState && tf.Symbol == c || tf.Symbol == Constants.Epsilon);
        }

        public FiniteStateAutomaton ConvertToDfa()
        {
            if (IsDFA()) return this;

            List<char> dfaSymbols = Alphabet;
            string dfaInitialState = InitialState;
            List<string> dfaStates = new List<string> { dfaInitialState };
            List<string> dfaFinalStates = FinalStates;
            List<TransitionFunction> dfaTransitions = new List<TransitionFunction>();

            for (int i = 0; i < dfaStates.Count; i++)
            {
                string dfaCurrentState = dfaStates[i];
                foreach (var symbol in dfaSymbols)
                {
                    var result = Transitions.FindAll(t => t.StartState == dfaCurrentState && t.Symbol == symbol);
                    if (result.Count == 1)
                    {
                        dfaTransitions.Add(new TransitionFunction(dfaCurrentState, result[0].EndState, symbol));
                        if (!dfaStates.Contains(result[0].EndState))
                            dfaStates.Add(result[0].EndState);
                    }
                    if (result.Count == 0)
                    {
                        dfaTransitions.Add(new TransitionFunction(dfaCurrentState,
                            GetSink(dfaStates, dfaTransitions), symbol));
                    }
                    if (result.Count > 1)
                    {
                        string name = null; // create name to state from two states
                        foreach (var function in result)
                            name = name + function.StartState;
                        var combinedState = name;
                        dfaTransitions.Add(new TransitionFunction(dfaCurrentState, combinedState, symbol));
                        if (!dfaStates.Contains(combinedState))
                            dfaStates.Add(combinedState);
                        //need completion
                    }
                }
            }

            return new FiniteStateAutomaton("", dfaSymbols,dfaStates, dfaInitialState, dfaFinalStates, dfaTransitions);
        }
        private string GetSink(List<string> dfaStates, List<TransitionFunction> dfaTransitions)
        {

            var result = dfaStates.Find(state => state == "*");
            if (result != null)
                return result;
            else
            {
                var trapStat = "*";
                foreach (var symbol in Alphabet)
                {
                    dfaTransitions.Add(new TransitionFunction(trapStat, trapStat, symbol));
                }
                dfaStates.Add(trapStat);
                return trapStat;
            }
        }


    }

}
