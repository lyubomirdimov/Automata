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
            StreamReader stream = new StreamReader(@"../Automata/PDAs/pdaAlgbr.txt");
            return FileParser.FileToPDA(reader: stream).Pda;
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
