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
            List<Symbol> alphabet = new List<Symbol>();
            Symbol sym1 = new Symbol('a');
            Symbol sym2 = new Symbol('b');
            alphabet.Add(sym1);
            alphabet.Add(sym2);

            List<State> states = new List<State>();
            State s1 = new State("S1");
            State s2 = new State("S2");
            State s3 = new State("S3");
            states.Add(s1);
            states.Add(s2);
            states.Add(s3);

            List<State> finalStates = new List<State>();
            finalStates.Add(s3);

            List<Transition> transitions = new List<Transition>();
            Transition tr1 = new Transition(s1,s2,sym1);
            Transition tr2 = new Transition(s1, s2, sym2);
            Transition tr3 = new Transition(s2, s3, sym1);
            transitions.Add(tr1);
            transitions.Add(tr2);
            transitions.Add(tr3);

            Automaton automata = new Automaton()
            {
                Comment = "This is comments",
                Alphabet = alphabet,
                States = states,
                FinalStates = finalStates,
                Transitions = transitions
            };


            string jsonObject = JsonConvert.SerializeObject(automata,Formatting.Indented);

            Console.WriteLine(jsonObject);

            Console.ReadLine();

        }

      
     

  

    }
}
