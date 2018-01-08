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
        public FiniteStateAutomaton NFA { get; set; }
        public FiniteStateAutomaton DFA { get; set; }
        public UnitTests()
        {
            Init();
        }

        private void Init()
        {

            //using (WebClient wc = new WebClient())
            //{
            //    string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps2/master/nfa.json");
            //    NFA = JsonConvert.DeserializeObject<FiniteStateAutomaton>(json);
            //}

            //using (WebClient wc = new WebClient())
            //{
            //    string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps2/master/dfa.json");
            //    DFA = JsonConvert.DeserializeObject<FiniteStateAutomaton>(json);
            //}

        }
        [TestMethod]
        public void IsDFATest()
        {
            Assert.IsTrue(DFA.IsDFA());

            Assert.IsFalse(NFA.IsDFA());
        }

        [TestMethod]
        public void TestNFAAccepts()
        {
            List<char> alphabet = new List<char>() { 'a', 'b', 'c' };
            List<string> states = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7" };
            string initState = "0";
            List<string> finStates = new List<string>() { "7" };
            List<Transition> transitions = new List<Transition>()
            {
                new Transition("0", "1", Constants.Epsilon),
                new Transition("0", "2", Constants.Epsilon),
                new Transition("1", "1", 'a'),
                new Transition("1", "3", Constants.Epsilon),
                new Transition("2", "2", Constants.Epsilon),
                new Transition("2", "4", 'a'),
                new Transition("3", "1", Constants.Epsilon),
                new Transition("3", "3", 'b'),
                new Transition("3", "5", Constants.Epsilon),
                new Transition("4", "4", 'a'),
                new Transition("4", "6", Constants.Epsilon),
                new Transition("5", "3", 'a'),
                new Transition("5", "7", 'c'),
                new Transition("6", "6", 'b'),
                new Transition("6", "7", Constants.Epsilon),
                new Transition("7", "7", 'c')
            };
            FiniteStateAutomaton NFA = new FiniteStateAutomaton(
                comment: "NFA",
                alphabet: alphabet,
                states: states,
                initState: initState,
                finalStates: finStates,
                transitions: transitions
            );

            bool accepted = NFA.Accepts("abcc");

            Assert.IsTrue(accepted);
        }


        [TestMethod]
        public void TestRegexToNfa()
        {

            string regex = "|(_,.(*(a),b))";

            AutomataConstructor constructor = new AutomataConstructor();
            FiniteStateAutomaton nfa = constructor.RegexToNfa(regex);


        }

        [TestMethod]
        public void TestNfaToDfa()
        {
            FiniteStateAutomaton nfa = new FiniteStateAutomaton
                (
                new List<char> { '0', '1' },
                new List<string> { "1", "2", "3", "4" },
                "1",
                new List<string> { "3", "4" },
                new List<Transition>
                {
                    new Transition("1","2",'0'),
                    new Transition("1","3",Constants.Epsilon),
                    new Transition("2","2",'1'),
                    new Transition("2","4",'1'),
                    new Transition("3","2",Constants.Epsilon),
                    new Transition("3","4",'0'),
                    new Transition("4","3",'0')
                }
                );
            FiniteStateAutomaton dfa = nfa.ToDfa();

        }
        [TestMethod]
        public void TestIsFinite()
        {
            FiniteStateAutomaton fsm = new FiniteStateAutomaton();
            fsm.Alphabet = new List<char>() { '1', '2', '3', '4', '5', '6' };
            fsm.States = new List<string>() { "a", "b", "c", "d" };
            fsm.InitialState = "a";
            fsm.FinalStates = new List<string>() { "c" };
            fsm.Transitions = new List<Transition>()
            {
                new Transition("a","b", '1'),
                new Transition("a","c", '5'),
                new Transition("b","c", '2'),
                new Transition("c","a", '6'),
                new Transition("c","d", '3'),
                new Transition("d","a", '4'),
            };

            Assert.IsTrue(fsm.IsInfinite());

        }

        [TestMethod]
        public void TestAcceptedWords()
        {
            FiniteStateAutomaton fsm = new FiniteStateAutomaton();
            fsm.Alphabet = new List<char>() { '1', '2', '3', '4', '5', '6' };
            fsm.States = new List<string>() { "a", "b", "c", "d" };
            fsm.InitialState = "a";
            fsm.FinalStates = new List<string>() { "c","d" };
            fsm.Transitions = new List<Transition>()
            {
                new Transition("a","b", '1'),
                new Transition("a","c", '2'),
                new Transition("a","d", '3'),
                new Transition("b","c", '2'),
                new Transition("c","d", '3'),
            };

            List<string> acceptedWords = fsm.AcceptedWords();
        }

        [TestMethod]
        public void TestPDAAccepts()
        {
            PDA pda = new PDA
            {
                InputAlphabet = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'y' },
                StackAlphabet = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'y' },
                States = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" },
                InitialState = "A",
                FinalStates = new List<string>() { "D", "E", "F", "G", "I", "K", "L" },
                Transitions = new List<TransitionFunction>()
                {
                    new TransitionFunction("A", 'a', Constants.Epsilon, new List<char>() {'x'}, "B"),
                    new TransitionFunction("A", 'b', "B"),
                    new TransitionFunction("A", 'c', Constants.Epsilon, new List<char>() {'y'}, "B"),
                    new TransitionFunction("B", 'd', "C"),
                    new TransitionFunction("B", 'd', 'x', new List<char>() {Constants.Epsilon}, "D"),
                    new TransitionFunction("B", 'e', "E"),
                    new TransitionFunction("B", Constants.Epsilon, 'x', new List<char>() {Constants.Epsilon}, "F"),
                    new TransitionFunction("B", Constants.Epsilon, "G"),
                    new TransitionFunction("C", Constants.Epsilon, Constants.Epsilon, new List<char>() {'x'}, "H"),
                    new TransitionFunction("E", 'f', 'y', new List<char>() {Constants.Epsilon}, "I"),
                    new TransitionFunction("F", 'g', "I"),
                    new TransitionFunction("G", 'g', 'x', new List<char>() {Constants.Epsilon}, "J"),
                    new TransitionFunction("G", 'e', "K"),
                    new TransitionFunction("G", 'h', 'y', new List<char>() {Constants.Epsilon}, "L")
                }
            };

            Assert.IsTrue(pda.Accepts("ad"));
            Assert.IsTrue(pda.Accepts("be"));
            Assert.IsTrue(pda.Accepts("cef"));
            Assert.IsTrue(pda.Accepts("ag"));
            Assert.IsTrue(pda.Accepts("b"));
            Assert.IsTrue(pda.Accepts("ch"));


        }

    }
}
