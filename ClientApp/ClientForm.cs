using DankordServerApp;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace DankordClientApp
{
	public partial class ClientForm : Form
	{
		public const string WindowHeader = "Client";

		private TcpClient? tcpServer;

		private string UserName
		{
			get => nameBox.Text.Trim() ?? "Velky Borec";
			set => nameBox.Text = value.Trim() ?? "Velky Borec";
		}

		private string ServerIP
		{
			get => ClientIP.Text.Trim() ?? "127.0.0.1";
			set => ClientIP.Text = value.Trim() ?? "127.0.0.1";
		}

		private string ServerPort
		{
			get => ClientPort.Text.Trim() ?? "7777";
			set => ClientPort.Text = value.Trim() ?? "7777";
		}

		private string OutputText
		{
			get => Messages.Text;
			set => Messages.Text = value;
		}

		public ClientForm()
		{
			tcpServer = new TcpClient();
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
			BackgroundReceiver.WorkerSupportsCancellation = true;
			BackgroundReceiver.RunWorkerAsync();
		}

		private void HandleConnection()
		{
			IPEndPoint IpEnd = new IPEndPoint(IPAddress.Parse(ServerIP), int.Parse(ServerPort));
			try
			{
				tcpServer = new TcpClient();
				tcpServer.Connect(IpEnd);
				StreamReader TcpReader = new(tcpServer.GetStream());
				LogMessage(WindowHeader, "Connected to server!");
				while (tcpServer.Connected)
				{
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
			}
			catch (Exception ex)
			{
				LogMessage(WindowHeader, ex.Message);
			}

			Disconnect();
		}

		private void Disconnect()
		{
			try
			{
				tcpServer.Close();
				tcpServer.Dispose();
			}
			catch (Exception ex)
			{
				LogMessage(WindowHeader, ex.Message);
			}
			LogMessage(WindowHeader, "Disconnected from server!");
		}

		private void RequestNameChange()
		{
			StreamWriter TcpWriter = new(tcpServer.GetStream());
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
				if (string.IsNullOrEmpty(MessText.Text))
				{
					LogError(WindowHeader, "message was null.");
					return;
				}
				if (!tcpServer.Connected)
				{
					LogError(WindowHeader, "tried to send message to null server.");
					return;
				}

				if (MessText.Text.StartsWith('/'))
					HandleConsoleInput();

				string tcpMessage = TcpMessage.TcpMessageToString(new(
						TcpMessageType.PublicMessage,
						UserName,
						MessText.Text.Trim()
					));

				MessText.Text = string.Empty;

				//TcpWriter.WriteLine(tcpMessage);
				new StreamWriter(tcpServer.GetStream()).WriteLine(tcpMessage);
			}
			catch (Exception ex)
			{
				LogMessage(WindowHeader, ex.Message);
			}
		}

		private void HandleConsoleInput()
		{
			LogMessage(WindowHeader, "command accepted");
			// Handle client-side console commands here
			// if command isn't client-side, pass it to the server via TcpMessageType.ConsoleCommand
		}

		public void LogMessage(string sender, string message, bool includeTimeStamp = true) => Messages.Invoke(new MethodInvoker(delegate
		{
			DateTime t = DateTime.Now;
			OutputText += $"{(includeTimeStamp ? $"[{t.Hour}:{t.Minute}:{t.Second}.{t.Millisecond}] " : "")}{sender}: {message}\n";
		}));

		public void LogDebug(string sender, string message, bool includeTimeStamp = true) => LogMessage(sender, message, includeTimeStamp);

		public void LogError(string sender, string message, bool includeTimeStamp = true) => LogMessage(sender, message, includeTimeStamp);

		private void Receiver_DoWork(object sender, DoWorkEventArgs e) => HandleConnection();

		private void SendButton_Click(object sender, EventArgs e) => SendMessage();

		private void ChangeButton_Click(object sender, EventArgs e) => RequestNameChange();

		private void ConnectClient_Click(object sender, EventArgs e)
		{
			ConnectClient.Enabled = false;
			if (!tcpServer.Connected)
			{
				Connect();
				ConnectClient.Text = "Disconnect";
			}
			else
			{
				Disconnect();
				ConnectClient.Text = "Connect";
			}
			ConnectClient.Enabled = true;
		}

		private void VitekButton_Click(object sender, EventArgs e)
		{
			ServerIP = "185.82.239.12";
			ServerPort = "25565";
		}
	}
}