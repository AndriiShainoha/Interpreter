using Interpreter.Browser;
using Interpreter.Optimizer;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interpreter.Expressions.Keywords
{
    class TypeExpression : IExpression
    {
        public IExpression Target { get; set; }
        public IExpression Text { get; set; }

        private delegate HtmlElementCollection GetCollection(WebBrowser browser, string tagName);

        private delegate HtmlElement GetElement(WebBrowser browser, string id);

        public TypeExpression(IExpression target, IExpression text)
        {
            Target = target;
            Text = text;
        }

        public object Run(Context context)
        {
            var targetRes = (string)Target.Run(context);
            var textRes = (string)Text.Run(context);
            var targetArgs = targetRes.Split('=').Select(s => s.Trim()).ToArray();
            HtmlElement target;
            switch (targetArgs[0])
            {
                case "name":
                    target = context.BrowserClient.GetElementsByNameConcurrent(targetArgs[1]).OfType<HtmlElement>()
                        .FirstOrDefault();
                    break;
                case "class":
                    target = context.BrowserClient.GetElementsByClassConcurrent(targetArgs[1]).FirstOrDefault();
                    break;
                case "tag":
                    target = context.BrowserClient.GetElementsByTagNameConcurrent(targetArgs[1]).OfType<HtmlElement>()
                        .FirstOrDefault();
                    break;
                case "id":
                    target = context.BrowserClient.GetElementByIdConcurrent(targetArgs[1]);
                    break;
                default:
                    throw new ArgumentException();
            }

            target?.SetAttribute("value", target.GetAttribute("value") + textRes);

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
            res.Append(Target.ToConsoleTree(level + 1));
            res.Append(Text.ToConsoleTree(level + 1));
            return res.ToString();
        }

        public override string ToString() => "Type";
    }
}
