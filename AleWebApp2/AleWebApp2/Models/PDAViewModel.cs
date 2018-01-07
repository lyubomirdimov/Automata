using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Automata;

namespace AleWebApp2.Models
{
    public class PDAViewModel
    {

        public PDA PDA { get; set; }
        public string PDAString { get; set; }
    }
}
