using Interpreter.Optimizer;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Expressions
{
    class BlockExpression : IExpression
    {
        public IList<IExpression> Expressions { get; }

        public BlockExpression(IList<IExpression> expressions)
        {
            Expressions = expressions;
        }

        public object Run(Context context)
        {
            foreach (var expression in Expressions)
            {
                expression.Run(context);
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
            foreach (var exp in Expressions)
            {
                res.Append(exp.ToConsoleTree(level + 1));
            }

            return res.ToString();
        }

        public override string ToString() => "Block";
    }
}
