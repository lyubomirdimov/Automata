using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AleWebApp2.Models;
using Automata;
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
        public IActionResult Index()
        {
            List<Symbol> alphabet = new List<Symbol>();
            Symbol sym1 = new Symbol('a');
            Symbol sym2 = new Symbol('b');
            alphabet.Add(sym1);
            alphabet.Add(sym2);

            List<State> states = new List<State>();
            State s1 = new State("S1");
            State s2 = new State("S2");
            State s3 = new State("S3");
            states.Add(s1);
            states.Add(s2);
            states.Add(s3);

            List<State> finalStates = new List<State>();
            finalStates.Add(s3);

            List<Transition> transitions = new List<Transition>();
            Transition tr1 = new Transition(s1, s2, sym1);
            Transition tr2 = new Transition(s1, s2, sym2);
            Transition tr3 = new Transition(s2, s3, sym1);
            transitions.Add(tr1);
            transitions.Add(tr2);
            transitions.Add(tr3);

            Automaton automata = new Automaton()
            {
                Comment = "This is comments",
                Alphabet = alphabet,
                States = states,
                FinalStates = finalStates,
                Transitions = transitions
            };

            AutomataViewModel model = new AutomataViewModel {Automaton = automata};
            foreach (State currState in model.Automaton.States)
            {
                if (model.Automaton.FinalStates.Contains(currState))
                {
                    // Final State
                    model.Nodes.Add(new Node() { color = "#F3B562", id = currState.Name, label = currState.Name });
                    continue;
                }
                model.Nodes.Add(new Node() { id = currState.Name, label = currState.Name });
            }

            foreach (Transition transition in model.Automaton.Transitions)
            {
                model.Edges.Add(new Edge() { arrows = "to", from = transition.StartState.Name, to = transition.EndState.Name, label = transition.Symbol.ToString() });
            }

            return View(model);
        }



        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
