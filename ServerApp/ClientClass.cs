using System.ComponentModel;
using System.Net.Sockets;
using System.Numerics;
using System.Text.Json;
using System.Windows.Forms;

namespace DankordServerApp
{
	[Serializable]
	internal class Client
	{
		private string WindowHeader { get { return $"{userName} Handler"; } }

		public readonly uint id;

		public readonly Server server;

		private readonly Thread ClientThread;

		private readonly TcpClient tcpClient;

		public StreamReader TcpReader => new(tcpClient.GetStream());
		public StreamWriter TcpWriter => new(tcpClient.GetStream());

		public bool IsConnected => tcpClient.Connected;

		private string userName;
		//private Image icon; // nebo tak neco kamo

		public Client(TcpClient tcpClient, Server server)
		{
			userName = "danek";
			this.server = server;
			this.tcpClient = tcpClient;

			ClientThread = new Thread(HandleClient);
			ClientThread.Start();

			//new Thread(HandleClient).Start();
		}

		public string WaitForMessage()
		{
			string? message = TcpReader.ReadLine();
			if (message == null)
			{
				server.LogError(WindowHeader, "Received message from client, but it is null");
				return string.Empty;
			}
			return message;
			//string? message = await TcpReader.ReadLineAsync();
			//TcpMessage receivedMessage = JsonSerializer.Deserialize<TcpMessage>(message, JsonSerializerOptions.Default);
			//return message;
		}

		public void SendMessage(TcpMessage message)
		{
			TcpWriter.WriteLine(TcpMessage.TcpMessageToString(message));
		}

		private void HandleClient()
		{
			if (!IsConnected) server.LogMessage(WindowHeader, "Tried to handle disconnected client...");

			TcpWriter.AutoFlush = true;

			while (IsConnected)
			{
				try
				{
					server.LogMessage(WindowHeader, "Awaiting message from client...");
					string? message = WaitForMessage();
					if (message == null)
					{
						server.LogMessage(WindowHeader, "Received message was null.");
						continue;
					}

					TcpMessage tcpMessage = TcpMessage.StringToTcpMessage(message);

					switch (tcpMessage.Type)
					{
						case TcpMessageType.PublicMessage:
							server.LogMessage(tcpMessage.Sender, tcpMessage.Message);
							server.BroadcastMessage(tcpMessage);
							break;

						case TcpMessageType.NameChangeRequest:
							server.LogMessage(tcpMessage.Sender, $"{userName} Changed their username to {tcpMessage.Message}!");
							userName = tcpMessage.Message;
							break;

						default:
							server.LogMessage(WindowHeader, $"Server sent back a TcpMessage with the {tcpMessage.Type} type, which should not be possible");
							break;
					}


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
			Dispose();
		}

		private void Dispose()
		{
		}
	}
}