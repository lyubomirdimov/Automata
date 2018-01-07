using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AleWebApp2.Models;
using Automata;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace AleWebApp2.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }
        public IActionResult FiniteStateAutomaton()
        {
            FiniteStateAutomaton automata = TestVector();
            //using (WebClient wc = new WebClient())
            //{
            //    string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps2/master/nfa.json");
            //    automata = JsonConvert.DeserializeObject<FiniteStateAutomaton>(json);
            //}
            string regex = "|(_,.(*(a),b))";

            AutomataConstructor constructor = new AutomataConstructor();
            FiniteStateAutomaton nfa = constructor.RegexToNfa(regex);

            FiniteStateViewModel model = new FiniteStateViewModel();
            model.IsDFA = nfa.IsDFA();
            model.NFA = nfa;


            return View(model);
        }

        public FiniteStateAutomaton TestVector()
        {

            List<char> alphabet = new List<char>() { 'a', 'b' };
            List<string> states = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7" };
            string initState = "0";
            List<string> finStates = new List<string>() { "7" };
            List<Transition> transitions = new List<Transition>()
            {
                new Transition("0","1",Constants.Epsilon),
                new Transition("0","2",Constants.Epsilon),
                new Transition("1","6",'b'),
                new Transition("2","3",Constants.Epsilon),
                new Transition("2","5",Constants.Epsilon),
                new Transition("3","4",'a'),
                new Transition("4","3",Constants.Epsilon),
                new Transition("4","5",Constants.Epsilon),
                new Transition("5","7",Constants.Epsilon),
                new Transition("6","7",Constants.Epsilon),
            };
            FiniteStateAutomaton NFA = new FiniteStateAutomaton(
                alphabet: alphabet,
                states: states,
                initState: initState,
                finalStates: finStates,
                transitions: transitions,
                comment: "NFA"
            );
            return NFA;
        }
        public IActionResult RegexToNFA()
        {

            return View();
        }

        public IActionResult PDA()
        {
            return View();
        }
        public IActionResult Whatever()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
