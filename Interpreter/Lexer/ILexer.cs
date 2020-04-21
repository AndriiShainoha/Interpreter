using Interpreter.Tokens;
using System.Collections.Generic;

namespace Interpreter.Lexer
{
    interface ILexer
    {
        Queue<Token> Lex(string programText);
    }
}
