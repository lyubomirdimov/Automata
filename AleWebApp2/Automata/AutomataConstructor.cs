using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata
{
    public class AutomataConstructor
    {
        /// <summary>
        /// Thompson Construction
        /// </summary>
        public FiniteStateAutomaton RegexToNfa(string regex)
        {
            List<Token> tokens = regex.ParseRegex();

            Node tree = TreeConstructor.ConstructTree(tokens);

            return RegexToNfa(tree);
        }

        private FiniteStateAutomaton RegexToNfa(Node tree)
        {
            return tree.ThomsonConstruct();
        }

    }
}
