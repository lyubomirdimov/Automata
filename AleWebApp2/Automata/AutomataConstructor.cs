using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata
{
    public static class AutomataConstructor
    {
        /// <summary>
        /// Thompson Construction
        /// </summary>
        public static FiniteStateAutomaton RegexToNfa(string regex)
        {
            List<Token> tokens = regex.ParseRegex();

            Node tree = TreeConstructor.ConstructTree(tokens);

            return RegexToNfa(tree);
        }

        private static FiniteStateAutomaton RegexToNfa(Node tree)
        {
            return tree.ThomsonConstruct();
        }

        public static FiniteStateAutomaton RandomFSM()
        {
            return RegexToNfa(TreeConstructor.ConstructRandomTree().ToPrefixNotation());
        }

        public static FiniteStateAutomaton RandomDFA()
        {
            return RegexToNfa(TreeConstructor.ConstructRandomTree().ToPrefixNotation()).ToDfa();
        }

    }
}
