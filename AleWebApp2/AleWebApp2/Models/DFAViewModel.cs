using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Automata;

namespace AleWebApp2.Models
{
    public class DFAViewModel
    {
        public bool IsAutomatonBuilt { get; set; }
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public bool IsDFA { get; set; }

        [Display(Name = "String")]
        public string AutomataString { get; set; }

        [Display(Name = "Regular Expression")]
        public string RegularExpression { get; set; }
    }

    public class Edge
    {
        public string from { get; set; }
        public string to { get; set; }
        public string arrows { get; set; }
        public string label { get; set; }

    }

    public class Node
    {
        public string id { get; set; }
        public string label { get; set; }
        public string color { get; set; }
    }
}
