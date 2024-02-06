using ServerApp;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace Chat_App
{
	public partial class DankordWindow : Form
	{
		public const string WindowHeader = "Client";

		private TcpClient _client;

		private string? UserName
		{ get => nameBox.Text.Trim(); set { nameBox.Text = value; } }

		private string? ServerIP
		{
			get => ClientIP.Text.Trim();
			set => ClientIP.Text = value;
		}

		private string? ServerPort
		{
			get => ClientPort.Text.Trim();
			set => ClientPort.Text = value;
		}

		public StreamReader? reader;
		public StreamWriter? writer;

		public DankordWindow()
		{
			InitializeComponent();
			UserName = "Danek";
		}

		private void Local_Click(object sender, EventArgs e)
		{
			ServerIP = "127.0.0.1";
			ServerPort = "7777";
		}

		private void Connect()
		{
			_client = new TcpClient();
			try
			{
				IPEndPoint IpEnd = new IPEndPoint(IPAddress.Parse(ServerIP), int.Parse(ServerPort));
				_client.Connect(IpEnd);

				reader = new StreamReader(_client.GetStream());
				writer = new StreamWriter(_client.GetStream());
				writer.AutoFlush = true;

				Messages.Text += ("Connected to Server!" + "\n");

				BackgroundReceiver.RunWorkerAsync();
				BackgroundSender.WorkerSupportsCancellation = true;
			}
			catch (Exception ex)
			{
				LogMessage(WindowHeader, ex.Message);
			}
		}

		private void Disconnect()
		{
			try
			{
				_client.Close();
			}
			catch (Exception ex)
			{
				LogMessage(WindowHeader, ex.Message);
			}
		}

		private void RequestNameChange()
		{
			if (string.IsNullOrEmpty(MessText.Text)) return;
			if (!_client.Connected) return;

			string tcpMessage = TcpMessage.TcpMessageToString(new(
					TcpMessageType.NameChangeRequest,
					UserName,
					UserName
				));

			writer.WriteLine(tcpMessage);
		}

		private void SendMessage()
		{
			if (string.IsNullOrEmpty(MessText.Text)) return;
			if (!_client.Connected) return;

			string tcpMessage = TcpMessage.TcpMessageToString(new(
					TcpMessageType.PublicMessage,
					UserName,
					MessText.Text.Trim()
				));

			MessText.Text = string.Empty;

			writer.WriteLine(tcpMessage);
		}

		private void ReceiveMessage()
		{
			while (_client.Connected)
			{
				try
				{
					if (reader == null) throw new NullReferenceException();

					string? receivedMessage = reader.ReadLine();
					if (receivedMessage == null) return;
					TcpMessage tcpMessage = TcpMessage.StringToTcpMessage(receivedMessage);

					switch (tcpMessage.Type)
					{
						case TcpMessageType.PublicMessage:
							LogMessage(tcpMessage.Sender, tcpMessage.Message);
							return;

						case TcpMessageType.NameChangeRequest:
							UserName = tcpMessage.Message;
							LogMessage(tcpMessage.Sender, $"*Changed your username to {tcpMessage.Message}!*");
							return;

						default:
							LogMessage(WindowHeader, $"Server sent back a TcpMessage with the {tcpMessage.Type} type, which should not be possible");
							return;
					}
				}
				catch (Exception ex)
				{
					LogMessage(WindowHeader, ex.Message);
				}
			}
		}

		public void LogMessage(string sender, string message, bool includeTimeStamp = true) => Messages.Invoke(new MethodInvoker(delegate
		{
			Messages.Text += $"{(includeTimeStamp ? "[" + DateTime.Now + "] " : "")}{sender}: {message}\n";
		}));

		private void Receiver_DoWork(object sender, DoWorkEventArgs e) => ReceiveMessage();

		private void SendButton_Click(object sender, EventArgs e) => SendMessage();

		private void ChangeButton_Click(object sender, EventArgs e) => RequestNameChange();

		private void ConnectClient_Click(object sender, EventArgs e) => Connect();
	}
}