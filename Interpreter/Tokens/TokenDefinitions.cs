using System.Collections.Generic;

namespace Interpreter.Tokens
{
    class TokenDefinitions
    {
        public IDictionary<string, TokenType> Definitions { get; }

        public TokenDefinitions()
        {
            Definitions = new Dictionary<string, TokenType>
            {
                {@"^\d+\.\d+", TokenType.Double },
                {@"^\d+", TokenType.Integer},
                {@"^open", TokenType.Open },
                {@"^find", TokenType.Find },
                {@"^type", TokenType.Type },
                {@"^click", TokenType.Click },
                {@"^while", TokenType.Cycle },
                {@"^if", TokenType.If },
                {@"^else", TokenType.Else },
                {@"log", TokenType.Log },
                {@"^(?:true|false)", TokenType.Bool },
                {@"^==", TokenType.Equal },
                {@"^!=", TokenType.NotEqual },
                {@"^:", TokenType.Colon },
                {@"^'[^']*'", TokenType.String },
                {@"^,", TokenType.Coma },
                {@"^\(", TokenType.LeftParentness },
                {@"^\)", TokenType.RightParentness },
                {@"^{", TokenType.LeftBracket },
                {@"^}", TokenType.RightBracket },
                {@"^<", TokenType.Less },
                {@"^>", TokenType.Greater },
                {@"^\+\+", TokenType.Increment },
                {@"^\+", TokenType.Add },
                {@"^\*", TokenType.Multiply },
                {@"^-", TokenType.Subtract },
                {@"^\\", TokenType.Divide },
                {@"^[a-zA-Z_][a-zA-Z_0-9]*", TokenType.Variable },
                {@"^=", TokenType.Assign },
                {@"^;", TokenType.EndOfOperation },
                {@"^\s+", TokenType.None }
            };
        }
    }
}
