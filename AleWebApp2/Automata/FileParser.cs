using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Automata
{
    public static class FileParser
    {
        public static FSMFileObject FileToFSM(IFormFile file)
        {
            StreamReader stream = new StreamReader(file.OpenReadStream());
            FSMFileObject fsm = FileToFSM(stream);
            stream.Close();
            return fsm;
        }
        public static FSMFileObject FileToFSM(StreamReader reader)
        {
            FiniteStateAutomaton fsm = new FiniteStateAutomaton();

            bool isDfa = false;
            bool isFinite = false;
            List<string> accWords = new List<string>();
            List<string> rejWords = new List<string>();
            bool readsTransitions = false;
            bool readsWords = false;
            string line;
            string start;
            string end;
            string[] strings;
            char symbol;
            string[] split;
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

                    strings = line.Split(',');
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

                    strings = line.Split(',');
                    start = strings[0];
                    end = strings[1].Split(' ').LastOrDefault();
                    symbol = strings[1].FirstOrDefault() == '_' ? Constants.Epsilon : strings[1].FirstOrDefault();
                    fsm.Transitions.Add(new Transition(start, end, symbol));
                    continue;
                }

                split = line.Split(' ');
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

            FSMFileObject file = new FSMFileObject
            {
                FSM = fsm,
                IsDfa = isDfa,
                IsFinite = isFinite,
                AcceptedWords = accWords,
                RejectedWords = rejWords
            };
            return file;
        }
        //public static FiniteStateAutomaton FSMToFile()
        //{

        //}

        public static PdaFileObject FileToPDA(IFormFile file)
        {
            StreamReader stream = new StreamReader(file.OpenReadStream());
            PdaFileObject fileObject = FileToPDA(stream);
            stream.Close();
            return fileObject;
        }
        public static PdaFileObject FileToPDA(StreamReader reader)
        {
            PDA pda = new PDA();
            bool isDfa = false;
            bool isFinite = false;
            List<string> accWords = new List<string>();
            List<string> rejWords = new List<string>();
            bool readsTransitions = false;
            bool readsWords = false;
            string line;
            string[] strings;
            string start;
            string end;
            char symbol;
            string[] split;
            char stackTop;
            List<char> pushSymbols;
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
                    strings = line.Split(',');
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

                    start = line.Split(',')[0].Trim();
                    end = line.Split('>').LastOrDefault().Trim();
                    symbol = line.Split(',')[1].FirstOrDefault() == '_' ? Constants.Epsilon : line.Split(',')[1].FirstOrDefault();
                    if (line.Contains('['))
                    {
                        strings = line.Substring(line.IndexOf('[') + 1, line.IndexOf(']') - line.IndexOf('[') - 1).Split(',');
                        stackTop = strings[0].FirstOrDefault() == '_' ? Constants.Epsilon : strings[0].FirstOrDefault();
                        if (strings[1].ToCharArray().Length == 1 && strings[1].ToCharArray()[0] == '_')
                        {
                            pushSymbols = new List<char>() { Constants.Epsilon };
                        }
                        else
                        {
                            pushSymbols = strings[1].ToCharArray().ToList();
                        }
                        pda.Transitions.Add(new TransitionFunction(start, symbol, stackTop, pushSymbols, end));
                    }
                    else
                    {
                        pda.Transitions.Add(new TransitionFunction(start, symbol, end));
                    }
                    continue;
                }
                split = line.Split(' ');
                switch (split[0])
                {
                    case "#":
                        //ignore
                        break;
                    case "alphabet:":
                        pda.InputAlphabet = split[1].ToCharArray().ToList();
                        break;
                    case "stack:":
                        pda.StackAlphabet = split[1].ToCharArray().ToList();
                        break;
                    case "states:":
                        pda.States = split[1].Split(',').ToList();
                        pda.InitialState = pda.States.FirstOrDefault();
                        break;
                    case "final:":
                        pda.FinalStates = split[1].Split(',').ToList();
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

            PdaFileObject fileObject = new PdaFileObject
            {
                Pda = pda,
                AcceptedWords = accWords,
                RejectedWords = rejWords,
                IsDfa = isDfa,
                IsFinite = isFinite
            };
            return fileObject;
        }
        //public static FiniteStateAutomaton PDAToFile()
        //{

        //}

    }
    public class FSMFileObject
    {
        public FiniteStateAutomaton FSM { get; set; }
        public bool IsDfa { get; set; }
        public bool IsFinite { get; set; }
        public List<string> AcceptedWords { get; set; }
        public List<string> RejectedWords { get; set; }
    }
    public class PdaFileObject
    {
        public PDA Pda { get; set; }
        public bool IsDfa { get; set; }
        public bool IsFinite { get; set; }
        public List<string> AcceptedWords { get; set; }
        public List<string> RejectedWords { get; set; }
    }
}

