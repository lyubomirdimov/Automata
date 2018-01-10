using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Automata;
using Microsoft.AspNetCore.Mvc;

namespace AleWebApp2.Models
{
    public class FiniteStateAutomatonViewModel
    {
        public bool IsAutomatonBuilt { get; set; }

        public bool IsDFA { get; set; }
        public bool IsFinite { get; set; } = false;
        public bool Accepts { get; set; }
        public List<string> AcceptedWords { get; set; } = new List<string>();

        [Display(Name = "Input String", Prompt = "Fill Input String")]
        public string InputString { get; set; } = "";

        [Display(Name = "Regular Expression", Prompt = "Fill Regular Expression")]
        [Remote("ValidateRegex", "Home", HttpMethod = "POST", ErrorMessage = "Invalid Regular Expression")]
        public string RegularExpression { get; set; } = "";
        public string RegularExpressionToInfix { get; set; } = "";

        public FiniteStateAutomaton FSM { get; set; }

        public FiniteStateAutomatonViewModel()
        {
            
        }
        public FiniteStateAutomatonViewModel(FiniteStateAutomaton fsm)
        {
            FSM = fsm;
            IsDFA = fsm.IsDFA();
            IsFinite = fsm.IsInfinite() == false;
            AcceptedWords = fsm.AcceptedWords();
        }

       
        public FiniteStateAutomatonViewModel(string regex)
        {
            FiniteStateAutomaton fsm = AutomataConstructor.RegexToNfa(regex);
            FSM = fsm;
            IsDFA = fsm.IsDFA();
            IsFinite = fsm.IsInfinite() == false;
            AcceptedWords = fsm.AcceptedWords();
            Node tree = TreeConstructor.ConstructTree(regex.ParseRegex());
            RegularExpressionToInfix = tree.ToInfixNotation();
        }
            
        
    }

}