using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Automata
{
    public static class FileParser
    {
        public static FiniteStateAutomaton FileToFSM(IFormFile file)
        {
            FiniteStateAutomaton fsm = new FiniteStateAutomaton();
            bool isDfa = false;
            bool isFinite = false;
            List<string> accWords = new List<string>();
            List<string> rejWords = new List<string>();
            bool readsTransitions = false;
            bool readsWords = false;
            string line;
            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    if (readsWords)
                    {
                        if (line == "end.")
                        {
                            readsWords = false;
                            continue;
                        }
                        string[] strings = line.Split(',');
                        if (strings[1] == "y")
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

                        string[] strings = line.Split(',');
                        string start = strings[0];
                        string end = strings[1].Split(' ').LastOrDefault();
                        char symbol = strings[1].FirstOrDefault();
                        if (symbol == '_')
                        {
                            symbol = Constants.Epsilon;
                        }
                        fsm.Transitions.Add(new Transition(start, end, symbol));
                        continue;
                    }
                    string[] split = line.Split(' ');
                    switch (split[0])
                    {
                        case "#":
                            //ignore
                            break;
                        case "alphabet:":
                            fsm.Alphabet = split[1].ToCharArray().ToList();
                            break;
                        case "states:":
                            fsm.States = split[1].Split(',').ToList();
                            fsm.InitialState = fsm.States.FirstOrDefault();
                            break;
                        case "final:":
                            fsm.FinalStates = split[1].Split(',').ToList();
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
                            break;
                    }
                }
            }
            return fsm;
        }
        //public static FiniteStateAutomaton FSMToFile()
        //{

        //}
        //public static FiniteStateAutomaton FileToPDA()
        //{

        //}
        //public static FiniteStateAutomaton PDAToFile()
        //{

        //}

    }
}
