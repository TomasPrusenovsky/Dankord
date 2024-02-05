using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using ServerApp;

namespace Chat_App
{
	public partial class DankordWindow : Form
	{
		private TcpClient _client;

		private string UserName { get => nameBox.Text; set { nameBox.Text = value; } }

		//private List<TcpClient> _clients;

		public TcpListener listener;
		public StreamReader reader;
		public StreamWriter writer;

		public DankordWindow()
		{
			InitializeComponent();
			UserName = "Danek";
			//_clients = new List<TcpClient>();
		}

		private void ConnectClient_Click(object sender, EventArgs e)
		{
			_client = new TcpClient();
			IPEndPoint IpEnd = new IPEndPoint(IPAddress.Parse(ClientIP.Text.Trim()), int.Parse(ClientPort.Text.Trim()));

			try
			{
				_client.Connect(IpEnd);
				Messages.Text += ("Connect to Server" + "\n");
				writer = new StreamWriter(_client.GetStream());
				reader = new StreamReader(_client.GetStream());
				writer.AutoFlush = true;
				BackgroundReceiver.RunWorkerAsync();
				BackgroundSender.WorkerSupportsCancellation = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
		}

		private void Local_Click(object sender, EventArgs e)
		{
			ClientIP.Text = "127.0.0.1";
			ClientPort.Text = "7777";
		}

		private void RequestNameChange()
		{
			UserName = nameBox.Text.Trim();
		}

		private void SendMessage()
		{
			if (string.IsNullOrEmpty(MessText.Text)) return;
			if (!_client.Connected) return;

			TcpMessage tcpMessage = new(
					TcpMessageType.PublicMessage,
					MessText.Text
				);
			MessText.Text = string.Empty;

			string formattedTextForServer = JsonSerializer.Serialize(tcpMessage);
			writer.WriteLine(formattedTextForServer);

			Messages.Invoke(new MethodInvoker(delegate ()
			{
				Messages.Text += (tcpMessage + "\n");
			}));
		}

		private void ReceiveMessage()
		{
			while (_client.Connected)
			{
				try
				{
					string? receivedMessage = reader.ReadLine();
					if (receivedMessage == null) throw new Exception("Received message was null");
					TcpMessage tcpMessage = JsonSerializer.Deserialize<TcpMessage>(receivedMessage);

					/*
					//// tohle smazat a predelat aby server poslal klientovi jeho vlastni zpravu zpatky, pro "klientske potvrzeni", ze serveru opravdu zprava prisla

					Messages.Invoke(new MethodInvoker(delegate ()
					{
						Messages.Text += (receivedMessage + "\n");
					}));
					*/

					receivedMessage = string.Empty;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void Receiver_DoWork(object sender, DoWorkEventArgs e) => ReceiveMessage();

		private void SendButton_Click(object sender, EventArgs e) => SendMessage();

		private void ChangeButton_Click(object sender, EventArgs e) => RequestNameChange();
	}
}