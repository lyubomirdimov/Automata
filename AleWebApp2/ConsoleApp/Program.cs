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

            DFA automata = new DFA
            (
                comment: "This is comments",
                states: states,
                alphabet: alphabet,
                initState: q0,
                finalStates: new List<string>() { q1 },
                transitions: transitions
            );


            string jsonObject = JsonConvert.SerializeObject(automata, Formatting.Indented);

            Console.WriteLine(jsonObject);

            string regex = "*(|(*(.(a,b)),c))";

            List<Token> tokens = Parser.ParseRegex(regex);


            Console.ReadLine();

        }






    }
}
