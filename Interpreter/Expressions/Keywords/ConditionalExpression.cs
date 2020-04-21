using Interpreter.Optimizer;
using System.Text;

namespace Interpreter.Expressions.Keywords
{
    class ConditionalExpression : IExpression
    {
        public IExpression Condition { get; set; }
        public IExpression TrueExpression { get; set; }
        public IExpression FalseExpression { get; set; }

        public ConditionalExpression(IExpression condition, IExpression trueExpression, IExpression falseExpression)
        {
            Condition = condition;
            TrueExpression = trueExpression;
            FalseExpression = falseExpression;
        }

        public object Run(Context context)
        {
            if ((bool)Condition.Run(context))
            {
                TrueExpression.Run(context);
            }
            else
            {
                FalseExpression?.Run(context);
            }
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
            res.Append(TrueExpression.ToConsoleTree(level + 1));
            res.Append(FalseExpression?.ToConsoleTree(level + 1));
            return res.ToString();
        }

        public override string ToString() => "Conditional";
    }
}
