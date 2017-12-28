using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


            List<Transition> transitions = new List<Transition>();
            Transition tr1 = new Transition(q0, q0, '1');
            Transition tr2 = new Transition(q0, q2, '0');
            Transition tr3 = new Transition(q2, q2, '0');
            Transition tr4 = new Transition(q2, q1, '1');
            Transition tr5 = new Transition(q1, q1, '0');
            Transition tr6 = new Transition(q1, q1, '1');
            transitions.Add(tr1);
            transitions.Add(tr2);
            transitions.Add(tr3);
            transitions.Add(tr4);
            transitions.Add(tr5);
            transitions.Add(tr6);

            FiniteStateAutomaton DFA = new FiniteStateAutomaton
            (
                
                alphabet: alphabet,
                states: states,
                initState: q0,
                finalStates: new List<string>() { q1 },
                transitions: transitions,
                comment: "DFA"
            );


            string jsonObject = JsonConvert.SerializeObject(DFA, Formatting.Indented);


            alphabet = new List<char>() { 'a', 'b' };
            states = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7" };
            string initState = "0";
            List<string> finStates = new List<string>() { "7" };
            transitions = new List<Transition>()
            {
                new Transition("0", "1", Constants.Epsilon),
                new Transition("0", "2", Constants.Epsilon),
                new Transition("1", "1", 'a'),
                new Transition("1", "3", Constants.Epsilon),
                new Transition("2", "2", Constants.Epsilon),
                new Transition("2", "4", 'a'),
                new Transition("3", "1", Constants.Epsilon),
                new Transition("3", "3", 'b'),
                new Transition("3", "5", Constants.Epsilon),
                new Transition("4", "4", 'a'),
                new Transition("4", "6", Constants.Epsilon),
                new Transition("5", "3", 'a'),
                new Transition("5", "7", Constants.Epsilon),
                new Transition("6", "6", 'b'),
                new Transition("6", "7", Constants.Epsilon),
            };
            FiniteStateAutomaton NFA = new FiniteStateAutomaton(
                
                alphabet: alphabet,
                states: states,
                initState: initState,
                finalStates: finStates,
                transitions: transitions,
                comment: "NFA"
            );

            NFA.Accepts("aaab");

            jsonObject = JsonConvert.SerializeObject(NFA, Formatting.Indented);


            string regex = "*(|(*(.(a,b)),c))";

            List<Token> tokens = Parser.ParseRegex(regex);


            Console.ReadLine();

        }

       






    }
}
