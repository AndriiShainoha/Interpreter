using System;
using System.Threading;
using System.Windows.Forms;

namespace Interpreter.Browser
{
    public partial class BrowserForm : Form
    {
        private Barrier _browserLoadBarrier;

        public Barrier BrowserLoadBarrier
        {
            get => _browserLoadBarrier;
            set
            {
                if (_browserLoadBarrier == null)
                {
                    _browserLoadBarrier = value;
                }
            }
        }

        public WebBrowser Browser
        {
            get => WebBrowser;
            set
            {
                if (WebBrowser == null)
                {
                    WebBrowser = value;
                    this.Controls.Add(WebBrowser);
                    WebBrowser.Dock = DockStyle.Fill;
                    WebBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
                }
            }
        }

        public BrowserForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            BrowserLoadBarrier = new Barrier(1);
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _browserLoadBarrier.SignalAndWait();
        }

        public void ApplicationClosing(object sender, EventArgs e)
        {
            this._browserLoadBarrier.Dispose();
            this.Invoke((MethodInvoker)this.Close);
        }


    }
}
