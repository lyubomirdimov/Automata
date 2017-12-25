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

            FiniteStateViewModel model = new FiniteStateViewModel();
            model.IsDFA = automata.IsDFA();
            foreach (string currState in automata.States)
            {

                if (currState == automata.InitialState)
                {
                    // Initial State
                    model.Nodes.Add(new Node() { color = "#538EA6", id = currState, label = currState });
                    continue;
                }
                if (automata.FinalStates.Exists(x => x == currState))
                {
                    // Final State
                    model.Nodes.Add(new Node() { color = "#F3B562", id = currState, label = currState });
                    continue;
                }
                model.Nodes.Add(new Node() { id = currState, label = currState });
            }

            foreach (TransitionFunction transition in automata.Transitions)
            {
                model.Edges.Add(new Edge() { arrows = "to", from = transition.StartState, to = transition.EndState, label = transition.Symbol.ToString() });
            }


            return View(model);
        }

        public FiniteStateAutomaton TestVector()
        {

            List<char> alphabet = new List<char>() { 'a', 'b' };
            List<string> states = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7" };
            string initState = "0";
            List<string> finStates = new List<string>() { "7" };
            List<TransitionFunction> transitions = new List<TransitionFunction>()
            {
                new TransitionFunction("0","1",Constants.Epsilon),
                new TransitionFunction("0","2",Constants.Epsilon),
                new TransitionFunction("1","6",'b'),
                new TransitionFunction("2","3",Constants.Epsilon),
                new TransitionFunction("2","5",Constants.Epsilon),
                new TransitionFunction("3","4",'a'),
                new TransitionFunction("4","3",Constants.Epsilon),
                new TransitionFunction("4","5",Constants.Epsilon),
                new TransitionFunction("5","7",Constants.Epsilon),
                new TransitionFunction("6","7",Constants.Epsilon),
            };
            FiniteStateAutomaton NFA = new FiniteStateAutomaton(
                comment: "NFA",
                alphabet: alphabet,
                states: states,
                initState: initState,
                finalStates: finStates,
                transitions: transitions
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
