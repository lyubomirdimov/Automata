using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Automata;

namespace AleWebApp2.Models
{
    public class FiniteStateViewModel
    {
        public bool IsAutomatonBuilt { get; set; }


        public bool IsDFA { get; set; }

        [Display(Name = "String")]
        public string InputString { get; set; }

        [Display(Name = "Regular Expression")]
        public string RegularExpression { get; set; }

        public FiniteStateAutomaton NFA { get; set; }
    }

}