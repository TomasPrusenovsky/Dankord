using System.Net;
using System.Diagnostics;

namespace DankordServerApp
{
	public partial class ServerForm : Form
	{
		public const string WindowHeader = "ServerForm";

		public static ServerForm? __instance;

		private Server? server;

		public ServerForm()
		{
			InitializeComponent();
			__instance = this;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
		}

		private void StartButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (server != null)
				{
					StartButton.Text = "Stopping...";
					server.Stop();
					server = null;
					StartButton.Text = "Start";
					portInputBox.Enabled = true;
				}
				else
				{
					StartButton.Text = "Starting...";
					int port = int.Parse(portInputBox.Text.Trim());
					server = new Server(port); // Also automatically starts server
					StartButton.Text = "Stop";
					portInputBox.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, WindowHeader);
				if (server != null)
				{
					server.Stop();
					server = null;
				}
				StartButton.Text = "Start";
			}
		}

		public void RefreshOutputWindow()
		{
			if (server != null)
				OutputTextWindow.Text = server.messageLog;
			else
				OutputTextWindow.Text = string.Empty;
		}
	}
}