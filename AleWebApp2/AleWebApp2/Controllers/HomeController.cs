using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AleWebApp2.Models;
using Automata;
using Microsoft.AspNetCore.Authorization;
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
            if (input == null) input = string.Empty;
            FiniteStateAutomaton finiteStateAutomaton = JsonConvert.DeserializeObject<FiniteStateAutomaton>(fsm);
            return Json(finiteStateAutomaton.Accepts(input));
        }

        public IActionResult NfaToDfa(string fsm)
        {
            if (fsm == null) return Error();
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

        /// <summary>
        /// Validates weather or not input is a valid Regular Expression in Infix Notenation
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public JsonResult ValidateRegex(string RegularExpression)
        {
            return Json(RegularExpressionValidator.Validate(RegularExpression));
        }

        public ActionResult GenerateRandomRegex()
        {
            Node tree = TreeConstructor.ConstructRandomTree();
            string prefixTree = tree.ToPrefixNotation();
            return Json(prefixTree);
        }
        #endregion

        #region PDA

        public IActionResult PDA()
        {

            
            PDAViewModel model = new PDAViewModel();
            model.PDA = getPDA();
            model.PDAString = JsonConvert.SerializeObject(getPDA());
            return View(model);
        }

        public IActionResult PDAAccepts(string input)
        {
            if (input == null) input = string.Empty;
            PDA pushDownAutomaton = getPDA();
            return Json(pushDownAutomaton.Accepts(input));
        }

        private PDA getPDA()
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
            return pda;
        }
        #endregion

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
