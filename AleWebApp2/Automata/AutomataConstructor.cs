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
            var finiteStateAutomaton = tree.ThomsonConstruct();
            return new FiniteStateAutomaton();
        }

        private FiniteStateAutomaton RegexToNfa(List<Token> tokens)
        {
            FiniteStateAutomaton result = new FiniteStateAutomaton()
            {
                Alphabet = tokens.Where(x => x.IsLetter || x.IsEpsilon).Select(token => token.Char).Distinct().ToList(),
            };

            FiniteStateAutomaton s = null;
            FiniteStateAutomaton t = null;

            // Check validity of Automaton and return
            if (tokens.Any() == false)
            {
                // Exactly one Initial State

                // Exactly one Final State

                // Let c be number of concatenations
                // Let s be number of symbols apart from paranthesis
                // Then the number of states A = 2s - c

                // The number of transitions leaving any state is at most 2

                return s;
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                Token currentToken = tokens[i];

                switch (currentToken.Type)
                {
                    // q0 -> q1
                    case TokenType.Epsion:
                    case TokenType.Letter:
                        if (s == null)
                        {
                            
                        }
                        string init = "q0" + i;
                        string fin = "qf" + i;
                        FiniteStateAutomaton at = new FiniteStateAutomaton();
                        at.InitialState = init;
                        at.Alphabet = new List<char> { currentToken.Char };
                        at.FinalStates = new List<string> { fin };
                        at.States = new List<string> { init, fin };
                        break;
                    case TokenType.Concatenation:
                        string initState = "q0" + i.ToString();
                        string finState = "qf" + i.ToString();
                        TransitionFunction tr = new TransitionFunction(s.FinalStates.FirstOrDefault(), t.InitialState, Constants.Epsilon);
                        FiniteStateAutomaton aut = new FiniteStateAutomaton();

                        break;
                    case TokenType.Union:
                        break;
                    case TokenType.KleeneStar:
                        break;
                    case TokenType.OpeningParenthesis:
                        break;
                    case TokenType.ClosingParenthesis:
                        break;
                    case TokenType.Separation:
                        break;
                }
            }
            return result;
        }





    }
}
