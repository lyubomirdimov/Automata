using System;
using System.Collections.Generic;
using System.IO;
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

            FiniteStateAutomaton fsm = ReadFile();
            bool good = false;
            Console.WriteLine(fsm.IsInfinite() == false);
            Console.WriteLine(fsm.IsDFA());
            
            Console.ReadLine();
        }

        public static FiniteStateAutomaton ReadFile()
        {
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            StreamReader stream = new StreamReader(@"E:\SelfDrive\Repositories\ALE2\tp4\nfaeps.txt");
            FiniteStateAutomaton fsm = FileParser.FileToFSM(reader: stream);
            stream.Close();
            return fsm;

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
