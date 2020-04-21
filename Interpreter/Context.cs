using Interpreter.Browser;
using System;
using System.Collections;
using System.Threading;
using System.Windows.Forms;

namespace Interpreter
{
    public class Context : IDisposable
    {
        public WebBrowser BrowserClient { get; private set; }
        private readonly Thread _browserAppThread;

        public Hashtable Variables { get; }

        public event EventHandler ApplicationClosing;

        public Context(string nameOfProgram = null)
        {
            Variables = new Hashtable();

            Barrier browserInitializedBarrier = new Barrier(2);
            _browserAppThread = new Thread(() =>
            {

                var browserForm = new BrowserForm { Text = nameOfProgram ?? "Browser" };
                BrowserClient = new WebBrowser();
                browserForm.Browser = BrowserClient;
                ApplicationClosing += browserForm.ApplicationClosing;

                browserInitializedBarrier.SignalAndWait();
                Application.Run(browserForm);
            });

            _browserAppThread.SetApartmentState(ApartmentState.STA);
            _browserAppThread.Start();
            browserInitializedBarrier.SignalAndWait();
            browserInitializedBarrier.Dispose();
        }

        public void Dispose()
        {
            ApplicationClosing?.Invoke(this, new EventArgs());
            _browserAppThread.Abort();
        }
    }
}
