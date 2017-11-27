using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Automata
{
    public class NFA : IFiniteStateAutomaton
    {
        public string Comment { get; set; }
        public List<string> States { get; set; }
        public List<char> Alphabet { get; set; }
        public string InitialState { get; set; }
        public List<string> FinalStates { get; set; }
        public List<TransitionFunction> TransitionFunctions { get; set; }

        public NFA()
        {

        }
        public NFA(string comment,
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



        public bool IsFinite()
        {
            // TODO
            return false;
        }

        public bool Accepts(string currentState,string input/*, out string steps*/)
        {
            if (input.Length > 0)
            {
                List<TransitionFunction> transitions = GetAllTransitions(currentState, input[0]);
                foreach (var transition in transitions)
                {
                    //StringBuilder currentSteps = new StringBuilder(steps.ToString() + transition + "\r\n");
                    if (Accepts(transition.EndState, input.Substring(1)/*, out steps*/))
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
            return TransitionFunctions.FindAll(tf => tf.StartState == currentState && tf.Symbol == c || tf.Symbol == Constants.Epsilon);
        }

        public DFA ConvertToDfa()
        {

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
                    var result = TransitionFunctions.FindAll(t => t.StartState == dfaCurrentState && t.Symbol == symbol);
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

            return new DFA("", dfaStates, dfaSymbols, dfaTransitions, dfaInitialState, dfaFinalStates);
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
