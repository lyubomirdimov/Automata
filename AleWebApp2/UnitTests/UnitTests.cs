﻿using System;
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

        public List<PDATestVectors> PDAs { get; set; }
        public List<RegexToFSM> RegexToFSM { get; set; }
        public List<RegexToFSM> RegexToDFA { get; set; }

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

        private void TestVectorsGen()
        {

            //AutomataConstructor c = new AutomataConstructor();
            //string regex = "a";
            //var breaking = c.RegexToNfa(regex);
            //var stateAutomaton = breaking.ToDfa();
            //var breaks = stateAutomaton.IsDFA();
            //breaks = breaking.IsDFA();

            //List<RegexToFSM> tsts = new List<RegexToFSM>();
            //for (int i = 0; i < 300; i++)
            //{
            //    var tree = TreeConstructor.ConstructRandomTree();
            //    var fsm = c.RegexToNfa(tree.ToPrefixNotation());
            //    fsm = fsm.ToDfa();
            //    RegexToFSM rgfx = new RegexToFSM();
            //    rgfx.FSM = fsm;
            //    rgfx.regex = tree.ToPrefixNotation();
            //    rgfx.IsDFA = fsm.IsDFA();
            //    rgfx.IsFinite = fsm.IsInfinite() == false;
            //    rgfx.Words = fsm.AcceptedWords();
            //    tsts.Add(rgfx);
            //}

            //var serializeObject = JsonConvert.SerializeObject(tsts,Formatting.Indented);
        }
        [TestMethod]
        public void IsDFATest()
        {

            foreach (var fsm in RegexToFSM.Where(x => x.IsDFA))
            {
                Assert.IsTrue(fsm.FSM.IsDFA());
            }
            foreach (var automaton in RegexToDFA)
            {
                Assert.IsTrue(automaton.FSM.IsDFA());
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
                Assert.IsTrue(fsmTestVector.Words.SequenceEqual(fsmTestVector.FSM.AcceptedWords()));
            }
        }


        [TestMethod]
        public void TestRegexToNfa()
        {
            foreach (var regexToFsm in RegexToFSM)
            {
                FiniteStateAutomaton nfa = AutomataConstructor.RegexToNfa(regexToFsm.regex);

                Assert.IsTrue(nfa.Alphabet.SequenceEqual(regexToFsm.FSM.Alphabet));
                Assert.IsTrue(nfa.States.SequenceEqual(regexToFsm.FSM.States));
                Assert.IsTrue(nfa.InitialState.Equals(regexToFsm.FSM.InitialState));
                Assert.IsTrue(nfa.FinalStates.SequenceEqual(regexToFsm.FSM.FinalStates));
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

        [TestMethod]
        public void TestJorisTestVector()
        {
            FiniteStateAutomaton nfa = new FiniteStateAutomaton();
            nfa.Alphabet = new List<char>() { 'a', 'b', 's' };
            nfa.States = new List<string>() { "A", "D", "B", "C" };
            nfa.InitialState = "A";
            nfa.FinalStates = new List<string>() { "A" };
            nfa.Transitions = new List<Transition>()
            {
                new Transition("A","B",'a'),
                new Transition("A","C",'a'),
                new Transition("B","A",'b'),
                new Transition("C","B",'b'),
                new Transition("C","D",'b'),
                new Transition("D","B",'b'),
            };

            Assert.IsFalse(nfa.IsDFA());
            Assert.IsTrue(nfa.IsInfinite());

            List<string> words = new List<string>() {"ababb", "ab", "abbbabababb", "" };
            List<string> notAccepted = new List<string>() { "abbbb", "aa", "a" };

            foreach (var word in words)
            {
                Assert.IsTrue(nfa.Accepts(word));
            }
            foreach (string s in notAccepted)
            {
                Assert.IsTrue(nfa.Accepts(s) == false);
            }

            string s1 = nfa.ToString();
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
