using Interpreter.Optimizer;

namespace Interpreter.Expressions
{
    interface IExpression
    {
        object Run(Context context);
        IExpression AcceptOptimizer(IOptimizer optimizer);
        string ToConsoleTree(int level = 0);
    }
}
