using Interpreter.Optimizer;
using System.Text;

namespace Interpreter.Expressions.Keywords
{
    class CycleExpression : IExpression
    {
        public IExpression Condition;
        public IExpression Expression;

        public CycleExpression(IExpression condition, IExpression expression)
        {
            Expression = expression;
            Condition = condition;
        }

        public object Run(Context context)
        {
            while ((bool)Condition.Run(context))
                Expression?.Run(context);
            return null;
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
            res.Append(Condition.ToConsoleTree(level + 1));
            res.Append(Expression?.ToConsoleTree(level + 1));
            return res.ToString();
        }

        public override string ToString() => "Cycle";
    }
}
