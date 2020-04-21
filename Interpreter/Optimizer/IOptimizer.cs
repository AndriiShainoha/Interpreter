using Interpreter.Expressions;
using Interpreter.Expressions.Keywords;
using Interpreter.Expressions.Operators;

namespace Interpreter.Optimizer
{
    internal interface IOptimizer
    {
        IExpression Optimize(AddExpression add);
        IExpression Optimize(SubtractExpression subtract);
        IExpression Optimize(MultiplyExpression multiply);
        IExpression Optimize(DivideExpression divide);
        IExpression Optimize(EqualExpression equal);
        IExpression Optimize(NotEqualExpression notEqual);
        IExpression Optimize(LessExpression less);
        IExpression Optimize(GreaterExpression greater);
        IExpression Optimize(AssignExpression assign);
        IExpression Optimize(CycleExpression cycle);
        IExpression Optimize(ConditionalExpression conditional);
        IExpression Optimize(BlockExpression block);
        IExpression Optimize(IExpression expression);
    }
}
