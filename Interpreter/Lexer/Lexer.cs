using Interpreter.Tokens;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Interpreter.Lexer
{
    class Lexer : ILexer
    {
        private TokenDefinitions _tokenDefinitions;

        public Lexer(TokenDefinitions tokenDefs)
        {
            _tokenDefinitions = tokenDefs;

        }

        public Queue<Token> Lex(string programText)
        {
            Queue<Token> tokens = new Queue<Token>();
            var remainingText = "" + programText;

            while (!string.IsNullOrEmpty(remainingText))
            {
                bool matchFound = false;
                foreach (var exp in _tokenDefinitions.Definitions.Keys)
                {
                    var match = CheckMatch(remainingText, exp);
                    if (match == null) continue;
                    remainingText = remainingText.Substring(match.Value.Length);
                    matchFound = true;
                    if (_tokenDefinitions.Definitions[exp] == TokenType.None) break;
                    tokens.Enqueue(new Token(_tokenDefinitions.Definitions[exp], match.Value));
                    break;
                }
                if (!matchFound) throw new InvalidExpressionException();
            }

            return tokens;
        }

        private Match CheckMatch(string text, string expression)
        {
            var match = Regex.Match(text, expression);
            return (match.Index == 0 && !string.IsNullOrEmpty(match.Value)) ? match : null;
        }
    }
}
