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
            string input = "aaaaaaab";
            StringBuilder steps = new StringBuilder();
            bool acceptsInput = NFA.Accepts(NFA.InitialState,input, steps);
            Assert.IsTrue(acceptsInput);
        }

        [TestMethod]
        public void ConstructNFA()
        {

        }
    }
}
