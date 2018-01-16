using System.Text.RegularExpressions;

namespace Automata
{
    public static class RegularExpressionValidator
    {
        public static Regex RegularExpressionMatchRegex => new Regex(


            @"^                                         # Start of line
                  (                                     # Either
                      [0-9A-Za-z_](?![0-9A-Za-z_]) |    # A predicate
                      (?<couple>[|.]\((?!,))|           # Start of couple
                      (?<comma-couple>,(?!\)))|         # Looks for comma followed by couple. Pops off the couple stack.
                      (?<dBracket-comma>\))|            # Looks for ending bracket following comma. pops off comma stack. 
                      (?<single>\*\((?!\)))|            # Start of single function.
                      (?<uBracket-single>\)))           # Looks for ending bracket for unary. Pops off the single stack. 
                  +                                     # Any number of times.
                  (?(couple)(?!))                       # Assert couple stack is empty. All have a comma.
                  (?(comma)(?!))                        # Assert comma stack is empty. All couple commas followed by parenthesis.
                  (?(single)(?!))                       # Assert single stack is empty. All single expressions have closing brackets.
                  $"
            , RegexOptions.IgnorePatternWhitespace);

        public static bool Validate(string input)
        {
            Regex r = RegularExpressionMatchRegex;
            return r.Match(input).Success;
        }

        public static string RemoveWhiteSpaces(this string input)
        {
            Regex rgx = new Regex("\\s+");
            return rgx.Replace(input, "");
        }

    }

}
