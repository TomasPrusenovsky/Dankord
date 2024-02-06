using DankordServerApp;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Windows.Forms;

namespace DankordClientApp
{
	public partial class ClientForm : Form
	{
		public const string WindowHeader = "Client";

		private TcpClient tcpServer;

		public StreamReader TcpReader => new(tcpServer.GetStream());
		public StreamWriter TcpWriter => new(tcpServer.GetStream());

		private string? UserName
		{
			get => nameBox.Text.Trim();
			set => nameBox.Text = value ?? "";
		}

		private string? ServerIP
		{
			get => ClientIP.Text.Trim();
			set => ClientIP.Text = value ?? "";
		}

		private string? ServerPort
		{
			get => ClientPort.Text.Trim();
			set => ClientPort.Text = value ?? "";
		}

		public ClientForm()
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
			tcpServer = new TcpClient();
			TcpWriter.AutoFlush = true;
			try
			{
				IPEndPoint IpEnd = new IPEndPoint(IPAddress.Parse(ServerIP), int.Parse(ServerPort));
				tcpServer.Connect(IpEnd);

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
				tcpServer.Close();
			}
			catch (Exception ex)
			{
				LogMessage(WindowHeader, ex.Message);
			}
		}

		private void RequestNameChange()
		{
			try
			{
				if (string.IsNullOrEmpty(MessText.Text)) return;
				if (!tcpServer.Connected) return;

				string tcpMessage = TcpMessage.TcpMessageToString(new(
						TcpMessageType.NameChangeRequest,
						UserName,
						UserName
					));

				TcpWriter.WriteLine(tcpMessage);
			}
			catch (Exception ex)
			{
				LogMessage(WindowHeader, ex.Message);
			}

		}

		private void SendMessage()
		{
			try
			{
				if (string.IsNullOrEmpty(MessText.Text)) return;
				if (!tcpServer.Connected) return;

				string tcpMessage = TcpMessage.TcpMessageToString(new(
						TcpMessageType.PublicMessage,
						UserName,
						MessText.Text.Trim()
					));

				MessText.Text = string.Empty;

				TcpWriter.WriteLine(tcpMessage);
			}
			catch (Exception ex)
			{
				LogMessage(WindowHeader, ex.Message);
			}

		}

		private void HandleConnection()
		{
			while (tcpServer.Connected)
			{
				try
				{
					if (TcpReader == null) throw new NullReferenceException();
					if (TcpWriter == null) throw new NullReferenceException();

					string? receivedMessage = TcpReader.ReadLine();
					if (receivedMessage == null) return;
					TcpMessage tcpMessage = TcpMessage.StringToTcpMessage(receivedMessage);

					switch (tcpMessage.Type)
					{
						case TcpMessageType.PublicMessage:
							LogMessage(tcpMessage.Sender, tcpMessage.Message);
							return;

						case TcpMessageType.NameChangeRequest:
							UserName = tcpMessage.Message;
							LogMessage(tcpMessage.Sender, $"*Changed your username to {UserName}!*");
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
			DateTime t = DateTime.Now;
			Messages.Text += $"{(includeTimeStamp ? $"[{t.Hour}:{t.Minute}:{t.Second}.{t.Millisecond}] " : "")}{sender}: {message}\n";
		}));

		private void Receiver_DoWork(object sender, DoWorkEventArgs e) => HandleConnection();

		private void SendButton_Click(object sender, EventArgs e) => SendMessage();

		private void ChangeButton_Click(object sender, EventArgs e) => RequestNameChange();

		private void ConnectClient_Click(object sender, EventArgs e) => Connect();

		private void vitekButton_Click(object sender, EventArgs e)
		{
			ServerIP = "185.82.239.12";
			ServerPort = "25565";
		}
	}
}