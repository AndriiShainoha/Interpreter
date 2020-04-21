using Interpreter.Optimizer;
using System;
using System.Text;

namespace Interpreter.Expressions.Terminals
{
    class VariableExpression : IExpression
    {
        private string _name;

        public VariableExpression(string name)
        {
            _name = name;
        }

        public object Run(Context context)
        {
            if (context.Variables.ContainsKey(_name) && context.Variables[_name] != null)
            {
                return context.Variables[_name];
            }

            throw new ArgumentNullException();
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

        public override string ToString() => "Variable: " + _name;
    }
}
