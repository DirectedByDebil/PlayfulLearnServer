using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace Server
{
    public partial class ServerPresenter : Form
    {

        private readonly HttpListener _listener;

        private readonly UriBuilder _builder;

        private readonly ServerModel _model;


        private bool _isActive;


        public ServerPresenter()
        {

            _listener = new HttpListener();

            _model = new ServerModel();


            _builder = new UriBuilder("http", "127.0.0.1", 8888);


            SetPrefixes(_model.Paths);


            InitializeComponent();
        }


        private void SetPrefixes(string[] prefixes)
        {

            foreach(string prefix in prefixes)
            {

                _builder.Path = prefix;

                _listener.Prefixes.Add(_builder.Uri.AbsoluteUri);
            }
        }



        private async void OnStartClicked(object sender, EventArgs e)
        {

            _listener.Start();
            
            _model.GotMessage += Print;

            _isActive = true;


            Print("Server started");


            await Task.Run(ListenContextAsync);
        }


        private void Print(string text)
        {
            
            if(InvokeRequired)
            {

                Invoke((Action<string>) Print, text);
            }
            else
            {

                output.Text += string.Concat(text, "\n");
            }
        }


        private async Task ListenContextAsync()
        {

            while (_isActive)
            {

                Task<HttpListenerContext> task = _listener.GetContextAsync();
                

                HttpListenerContext context = await task;

                await _model.HandleContextAsync(context);


                Print("----------------------------------------------------------------");
            }


            Print("Server stopped");
        }


        private void OnStopClicked(object sender, EventArgs e)
        {

            _isActive = false;

            _model.GotMessage -= Print;
        }


        private void OnCloseClicked(object sender, EventArgs e)
        {

            _listener.Close();

            Print("Server closed");

            Application.Exit();
        }
    }
}
