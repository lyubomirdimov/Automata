﻿using System;
using System.Net;

namespace Automata
{

    public static class Constants
    {
        public static char Epsilon = 'ϵ';
    }
    
    /// <summary>
    /// Token type represents the equivalent logical proposition statement for certain token
    /// Token Type accepts Connectives, Predicates, Separator, and Paranthesis
    /// </summary>
    public enum TokenType
    {
        Epsion,
        Letter,
        Concatenation,
        Union,
        KleeneStar,
        OpeningParenthesis,
        ClosingParenthesis,
        Separation

    }

    /// <summary>
    /// Used for parsing a Ascii string   
    /// </summary>
    public class Token
    {
        public TokenType Type { get; set; }
        public char Char { get; private set; }

        public bool IsOperation => Type == TokenType.Concatenation ||
                                       Type == TokenType.Union ||
                                       Type == TokenType.KleeneStar;

        public bool IsEpsilon => Type == TokenType.Epsion;
        public bool IsLetter => Type == TokenType.Letter;
        public bool IsConcatenation => Type == TokenType.Concatenation;
        public bool IsUnion => Type == TokenType.Union;
        public bool IsKleeneStar => Type == TokenType.KleeneStar;

        public bool IsOpeningParenthesis => Type == TokenType.OpeningParenthesis;
        public bool IsClosingParenthesis => Type == TokenType.ClosingParenthesis;
        public bool IsSeparation => Type == TokenType.Separation;

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
                        Type = TokenType.Epsion;
                        break;
                    case '.':
                        Type = TokenType.Concatenation;
                        break;
                    case '|':
                        Type = TokenType.Union;
                        break;
                    case '*':
                        Type = TokenType.KleeneStar;
                        break;
                    case '(':
                        Type = TokenType.OpeningParenthesis;
                        break;
                    case ')':
                        Type = TokenType.ClosingParenthesis;
                        break;
                    case ',':
                        Type = TokenType.Separation;
                        break;

                    default:
                        Type = TokenType.Letter;
                        break;
                }
                Char = (char)c;
            }
        }

        public override string ToString() => Char.ToString();

        public string ToInfixString()
        {
            switch (Type)
            {
                case TokenType.Concatenation:
                    return "";
                case TokenType.Union:
                    return "|";
                case TokenType.Epsion:
                    return Constants.Epsilon.ToString();
                case TokenType.KleeneStar:
                    return "*";
                case TokenType.Letter:
                    return Char.ToString();
                case TokenType.Separation:
                case TokenType.OpeningParenthesis:
                case TokenType.ClosingParenthesis:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public static class TokenHelper
    {
        public static Token ToToken(this char input) => new Token(input);
    }
}