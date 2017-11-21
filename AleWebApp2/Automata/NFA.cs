using System;
using System.Collections.Generic;
using System.Text;

namespace Automata
{
    public class NFA
    {
        public NFA(string regularExpression)
        {
            List<Token> regex = regularExpression.ParseRegex();
            RegexToNFA(regex);
        }


        /// <summary>
        /// Construction of NFA using Thompson construct
        /// </summary>
        /// <param name="regex"></param>
        private void RegexToNFA(List<Token> regex)
        {
            
            foreach (Token token in regex)
            {
                switch (token.Type)
                {
                    case TokenType.Epsion:
                        
                        break;
                    case TokenType.Letter:
                        break;
                    case TokenType.Concatenation:
                        break;
                    case TokenType.Union:
                        break;
                    case TokenType.KleeneStar:
                        break;
                    case TokenType.OpeningParenthesis:
                    case TokenType.ClosingParenthesis:
                    case TokenType.Separation:
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
