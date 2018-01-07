using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Automata;

namespace AleWebApp2.Models
{
    public class FiniteStateAutomatonViewModel
    {
        public bool IsAutomatonBuilt { get; set; }

        public bool IsDFA { get; set; }
        public bool IsFinite { get; set; }
        public bool Accepts { get; set; }

        [Display(Name = "Input String", Prompt = "Fill Input String")]
        public string InputString { get; set; } = "";

        [Display(Name = "Regular Expression", Prompt = "Fill Regular Expression")]
        public string RegularExpression { get; set; } = "";

        public FiniteStateAutomaton FSM { get; set; }

        public FiniteStateAutomatonViewModel()
        {
            
        }
        public FiniteStateAutomatonViewModel(FiniteStateAutomaton fsm)
        {
            FSM = fsm;
            IsDFA = fsm.IsDFA();
            IsFinite = false;
        }

       
        public FiniteStateAutomatonViewModel(string regex)
        {
            AutomataConstructor constructor = new  AutomataConstructor();
            FiniteStateAutomaton fsm = constructor.RegexToNfa(regex);
            FSM = fsm;
            IsDFA = fsm.IsDFA();
            IsFinite = false;
        }
            
        
    }

}