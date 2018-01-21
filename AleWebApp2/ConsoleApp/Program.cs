using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Automata;
using Newtonsoft.Json;
using UnitTests;

namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            FSMFileObject fsm = ReadFile();
            Console.WriteLine(fsm.FSM.IsInfinite() != fsm.IsFinite ? $"passed IsFinite" : $"failed IsFinite");
            Console.WriteLine(fsm.FSM.IsDFA() == fsm.IsDfa ? $"passed IsDFA" : $"failed IsDFA");
            foreach (string fsmAcceptedWord in fsm.AcceptedWords)
            {
                Console.WriteLine(fsm.FSM.Accepts(fsmAcceptedWord) ? "passed" : "failed");
            }
            foreach (string rejected in fsm.RejectedWords)
            {
                Console.WriteLine(fsm.FSM.Accepts(rejected) == false ? "passed" : "failed");
            }

            var words = fsm.FSM.AcceptedWords();
            Console.ReadLine();
            //TestVectorsGen();
            // RE to DFA

        }

        public static FSMFileObject ReadFile()
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            StreamReader stream = new StreamReader(@"../Automata/FSMs/fsm2.txt");
            FSMFileObject fsm = FileParser.FileToFSM(reader: stream);
            stream.Close();
            return fsm;

        }

        public static void WriteToFile()
        {


            string regex = "|(_,.(*(a),b))";

            FiniteStateAutomaton nfa = AutomataConstructor.RegexToNfa(regex);
            FileParser.FSMToFile(@"../Automata/FSMs/fsm2.txt", nfa);
        }
        public static void TestVectorsGen()
        {
            //List<RegexToFSM> tsts = new List<RegexToFSM>();

            //    for (int i = 0; i < 100; i++)
            //    {
            //        var tree = TreeConstructor.ConstructRandomTree();
            //        var fsm = AutomataConstructor.RegexToNfa(tree.ToPrefixNotation());
            //        fsm = fsm.ToDfa();
            //        RegexToFSM rgfx = new RegexToFSM();
            //        rgfx.FSM = fsm;
            //        rgfx.regex = tree.ToPrefixNotation();
            //        rgfx.IsDFA = fsm.IsDFA();
            //        rgfx.IsFinite = fsm.IsInfinite() == false;
            //        rgfx.Words = fsm.AcceptedWords();
            //        tsts.Add(rgfx);
            //    }


            //var serializeObject = JsonConvert.SerializeObject(tsts, Formatting.Indented);

            //List<PDATestVectors> PDAs;
            //List<RegexToFSM> RegexToFSM;
            //List<RegexToFSM> RegexToDFA;
            //using (WebClient wc = new WebClient())
            //{


            //    string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/Breaking-bad-Episode-Ale2/master/RegexToFSM.json");
            //    RegexToFSM = JsonConvert.DeserializeObject<List<RegexToFSM>>(json);

            //    json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/Breaking-bad-Episode-Ale2/master/RegexToDFA.json");
            //    RegexToDFA = JsonConvert.DeserializeObject<List<RegexToFSM>>(json);
            //}

            //for (var i = 0; i < RegexToFSM.Count; i++)
            //{
            //    RegexToFSM regexToFsm = RegexToFSM[i];
            //    regexToFsm.FSM.Comment = regexToFsm.regex;
            //    FileParser.FSMToFile($"../Automata/REtoFSM/retofsm{i}.txt", regexToFsm.FSM);
            //}

            //for (var i = 0; i < RegexToDFA.Count; i++)
            //{
            //    RegexToFSM rgxToDfa = RegexToDFA[i];
            //    rgxToDfa.FSM.Comment = rgxToDfa.regex;
            //    FileParser.FSMToFile($"../Automata/REtoDFA/retodfa{i}.txt", rgxToDfa.FSM);
            //}

        }
    }
}
