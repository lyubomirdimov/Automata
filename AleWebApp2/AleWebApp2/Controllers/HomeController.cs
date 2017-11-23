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
        private AutomataGenerator _generator;
        public HomeController(AutomataGenerator generator)
        {
            _generator = generator;
        }
        public IActionResult DFA()
        {
            DFA automata;
            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString("https://raw.githubusercontent.com/lyubomirdimov/AleProps2/master/Automata.json");
                automata = JsonConvert.DeserializeObject<DFA>(json);
            }

            DFAViewModel model = new DFAViewModel();
            model.IsDFA = automata.IsDFA;
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

            foreach (Transition transition in automata.Transitions)
            {
                model.Edges.Add(new Edge() { arrows = "to", from = transition.StartState, to = transition.EndState, label = transition.Symbol.ToString() });
            }
            
            


            return View(model);
        }

        public IActionResult RegexToNFA()
        {

            return View();
        }

        public IActionResult PDA()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
