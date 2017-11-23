using System;
using System.Collections.Generic;
using System.Text;

namespace Automata
{
    public class NFA
    {
        public string Comment { get; set; }

        // a finite set of states (Q)
        public List<string> States { get; set; }

        // a finite set of input symbols called the alphabet(Σ)
        public List<char> Alphabet { get; set; }

        public string InitialState { get; set; }

        public List<string> FinalStates { get; set; }

        // a transition function(δ : Q × Σ → Q)
        public List<Transition> Transitions { get; set; } = new List<Transition>();

        public NFA()
        {
            
        }
        public NFA(string regularExpression)
        {
            List<Token> regex = regularExpression.ParseRegex();
            //RegexToNFA(regex);
        }


        /// <summary>
        /// Construction of NFA using Thompson construct
        /// </summary>
        /// <param name="regex"></param>
        private void RegexToNFA(List<Token> regex, NFA nfa)
        {
            Token initToken = regex[0];
            switch (initToken.Type)
            {
                case TokenType.Epsion:
                    // 
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
                    break;
                case TokenType.ClosingParenthesis:
                    break;
                case TokenType.Separation:
                    break;
            }

        }

       
    }
}
