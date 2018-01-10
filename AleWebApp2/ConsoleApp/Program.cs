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
           
            ReadFile();

        }

        public static void ReadFile()
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(@"E:\OneDrive\Education\Programming\C#\Automated Logic 2\tp4\finite4.txt");
            FiniteStateAutomaton fsm = new FiniteStateAutomaton();
            bool isDfa = false;
            bool isFinite = false;
            List<string> accWords = new List<string>();
            List<string> rejWords = new List<string>();
            bool readsTransitions = false;
            bool readsWords = false;
            while ((line = file.ReadLine()) != null)
            {
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (readsWords)
                {
                    if (line == "end.")
                    {
                        readsTransitions = false;
                        continue;
                    }
                    string[] strings = line.Split(",");
                    if(strings[1] == "y")
                        accWords.Add(strings[0]);
                    else
                        rejWords.Add(strings[0]);

                    continue;
                }

                if (readsTransitions)
                {
                    if (line == "end.")
                    {
                        readsTransitions = false;
                        continue;
                    }
                   
                    string[] strings = line.Split(",");
                    fsm.Transitions.Add(new Transition(strings[0],strings[1].Split(" ").LastOrDefault(),strings[1].FirstOrDefault()));
                    continue;
                }
                string[] split = line.Split(" ");
                switch (split[0])
                {
                    case "#":
                        //ignore
                        break;
                    case "alphabet:":
                        fsm.Alphabet = split[1].ToCharArray().ToList();
                        break;
                    case "states:":
                        fsm.States = split[1].Split(",").ToList();
                        fsm.InitialState = fsm.States.FirstOrDefault();
                        break;
                    case "final:":
                        fsm.FinalStates = split[1].Split(",").ToList();
                        break;
                    case "transitions:":
                        readsTransitions = true;
                        break;
                    case "dfa:":
                        isDfa = split[1] == "y";
                        break;
                    case "finite:":
                        isFinite = split[1] == "y";
                        break;
                    case "words:":
                        readsWords = true;
                        break;
                    default:
                        throw new Exception();
                }
            }
            FSMTestVector testv = new FSMTestVector();
            testv.FSM = fsm;
            testv.IsDFA = isDfa;
            testv.IsFinite = isFinite;
            testv.AcceptedWords = accWords;
            testv.RejectedWords = rejWords;
            
            List<char> fsmAlphabet = fsm.Alphabet;
            List<string> fsmStates = fsm.States;
            string init = fsm.InitialState;
            List<string> finals = fsm.FinalStates;
            List<Transition> trans = fsm.Transitions;
            file.Close();
        }

        public class FSMTestVector
        {
            public FiniteStateAutomaton FSM { get; set; }
            public bool IsDFA { get; set; }
            public bool IsFinite { get; set; }
            public List<string> AcceptedWords { get; set; }
            public List<string> RejectedWords { get; set; }
        }

        public class RegexToFSM : FSMTestVector
        {
            public string regex { get; set; }
        }









    }
}
