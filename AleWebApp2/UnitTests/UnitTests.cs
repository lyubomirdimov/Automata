using System.Collections.Generic;
using System.Linq;
using System.Net;
using Automata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTests
{
    [TestClass]
    public class UnitTests
    {
        public DFA Automaton { get; set; }
        public UnitTests()
        {
            Init();
        }

        private void Init()
        {
            
            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps2/master/Automata.json");
                Automaton = JsonConvert.DeserializeObject<DFA>(json);
            }

        }
        //[TestMethod]
        //public void IsDFATest()
        //{
        //    Automaton.DetermineIsDFA();
        //    Assert.IsTrue(Automaton.IsDFA);

        //    Automaton.Alphabet[0] = '_';
        //    Automaton.DetermineIsDFA();
        //    Assert.IsFalse(Automaton.IsDFA);
        //}

        //[TestMethod]
        //public void DFAInput()
        //{
        //    string input = "1000000001111111";
        //    string steps;
        //    bool acceptsInput = Automaton.AcceptsInput(input, out steps);
        //    Assert.IsTrue(acceptsInput);
        //}

        //[TestMethod]
        //public void ConstructNFA()
        //{
            
        //} 
    }
}
