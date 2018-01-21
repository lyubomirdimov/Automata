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
            Console.ReadLine();
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
    }
}
