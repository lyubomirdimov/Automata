using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AleWebApp2.Models;
using Automata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

            FiniteStateAutomaton nfa = AutomataConstructor.RegexToNfa(regex);

            FiniteStateAutomatonViewModel model = new FiniteStateAutomatonViewModel(nfa);
            SetFSMSession(model.FSM);
            return View(model);
        }

        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return RedirectToAction("FiniteStateAutomaton");
            FSMFileObject fsmFileObject = FileParser.FileToFSM(file);
            FiniteStateAutomatonViewModel model = new FiniteStateAutomatonViewModel(fsmFileObject.FSM);
            SetFSMSession(model.FSM);
            return View("FiniteStateAutomaton", model);
        }
        public IActionResult FSMAccepts(string input)
        {
            if (input == null) input = string.Empty;

            FiniteStateAutomaton finiteStateAutomaton = GetFSMSession();
            return Json(finiteStateAutomaton.Accepts(input));
        }

        public IActionResult NfaToDfa()
        {
            FiniteStateAutomaton finiteStateAutomaton = GetFSMSession();
            FiniteStateAutomatonViewModel model = new FiniteStateAutomatonViewModel(finiteStateAutomaton.ToDfa());
            SetFSMSession(model.FSM);
            return View("FiniteStateAutomaton", model);
        }

        public IActionResult RegexToNfa(FiniteStateAutomatonViewModel model)
        {
            if (model.RegularExpression == null) model.RegularExpression = Constants.Epsilon.ToString();
            FiniteStateAutomatonViewModel newModel = new FiniteStateAutomatonViewModel(model.RegularExpression);
            SetFSMSession(newModel.FSM);
            return View("FiniteStateAutomaton", newModel);
        }

        private static string _fsmKey = "fsm";
        private void SetFSMSession(FiniteStateAutomaton fsm)
        {
            this.HttpContext.Session.SetString(_fsmKey, JsonConvert.SerializeObject(fsm));
        }

        private FiniteStateAutomaton GetFSMSession()
        {
            string fsm = this.HttpContext.Session.GetString(_fsmKey);
            return JsonConvert.DeserializeObject<FiniteStateAutomaton>(fsm);

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
            model.PDA = DefaultPDA();

            SetPDASession(model.PDA);
            return View(model);
        }
        public IActionResult UploadPDA(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return RedirectToAction("PDA");

            PdaFileObject pdaFileObject = FileParser.FileToPDA(file);
            PDAViewModel model = new PDAViewModel(pdaFileObject.Pda);
            SetPDASession(model.PDA);
            return View("PDA", model);
        }
        public IActionResult PDAAccepts(string input)
        {
            if (input == null) input = string.Empty;
            PDA pushDownAutomaton = GetPDASession();
            return Json(pushDownAutomaton.Accepts(input));
        }

        private PDA DefaultPDA()
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

        private static string _pdaKey = "pda";
        private void SetPDASession(PDA pda)
        {
            this.HttpContext.Session.SetString(_pdaKey, JsonConvert.SerializeObject(pda));
        }

        private PDA GetPDASession()
        {
            string pda = this.HttpContext.Session.GetString(_pdaKey);
            return JsonConvert.DeserializeObject<PDA>(pda);

        }
        #endregion

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
