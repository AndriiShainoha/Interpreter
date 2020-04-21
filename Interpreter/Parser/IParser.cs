using Interpreter.Expressions;
using Interpreter.Tokens;
using System.Collections.Generic;

namespace Interpreter.Parser
{
    interface IParser
    {
        IExpression Parse(Queue<Token> tokens);
    }
}
