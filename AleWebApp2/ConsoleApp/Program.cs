﻿using System;
using System.Collections.Generic;
using System.Linq;
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

            Automata automata = new Automata
            {
                Comments = "This is comments",
                Alphabet = alphabet,
                States = states,
                FinalStates = finalStates,
                Transitions = transitions
            };

            //AutomataV1 automata = new AutomataV1
            //{
            //    Comments = "This is comment",
            //    Alphabet = new List<char>() { 'a','b'},
            //    States = new List<string>() { "S1","S2","S3"},
            //    FinalStates = new List<string>() { "S3"},
            //    Transitions = new List<string>() { "S1,a --> S2", "S1,b --> S2", "S2,a --> S3" }
            //};

            string jsonObject = JsonConvert.SerializeObject(automata,Formatting.Indented);

            Console.WriteLine(jsonObject);

            Console.ReadLine();

        }

        class AutomataV1
        {
            public string Comments { get; set; }
            public List<char> Alphabet { get; set; }
            public List<string> States { get; set; }

            public List<string> FinalStates { get; set; }
            public List<string> Transitions { get; set; }
        }
        class Automata
        {
            public string Comments { get; set; }
            public List<Symbol> Alphabet { get; set; }
            public List<State> States { get; set; }
            public List<State> FinalStates { get; set; }
            public List<Transition> Transitions { get; set; }

        }

        class Symbol
        {
            public char Name { get; set; }

            public Symbol(char name)
            {
                Name = name;
            }

            public override string ToString()
            {
                return Name.ToString();
            }
        }
        class State
        {
            public string Name { get; set; }
            public State(string name)
            {
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }
        class Transition
        {
            public State StartState { get; set; }
            public State EndState { get; set; }
            public Symbol Symbol { get; set; }
            public Transition(State startState, State endState, Symbol symbol)
            {
                StartState = startState;
                EndState = endState;
                Symbol = symbol;
            }

        }

    }
}
