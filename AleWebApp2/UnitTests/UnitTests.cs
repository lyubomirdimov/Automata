using System;
using System.Collections.Generic;
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

        public List<FSMTestVector> FiniteStateAutomatons { get; set; }
        public List<PDATestVectors> PDAs { get; set; }
        public List<RegexToFSM> RegexToNFA { get; set; }
        public List<RegexToFSM> RegexToDFA { get; set; }

        public UnitTests()
        {
            Init();
        }

        private void Init()
        {
           
            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/Breaking-bad-Episode-Ale2/master/FiniteStateAutomatons.json");
                FiniteStateAutomatons = JsonConvert.DeserializeObject<List<FSMTestVector>>(json);

                json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/Breaking-bad-Episode-Ale2/master/PDAs.json");
                PDAs = JsonConvert.DeserializeObject<List<PDATestVectors>>(json);

                json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/Breaking-bad-Episode-Ale2/master/RegexToNFA.json");
                RegexToNFA = JsonConvert.DeserializeObject<List<RegexToFSM>>(json);

                json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/Breaking-bad-Episode-Ale2/master/RegexToDFA.json");
                RegexToDFA = JsonConvert.DeserializeObject<List<RegexToFSM>>(json);

            }

        }
        [TestMethod]
        public void IsDFATest()
        {
            //AutomataConstructor c = new AutomataConstructor();
            //List<RegexToFSM> tsts = new List<RegexToFSM>();
            //for (int i = 0; i < 100; i++)
            //{
            //    var tree = TreeConstructor.ConstructRandomTree();
            //    var automaton = c.RegexToNfa(tree.ToPrefixNotation());
            //    var dfa = automaton.ToDfa();
            //    RegexToFSM rgfx = new RegexToFSM();
            //    rgfx.FSM = dfa;
            //    rgfx.regex = tree.ToPrefixNotation();
            //    tsts.Add(rgfx);
            //}

            //var serializeObject = JsonConvert.SerializeObject(tsts,Formatting.Indented);

            foreach (var fsm in FiniteStateAutomatons.Where(x=>x.IsDFA))
            {
                Assert.IsTrue(fsm.FSM.IsDFA());
            }
            foreach (var automaton in RegexToDFA)
            {
                if (automaton.regex == "_")
                {
                    Assert.IsTrue(automaton.FSM.IsDFA());
                }
                Assert.IsTrue(automaton.FSM.IsDFA());
            }

        }

        [TestMethod]
        public void IsNFATest()
        {
            foreach (var testVector in FiniteStateAutomatons.Where(x=>x.IsDFA == false))
            {
                Assert.IsFalse(testVector.FSM.IsDFA());
            }

            foreach (var nfa in RegexToNFA)
            {
                Assert.IsFalse(nfa.FSM.IsDFA());
            }
        }

        [TestMethod]
        public void TestFSMAccepts()
        {
            foreach (var fsmTestVector in FiniteStateAutomatons)
            {
                Assert.IsTrue(fsmTestVector.Words == fsmTestVector.FSM.AcceptedWords());
            }
        }


        [TestMethod]
        public void TestRegexToNfa()
        {
            AutomataConstructor constructor = new AutomataConstructor();
            foreach (var regexToFsm in RegexToNFA)
            {
                FiniteStateAutomaton nfa = constructor.RegexToNfa(regexToFsm.regex);

                Assert.IsTrue(nfa.Alphabet.Equals(regexToFsm.FSM.Alphabet));
                Assert.IsTrue(nfa.States.Equals(regexToFsm.FSM.States));
                Assert.IsTrue(nfa.InitialState.Equals(regexToFsm.FSM.InitialState));
                Assert.IsTrue(nfa.FinalStates.Equals(regexToFsm.FSM.FinalStates));
                Assert.IsTrue(nfa.Transitions.Equals(regexToFsm.FSM.Transitions));
            }
        }

        [TestMethod]
        public void TestRegexToDfa()
        {

        }

     
        [TestMethod]
        public void TestIsFinite()
        {
            

        }

        [TestMethod]
        public void TestAcceptedWords()
        {
           
        }

        [TestMethod]
        public void TestPDAAccepts()
        {
           

        }

    }

    public class FSMTestVector
    {
        public FiniteStateAutomaton FSM { get; set; }
        public bool IsDFA { get; set; }
        public bool IsFinite { get; set; }
        public List<string> Words { get; set; }
    }

    public class RegexToFSM
    {
        public string regex { get; set; }
        public FiniteStateAutomaton FSM { get; set; }
    }

    public class PDATestVectors
    {
        public PDA PDA { get; set; }
        public List<string> SomeAcceptedWords { get; set; }
    }
}
