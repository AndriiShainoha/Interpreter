using Interpreter.Browser;
using Interpreter.Optimizer;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Interpreter.Expressions.Keywords
{
    class OpenExpression : IExpression
    {
        public IExpression Url { get; set; }

        public OpenExpression(IExpression url)
        {
            Url = url;
        }

        public object Run(Context context)
        {
            var urlRes = (string)Url.Run(context);
            context.BrowserClient.AddWaiter();

            if (Regex.IsMatch(urlRes, @"^(http://|https://)"))
            {
                var request = WebRequest.Create(urlRes);
                request.GetResponse();
                context.BrowserClient.Navigate(urlRes);
            }
            else
            {
                var request = WebRequest.Create(context.BrowserClient.Url.AbsoluteUri + urlRes);
                request.GetResponse();
                context.BrowserClient.Navigate(context.BrowserClient.Url.AbsoluteUri + urlRes);
            }

            while (context.BrowserClient.IsLoading() ?? false)
            {
                context.BrowserClient.WaitForLoad();
            }
            context.BrowserClient.RemoveWaiter();
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
            res.Append(Url.ToConsoleTree(level + 1));
            return res.ToString();
        }

        public override string ToString() => "Open";
    }
}
