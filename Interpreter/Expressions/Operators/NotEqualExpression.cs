using Interpreter.Optimizer;
using System.Text;

namespace Interpreter.Expressions.Operators
{
    class NotEqualExpression : IExpression
    {
        public IExpression Expression1 { get; set; }
        public IExpression Expression2 { get; set; }

        public NotEqualExpression(IExpression expression1, IExpression expression2)
        {
            Expression1 = expression1;
            Expression2 = expression2;
        }

        public object Run(Context context)
        {
            var firstOperandObj = Expression1.Run(context);
            var secondOperandObj = Expression2.Run(context);
            if (firstOperandObj is double || secondOperandObj is double)
            {
                return (double)firstOperandObj != (double)secondOperandObj;
            }
            return (int)firstOperandObj != (int)secondOperandObj;
        }

        public IExpression AcceptOptimizer(IOptimizer optimizer)
        {
            return optimizer.Optimize(this);
        }

        public string ToConsoleTree(int level = 0)
        {
            var res = new StringBuilder();
            for (int i = 0; i < level; ++i)
            {
                res.Append("|\t");
            }
            res.AppendLine(ToString() + ":");
            res.Append(Expression1.ToConsoleTree(level + 1));
            res.Append(Expression2.ToConsoleTree(level + 1));
            return res.ToString();
        }

        public override string ToString() => "NotEqual";
    }
}
