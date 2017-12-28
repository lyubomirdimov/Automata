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

            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps2/master/nfa.json");
                NFA = JsonConvert.DeserializeObject<FiniteStateAutomaton>(json);
            }

            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps2/master/dfa.json");
                DFA = JsonConvert.DeserializeObject<FiniteStateAutomaton>(json);
            }

        }
        [TestMethod]
        public void IsDFATest()
        {
            Assert.IsTrue(DFA.IsDFA());
            
            Assert.IsFalse(NFA.IsDFA());
        }

        [TestMethod]
        public void TestAccepts()
        {
            List<char> alphabet = new List<char>() { 'a', 'b','c' };
            List<string> states = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7" };
            string initState = "0";
            List<string> finStates = new List<string>() { "7" };
            List<TransitionFunction> transitions = new List<TransitionFunction>()
            {
                new TransitionFunction("0", "1", Constants.Epsilon),
                new TransitionFunction("0", "2", Constants.Epsilon),
                new TransitionFunction("1", "1", 'a'),
                new TransitionFunction("1", "3", Constants.Epsilon),
                new TransitionFunction("2", "2", Constants.Epsilon),
                new TransitionFunction("2", "4", 'a'),
                new TransitionFunction("3", "1", Constants.Epsilon),
                new TransitionFunction("3", "3", 'b'),
                new TransitionFunction("3", "5", Constants.Epsilon),
                new TransitionFunction("4", "4", 'a'),
                new TransitionFunction("4", "6", Constants.Epsilon),
                new TransitionFunction("5", "3", 'a'),
                new TransitionFunction("5", "7", 'c'),
                new TransitionFunction("6", "6", 'b'),
                new TransitionFunction("6", "7", Constants.Epsilon),
                new TransitionFunction("7", "7", 'c')
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

    }
}
