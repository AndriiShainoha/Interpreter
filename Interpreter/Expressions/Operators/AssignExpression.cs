using Interpreter.Optimizer;
using System.Text;

namespace Interpreter.Expressions.Operators
{
    class AssignExpression : IExpression
    {
        public string VariableName { get; }
        public IExpression Expression { get; set; }

        public AssignExpression(string variableName, IExpression expression)
        {
            VariableName = variableName;
            Expression = expression;
        }

        public object Run(Context context)
        {
            context.Variables[VariableName] = Expression.Run(context);
            return context.Variables[VariableName];
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
            for (int i = 0; i < (level + 1); ++i)
            {
                res.Append("|\t");
            }
            res.AppendLine("Variable: " + VariableName);
            res.Append(Expression.ToConsoleTree(level + 1));
            return res.ToString();
        }

        public override string ToString() => "Assign";
    }
}
