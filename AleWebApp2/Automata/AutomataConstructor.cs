//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Automata
//{
//    public class AutomataConstructor
//    {

//        public NFA RegexToNfa(string regex, NFA r)
//        {
//            List<Token> tokens = regex.ParseRegex();

//            Token token = regex[0].ToToken();

//            NFA newR;
//            switch (token.Type)
//            {
//                case TokenType.Epsion:
//                    newR = new NFA
//                    {
//                        InitialState = "q0",
//                        States = new List<string>() { "q0", "q1" },
//                        FinalStates = new List<string>() { "q1" },
//                        Transitions = new List<Transition>() { new Transition("q0", "q1", Constants.Epsilon) }
//                    };

//                    break;
//                case TokenType.Letter:
//                    newR = new NFA
//                    {
//                        InitialState = "q0",
//                        States = new List<string>() { "q0", "q1" },
//                        FinalStates = new List<string>() { "q1" },
//                        Transitions = new List<Transition>() { new Transition("q0", "q1", Constants.Epsilon) }
//                    };
//                    break;
//                case TokenType.Concatenation:






//                    break;
//                case TokenType.Union:
//                    break;
//                case TokenType.KleeneStar:
//                    break;
//                case TokenType.OpeningParenthesis:
//                    break;
//                case TokenType.ClosingParenthesis:
//                    break;
//                case TokenType.Separation:
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }

//        }


//    }
//}
