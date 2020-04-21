using Interpreter.Optimizer;
using System.Text;

namespace Interpreter.Expressions.Terminals
{
    class StringExpression : IExpression
    {
        public string Value { get; }

        public StringExpression(string value)
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

        public override string ToString() => "String: " + Value;
    }
}
