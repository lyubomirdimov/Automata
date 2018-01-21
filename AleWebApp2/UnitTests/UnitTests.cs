using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Automata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTests
{
    [TestClass]
    public class UnitTests
    {

        public List<PDATestVectors> PDAs { get; set; }
        public List<RegexToFSM> RegexToFSM { get; set; }
        public List<RegexToFSM> RegexToDFA { get; set; }
        public List<PdaFileObject> PdaFileObjects { get; set; } = new List<PdaFileObject>();
        public List<FSMFileObject> FSMFileObjects { get; set; } = new List<FSMFileObject>();
        private static string FsmFolderPath = @"../../../../Automata/FSMs/";
        private static string PDAFolderPath = @"../../../../Automata/PDAs";
        private static string REtoFSMFolderPath = @"../../../../Automata/REtoFSM/";
        private static string REtoDFAFolderPath = @"../../../../Automata/REtoDFA/";

        public UnitTests()
        {
            Init();
        }

        private void Init()
        {

            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/Breaking-bad-Episode-Ale2/master/PDAs.json");
                PDAs = JsonConvert.DeserializeObject<List<PDATestVectors>>(json);

                json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/Breaking-bad-Episode-Ale2/master/RegexToFSM.json");
                RegexToFSM = JsonConvert.DeserializeObject<List<RegexToFSM>>(json);

                json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/Breaking-bad-Episode-Ale2/master/RegexToDFA.json");
                RegexToDFA = JsonConvert.DeserializeObject<List<RegexToFSM>>(json);
            }
        }


        [TestMethod]
        public void IsDFATest()
        {

            foreach (var automaton in RegexToDFA)
            {
                Assert.IsTrue(automaton.FSM.IsDFA());
            }
            foreach (var fsm in RegexToFSM.Where(x => x.IsDFA))
            {
                Assert.IsTrue(fsm.FSM.IsDFA());
            }

            // Manual FSM
            DirectoryInfo fsmDirectory = new DirectoryInfo(FsmFolderPath);

            FSMFileObjects = new List<FSMFileObject>();
            foreach (FileInfo file in fsmDirectory.GetFiles("*.txt"))
            {
                FSMFileObjects.Add(FileParser.FileToFSM(reader: file.OpenText()));
            }

            foreach (var fsmFileObject in FSMFileObjects)
            {
                Assert.IsTrue(fsmFileObject.IsDfa == fsmFileObject.FSM.IsDFA());
            }

            // RE to FSM
            fsmDirectory = new DirectoryInfo(REtoFSMFolderPath);

            FSMFileObjects = new List<FSMFileObject>();
            foreach (FileInfo file in fsmDirectory.GetFiles("*.txt"))
            {
                FSMFileObjects.Add(FileParser.FileToFSM(reader: file.OpenText()));
            }

            for (var i = 0; i < FSMFileObjects.Count; i++)
            {
                var fsmFileObject = FSMFileObjects[i];
                Assert.IsTrue(fsmFileObject.IsDfa == fsmFileObject.FSM.IsDFA());
            }


            // RE to DFA
            fsmDirectory = new DirectoryInfo(REtoDFAFolderPath);

            FSMFileObjects = new List<FSMFileObject>();
            foreach (FileInfo file in fsmDirectory.GetFiles("*.txt"))
            {
                FSMFileObjects.Add(FileParser.FileToFSM(reader: file.OpenText()));
            }

            for (var i = 0; i < FSMFileObjects.Count; i++)
            {
                var fsmFileObject = FSMFileObjects[i];
                Assert.IsTrue(fsmFileObject.IsDfa == fsmFileObject.FSM.IsDFA());
            }
        }

        [TestMethod]
        public void IsNFATest()
        {
            foreach (var testVector in RegexToFSM.Where(x => x.IsDFA == false))
            {
                Assert.IsFalse(testVector.FSM.IsDFA());
            }

        }

        [TestMethod]
        public void TestFSMAccepts()
        {
            foreach (var fsmTestVector in RegexToFSM)
            {
                foreach (string word in fsmTestVector.Words)
                {
                    Assert.IsTrue(fsmTestVector.FSM.Accepts(word));
                }
            }
            foreach (var automaton in RegexToDFA)
            {
                foreach (string automatonWord in automaton.Words)
                {
                    Assert.IsTrue(automaton.FSM.Accepts(automatonWord));
                }
            }


            // Manual FSM
            DirectoryInfo fsmDirectory = new DirectoryInfo(FsmFolderPath);

            FSMFileObjects = new List<FSMFileObject>();
            foreach (FileInfo file in fsmDirectory.GetFiles("*.txt"))
            {
                FSMFileObjects.Add(FileParser.FileToFSM(reader: file.OpenText()));
            }

            foreach (var fsmFileObject in FSMFileObjects)
            {
                foreach (var acceptedWord in fsmFileObject.AcceptedWords)
                {
                    fsmFileObject.FSM.Accepts(acceptedWord);
                }
            }

            // RE to FSM
            fsmDirectory = new DirectoryInfo(REtoFSMFolderPath);

            FSMFileObjects = new List<FSMFileObject>();
            foreach (FileInfo file in fsmDirectory.GetFiles("*.txt"))
            {
                FSMFileObjects.Add(FileParser.FileToFSM(reader: file.OpenText()));
            }
            foreach (var fsmFileObject in FSMFileObjects)
            {
                foreach (var acceptedWord in fsmFileObject.AcceptedWords)
                {
                    fsmFileObject.FSM.Accepts(acceptedWord);
                }
            }


            // RE to DFA
            fsmDirectory = new DirectoryInfo(REtoDFAFolderPath);

            FSMFileObjects = new List<FSMFileObject>();
            foreach (FileInfo file in fsmDirectory.GetFiles("*.txt"))
            {
                FSMFileObjects.Add(FileParser.FileToFSM(reader: file.OpenText()));
            }

            foreach (var fsmFileObject in FSMFileObjects)
            {
                foreach (var acceptedWord in fsmFileObject.AcceptedWords)
                {
                    fsmFileObject.FSM.Accepts(acceptedWord);
                }
            }
        }


        [TestMethod]
        public void TestRegexToNfa()
        {
            // NOTE: THIS test could fail, because of a static variable _Counter. 
            foreach (var regexToFsm in RegexToFSM)
            {
                FiniteStateAutomaton nfa = AutomataConstructor.RegexToNfa(regexToFsm.regex);

                Assert.IsTrue(nfa.Alphabet.SequenceEqual(regexToFsm.FSM.Alphabet));
                Assert.IsTrue(nfa.States.Count.Equals(regexToFsm.FSM.States.Count));
                Assert.IsTrue(nfa.FinalStates.Count.Equals(regexToFsm.FSM.FinalStates.Count));
            }

        }

        [TestMethod]
        public void TestRegexToDfa()
        {
            foreach (var regexToFsm in RegexToDFA)
            {
                FiniteStateAutomaton nfa = AutomataConstructor.RegexToNfa(regexToFsm.regex);
                FiniteStateAutomaton dfa = nfa.ToDfa();
                Assert.IsTrue(dfa.Alphabet.SequenceEqual(regexToFsm.FSM.Alphabet));
                Assert.IsTrue(dfa.States.SequenceEqual(regexToFsm.FSM.States));
                Assert.IsTrue(dfa.InitialState.Equals(regexToFsm.FSM.InitialState));
                Assert.IsTrue(dfa.FinalStates.SequenceEqual(regexToFsm.FSM.FinalStates));
            }
        }

        [TestMethod]
        public void TestManualFsms()
        {
            DirectoryInfo fsmDirectory = new DirectoryInfo(FsmFolderPath);

            foreach (FileInfo file in fsmDirectory.GetFiles("*.txt"))
            {
                FSMFileObjects.Add(FileParser.FileToFSM(reader: file.OpenText()));
            }

            foreach (var fsmFileObject in FSMFileObjects)
            {
                Assert.IsTrue(fsmFileObject.IsFinite != fsmFileObject.FSM.IsInfinite());
                Assert.IsTrue(fsmFileObject.IsDfa == fsmFileObject.FSM.IsDFA());
                foreach (string fsmAcceptedWord in fsmFileObject.AcceptedWords)
                {
                    Assert.IsTrue(fsmFileObject.FSM.Accepts(fsmAcceptedWord));
                }
                foreach (string rejected in fsmFileObject.RejectedWords)
                {
                    Assert.IsFalse(fsmFileObject.FSM.Accepts(rejected));
                }
            }
        }

       

        [TestMethod]
        public void TestPDAAccepts()
        {
            DirectoryInfo pdaDirectory = new DirectoryInfo(PDAFolderPath);

            foreach (FileInfo file in pdaDirectory.GetFiles("*.txt"))
            {
                PdaFileObjects.Add(FileParser.FileToPDA(reader: file.OpenText()));
            }

            bool accepts;
            foreach (var pdaFileObject in PdaFileObjects)
            {
                foreach (string accepted in pdaFileObject.AcceptedWords)
                {
                    accepts = pdaFileObject.Pda.Accepts(accepted);
                    Assert.IsTrue(accepts);
                }
                foreach (string rejected in pdaFileObject.RejectedWords)
                {
                    accepts = pdaFileObject.Pda.Accepts(rejected);
                    Assert.IsFalse(accepts);
                }
            }
        }
    }

    public class FSMTestVector
    {
        public FiniteStateAutomaton FSM { get; set; }
        public bool IsDFA { get; set; }
        public bool IsFinite { get; set; }
        public List<string> Words { get; set; }
    }

    public class RegexToFSM : FSMTestVector
    {
        public string regex { get; set; }
    }

    public class PDATestVectors
    {
        public PDA PDA { get; set; }
        public List<string> SomeAcceptedWords { get; set; }
    }
}
