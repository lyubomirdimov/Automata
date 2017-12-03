using System;
using System.Collections.Generic;
using System.Linq;
using Automata;
using Newtonsoft.Json;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<char> alphabet = new List<char> { '1', '0' };

            List<string> states = new List<string>();
            string q0 = "q0";
            string q2 = "q2";
            string q1 = "q1";
            states.Add(q0);
            states.Add(q2);
            states.Add(q1);


            List<TransitionFunction> transitions = new List<TransitionFunction>();
            TransitionFunction tr1 = new TransitionFunction(q0, q0, '1');
            TransitionFunction tr2 = new TransitionFunction(q0, q2, '0');
            TransitionFunction tr3 = new TransitionFunction(q2, q2, '0');
            TransitionFunction tr4 = new TransitionFunction(q2, q1, '1');
            TransitionFunction tr5 = new TransitionFunction(q1, q1, '0');
            TransitionFunction tr6 = new TransitionFunction(q1, q1, '1');
            transitions.Add(tr1);
            transitions.Add(tr2);
            transitions.Add(tr3);
            transitions.Add(tr4);
            transitions.Add(tr5);
            transitions.Add(tr6);

            FiniteStateAutomaton DFA = new FiniteStateAutomaton
            (
                comment: "DFA",
                alphabet: alphabet,
                states: states,
                initState: q0,
                finalStates: new List<string>() { q1 },
                transitions: transitions
            );


            string jsonObject = JsonConvert.SerializeObject(DFA, Formatting.Indented);


            alphabet = new List<char>() { 'a', 'b' };
            states = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            string initState = "0";
            List<string> finStates = new List<string>() { "10" };
            transitions = new List<TransitionFunction>()
            {
                new TransitionFunction("0","1",Constants.Epsilon),
                new TransitionFunction("0","10",Constants.Epsilon),
                new TransitionFunction("1","2",Constants.Epsilon),
                new TransitionFunction("1","4",Constants.Epsilon),
                new TransitionFunction("2","3",Constants.Epsilon),
                new TransitionFunction("4","5",'a'),
                new TransitionFunction("3","6",Constants.Epsilon),
                new TransitionFunction("6","7",Constants.Epsilon),
                new TransitionFunction("6","9",Constants.Epsilon),
                new TransitionFunction("7","8",'b'),
                new TransitionFunction("8","7",Constants.Epsilon),
                new TransitionFunction("8","9",Constants.Epsilon),
                new TransitionFunction("9","1",Constants.Epsilon),
                new TransitionFunction("9","10",Constants.Epsilon),
            };
            FiniteStateAutomaton NFA = new FiniteStateAutomaton(
                comment: "NFA",
                alphabet: alphabet,
                states: states,
                initState: initState,
                finalStates: finStates,
                transitions: transitions
                );

            jsonObject = JsonConvert.SerializeObject(NFA, Formatting.Indented);


            string regex = "*(|(*(.(a,b)),c))";

            List<Token> tokens = Parser.ParseRegex(regex);


            Console.ReadLine();

        }






    }
}
