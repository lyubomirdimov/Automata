using System;

namespace Automata.Parsing
{

    /// <summary>
    /// Token type represents the equivalent logical proposition statement for certain token
    /// Token Type accepts Connectives, Predicates, Separator, and Paranthesis
    /// </summary>
    public enum TokenType
    {
        Epsion,
        Letter,
        Concatenation,
        Disjunction,
        KleeneStar

    }

    /// <summary>
    /// Used for parsing a Ascii string   
    /// </summary>
    public class Token
    {
        public TokenType Type { get; set; }
        public char Char { get; private set; }
        public bool IsEpsilon => Type == TokenType.Epsion;
        public bool IsLetter => Type == TokenType.Letter;
        public bool IsConcatenation => Type == TokenType.Concatenation;
        public bool IsDisjunction => Type == TokenType.Disjunction;
        public bool IsAsterisk => Type == TokenType.KleeneStar;
        public bool IsValid { get; set; } = true;

        public Token(char? c)
        {
            if (c == null)
            {
                IsValid = false;
            }
            else
            {
                switch (c)
                    // Define the type of the Character
                    {
                        case '_':
                            Type = TokenType.KleeneStar;
                            break;
                        case '.':
                            Type = TokenType.Concatenation;
                            break;
                        case '|':
                            Type = TokenType.Disjunction;
                            break;
                        case '*':
                            Type = TokenType.KleeneStar;
                            break;
                   
                        default:
                            Type = TokenType.Letter;
                            break;
                    }
                Char = (char) c;
            }
        }

        public override string ToString() => Char.ToString();

    }
}