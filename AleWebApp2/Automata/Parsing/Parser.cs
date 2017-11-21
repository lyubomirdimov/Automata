using System.Collections.Generic;
using System.Linq;

namespace Automata.Parsing
{
    public static class Parser
    {
        /// <summary>
        /// Parses a Logical ASCII string Proposition to a List of Tokens
        /// </summary>
        /// <param name="input">Logical Ascii string proposition, which is parsed to a List of tokens</param>
        public static List<Token> ParseRegex(this string input) => input.ToCharArray().Select(c => new Token(c)).ToList();
    }
}
