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
            //        bool isDfa = false;
            //        bool isFinite = false;
            //        List<string> accWords = new List<string>();
            //        List<string> rejWords = new List<string>();
            //        bool readsTransitions = false;
            //        bool readsWords = false;
            //        string line;
            //            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            //            {
            //                while ((line = reader.ReadLine()) != null)
            //                {
            //                    if (String.IsNullOrWhiteSpace(line))
            //                    {
            //                        continue;
            //                    }

            //                    if (readsWords)
            //                    {
            //                        if (line == "end.")
            //                        {
            //                            readsTransitions = false;
            //                            continue;
            //                        }
            //                        string[] strings = line.Split(",");
            //                        if (strings[1] == "y")
            //                            accWords.Add(strings[0]);
            //                        else
            //                            rejWords.Add(strings[0]);

            //                        continue;
            //                    }

            //                    if (readsTransitions)
            //                    {
            //                        if (line == "end.")
            //                        {
            //                            readsTransitions = false;
            //                            continue;
            //                        }

            //                        string[] strings = line.Split(",");
            //fsm.Transitions.Add(new Transition(strings[0], strings[1].Split(" ").LastOrDefault(), strings[1].FirstOrDefault()));
            //                        continue;
            //                    }
            //                    string[] split = line.Split(" ");
            //                    switch (split[0])
            //                    {
            //                        case "#":
            //                            //ignore
            //                            break;
            //                        case "alphabet:":
            //                            fsm.Alphabet = split[1].ToCharArray().ToList();
            //                            break;
            //                        case "states:":
            //                            fsm.States = split[1].Split(",").ToList();
            //fsm.InitialState = fsm.States.FirstOrDefault();
            //                            break;
            //                        case "final:":
            //                            fsm.FinalStates = split[1].Split(",").ToList();
            //                            break;
            //                        case "transitions:":
            //                            readsTransitions = true;
            //                            break;
            //                        case "dfa:":
            //                            isDfa = split[1] == "y";
            //                            break;
            //                        case "finite:":
            //                            isFinite = split[1] == "y";
            //                            break;
            //                        case "words:":
            //                            readsWords = true;
            //                            break;
            //                        default:
            //                            throw new Exception();
            //                    }
            //                }
            //            }
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
