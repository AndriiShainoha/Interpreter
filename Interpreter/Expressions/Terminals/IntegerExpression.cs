using Interpreter.Optimizer;
using System.Text;

namespace Interpreter.Expressions.Terminals
{
    class IntegerExpression : IExpression
    {
        public int Value { get; }

        public IntegerExpression(int value)
        {
            Value = value;
        }

        public object Run(Context context)
        {
            return Value;
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
            res.AppendLine(ToString());
            return res.ToString();
        }

        public override string ToString() => "Integer: " + Value;
    }
}
