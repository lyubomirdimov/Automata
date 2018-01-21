using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public static void FSMToFile(string path, FiniteStateAutomaton fsm)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"# {fsm.Comment}");
            builder.AppendLine();
            builder.AppendLine($"alphabet: {string.Join("", fsm.Alphabet)}");
            builder.AppendLine($"states: {fsm.InitialState},{String.Join(",", fsm.States.Where(x => x != fsm.InitialState).ToArray())}");
            builder.AppendLine($"final: {String.Join(",", fsm.FinalStates.ToArray())}");

            builder.AppendLine();
            builder.AppendLine("dfa: " + (fsm.IsDFA() ? "y" : "n"));
            builder.AppendLine("finite: " + (fsm.IsInfinite() ? "n" : "y"));

            builder.AppendLine($"transitions: ");
            foreach (var transition in fsm.Transitions)
            {
                builder.AppendLine(transition.ToPrefixString());
            }
            builder.AppendLine("end.");

            List<string> acceptedWords = fsm.AcceptedWords();

            if (acceptedWords.Any())
            {
                builder.AppendLine();
                builder.AppendLine("words: ");
                foreach (string acceptedWord in acceptedWords)
                {
                    builder.AppendLine($"{acceptedWord},y");
                }
                builder.AppendLine("end.");
            }
            File.WriteAllText(path, builder.ToString());

        }

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
        public static void PDAToFile(string path, PDA pda)
        {

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"# {pda.Comment}");
            builder.AppendLine();
            builder.AppendLine($"alphabet: {string.Join("", pda.InputAlphabet)}");
            builder.AppendLine($"stack: {string.Join("", pda.StackAlphabet)}");
            builder.AppendLine($"states: {pda.InitialState},{String.Join(",", pda.States.Where(x => x != pda.InitialState).ToArray())}");
            builder.AppendLine($"final: {String.Join(",", pda.FinalStates.ToArray())}");


            builder.AppendLine();
            builder.AppendLine("dfa: n");
            builder.AppendLine("finite: n");

            builder.AppendLine($"transitions: ");
            foreach (var transition in pda.Transitions)
            {
                builder.AppendLine(transition.ToPrefixString());
            }
            builder.AppendLine("end.");



            File.WriteAllText(path, builder.ToString());

        }

    }
    public class FSMFileObject
    {
        public string Comment { get; set; }
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

