using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Interpreter.Browser
{
    public static class BrowserExtensions
    {
        private delegate bool? BoolHandler(WebBrowser browser);

        private delegate IList<HtmlElement> ListOfHtmlElementsHandler(WebBrowser browser, string name);

        private delegate HtmlElementCollection HtmlCollectionHandler(WebBrowser browser, string name);

        private delegate HtmlElement HtmlElementHandler(WebBrowser browser, string name);

        public static void AddWaiter(this WebBrowser browser)
        {
            var barrier = (browser.FindForm() as BrowserForm)?.BrowserLoadBarrier;
            barrier?.AddParticipant();
        }

        public static void RemoveWaiter(this WebBrowser browser)
        {
            var barrier = (browser.FindForm() as BrowserForm)?.BrowserLoadBarrier;
            barrier?.RemoveParticipant();
        }

        public static void WaitForLoad(this WebBrowser browser)
        {
            var barrier = (browser.FindForm() as BrowserForm)?.BrowserLoadBarrier;
            barrier?.SignalAndWait();
        }

        public static bool? IsLoading(this WebBrowser browser)
        {
            if (browser.InvokeRequired)
            {
                return browser.Invoke(new BoolHandler(IsLoading), browser) as bool?;
            }

            return browser.ReadyState != WebBrowserReadyState.Complete;
        }

        public static HtmlElementCollection GetElementsByNameConcurrent(this WebBrowser browser, string name)
        {
            if (browser.InvokeRequired)
            {
                return browser.Invoke(new HtmlCollectionHandler(GetElementsByNameConcurrent), browser, name) as HtmlElementCollection;
            }

            return browser.Document?.All.GetElementsByName(name);
        }

        public static HtmlElementCollection GetElementsByTagNameConcurrent(this WebBrowser browser, string tagName)
        {
            if (browser.InvokeRequired)
            {
                return browser.Invoke(new HtmlCollectionHandler(GetElementsByTagNameConcurrent), browser, tagName) as HtmlElementCollection;
            }

            return browser.Document?.GetElementsByTagName(tagName);
        }

        public static IList<HtmlElement> GetElementsByClassConcurrent(this WebBrowser browser, string className)
        {
            if (browser.InvokeRequired)
            {
                return browser.Invoke(new ListOfHtmlElementsHandler(GetElementsByClassConcurrent), browser, className) as List<HtmlElement>;
            }

            var elements = new List<HtmlElement>();

            if (browser.Document?.All == null) return elements;
            foreach (HtmlElement el in browser.Document.All)
            {
                if (!Regex.IsMatch(el.GetAttribute("className"), $@"\b{className}\b")) continue;
                elements.Add(el);
            }

            return elements;
        }

        public static HtmlElement GetElementByIdConcurrent(this WebBrowser browser, string id)
        {
            if (browser.InvokeRequired)
            {
                return browser.Invoke(new HtmlElementHandler(GetElementByIdConcurrent), browser, id) as HtmlElement;
            }
            else
            {
                return browser.Document?.GetElementById(id);
            }
        }
    }
}
