using Interpreter.Tokens;

namespace Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            (new Interpreter(new Lexer.Lexer(new TokenDefinitions()), new Parser.Parser(), new Optimizer.Optimizer())).Run();
        }
    }
}
