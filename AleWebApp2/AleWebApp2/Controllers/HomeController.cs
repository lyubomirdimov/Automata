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

        #region FiniteStateAutomaton
        
        public IActionResult FiniteStateAutomaton()
        {
            string regex = "|(_,.(*(a),b))";

            AutomataConstructor constructor = new AutomataConstructor();
            FiniteStateAutomaton nfa = constructor.RegexToNfa(regex);

            FiniteStateAutomatonViewModel model = new FiniteStateAutomatonViewModel(nfa);

            return View(model);
        }

        public IActionResult FSMAccepts(string fsm, string input)
        {
            FiniteStateAutomaton finiteStateAutomaton = JsonConvert.DeserializeObject<FiniteStateAutomaton>(fsm);
            return Json(finiteStateAutomaton.Accepts(input));
        }

        public IActionResult NfaToDfa(string fsm)
        {
            FiniteStateAutomaton finiteStateAutomaton = JsonConvert.DeserializeObject<FiniteStateAutomaton>(fsm);
            FiniteStateAutomatonViewModel model = new FiniteStateAutomatonViewModel(finiteStateAutomaton.ToDfa());
            return View("FiniteStateAutomaton", model);
        }

        public IActionResult RegexToNfa(FiniteStateAutomatonViewModel model)
        {
            if (model.RegularExpression == null) model.RegularExpression = string.Empty;
            FiniteStateAutomatonViewModel newModel = new FiniteStateAutomatonViewModel(model.RegularExpression);
            return View("FiniteStateAutomaton", newModel);
        }

        #endregion

        #region PDA

        public IActionResult PDA()
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
            PDAViewModel model = new PDAViewModel();
            model.PDA = pda;
            return View(model);
        }
        #endregion

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
