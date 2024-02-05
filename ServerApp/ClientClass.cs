using System.ComponentModel;
using System.Net.Sockets;
using System.Text.Json;
using System.Windows.Forms;

namespace ServerApp
{
	[Serializable]
	internal class Client
	{
		private string WindowHeader { get { return $"{userName} Handler"; } }

		private string userName;
		//private Image icon; // nebo tak neco kamo

		public bool IsConnected => tcpClient.Connected;

		public Server parentServer;

		private readonly Thread ClientThread;

		private TcpClient tcpClient;
		private StreamReader reader;
		private StreamWriter writer;

		public Client(TcpClient tcpClient, Server server)
		{
			userName = "danek";
			this.parentServer = server;
			this.tcpClient = tcpClient;
			reader = new(this.tcpClient.GetStream());
			writer = new(this.tcpClient.GetStream());
			writer.AutoFlush = true;

			ClientThread = new Thread(() => HandleClient(tcpClient, server));
			ClientThread.Start();
		}

		public async Task<string?> AwaitMessage()
		{
			string? message = await reader.ReadLineAsync();
			if (message == null)
				return string.Empty;
			return message;
			//string? message = await reader.ReadLineAsync();
			//TcpMessage receivedMessage = JsonSerializer.Deserialize<TcpMessage>(message, JsonSerializerOptions.Default);
			//return message;
		}

		public void SendMessage()
		{

		}

		private async void HandleClient(TcpClient tcpClient, Server server)
		{
			while (IsConnected)
			{
				try
				{
					server.LogMessage(WindowHeader, "Awaiting message from client...");
					string? message = await AwaitMessage();
					server.LogMessage(WindowHeader, "Message received!");
					message ??= string.Empty;
					server.LogMessage(userName, message);
				}
				catch (Exception ex)
				{
					server.LogError(WindowHeader, ex.Message);
				}
			}
			server.LogMessage(WindowHeader, "Client disconnected.");
		}
	}
}