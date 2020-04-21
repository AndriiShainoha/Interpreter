using Interpreter.Optimizer;
using System;
using System.Text;

namespace Interpreter.Expressions.Keywords
{
    class LogExpression : IExpression
    {
        public IExpression LogMessage { get; set; }

        public LogExpression(IExpression logMessage)
        {
            LogMessage = logMessage;
        }

        public object Run(Context context)
        {
            Console.WriteLine(LogMessage.Run(context));
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
            res.Append(LogMessage.ToConsoleTree(level + 1));
            return res.ToString();
        }

        public override string ToString() => "Log";
    }
}
