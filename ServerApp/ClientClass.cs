using System.Net;
using System.Net.Sockets;

namespace DankordServerApp
{
	[Serializable]
	internal class Client
	{
		private static uint __Id = 0;

		private string WindowHeader
		{ get { return $"ClientThread[{id}]"; } }

		public readonly uint id = Interlocked.Increment(ref __Id); // thread-safe id management

		private readonly Thread ClientThread;

		public Server? server;

		private TcpClient? tcpClient;

		public bool IsServerAdministrator;

		private string? userName;
		//private Image icon; // nebo tak neco kamo

		public Client(TcpClient tcpClient, Server server)
		{
			ClientThread = new Thread(() => HandleClient(tcpClient, server))
			{
				Name = WindowHeader
			};
			ClientThread.Start();
		}

		private async Task HandleClient(TcpClient tcpClient, Server server)
		{
			userName = "danek";
			this.server = server;
			this.tcpClient = tcpClient;

			string? endPointIP = (tcpClient.Client.RemoteEndPoint as IPEndPoint)?.Address.ToString();

			if (endPointIP != null)
				if (endPointIP.StartsWith("192.168."))
					IsServerAdministrator = true;

			if (!tcpClient.Connected)
			{
				server.LogDebug(WindowHeader, "Tried to handle disconnected client...");
				return;
			}

			while (tcpClient.Connected)
			{
				try
				{
					server.LogDebug(WindowHeader, "Awaiting message from client...");
					TcpMessage tcpMessage = await AwaitMessage();
					server.LogDebug(WindowHeader, "message....");

					if (!tcpMessage.IsValid())
					{
						server.LogDebug(WindowHeader, $"Received invalid message from client '{userName}', disconnecting.");
						Dispose();
						continue;
					}

					switch (tcpMessage.Type)
					{
						case TcpMessageType.PublicMessage:
							server.LogMessage(userName, tcpMessage.Message);
							server.BroadcastMessage(tcpMessage);
							break;

						case TcpMessageType.NameChangeRequest:
							server.LogDebug(userName, $"{userName} Changed their username to {tcpMessage.Message}!");
							userName = tcpMessage.Message;
							break;

						case TcpMessageType.ConsoleCommand:
							//handle console commands
							server.LogDebug(userName, tcpMessage.Message);
							break;

						default:
							server.LogDebug(WindowHeader, $"Client sent back a TcpMessage with the {tcpMessage.Type} type, which should not be possible");
							break;
					}
				}
				catch (Exception ex)
				{
					server.LogError(WindowHeader, ex.Message);
				}
			}
			server.LogDebug(WindowHeader, "Client disconnected.");
			//finally dispose of the client
			this.Dispose();
		}

		public async Task<TcpMessage> AwaitMessage()
		{
			StreamReader TcpReader = new(tcpClient.GetStream());

			SendMessage(new TcpMessage(TcpMessageType.PublicMessage, "Server", "I see you!"));
			server.LogDebug(WindowHeader, "Sent acknowledgement message to client");
			string? message = await TcpReader.ReadLineAsync();
			//string? message = await new StreamReader(tcpClient.GetStream()).ReadLineAsync();

			if (message == null)
				return new TcpMessage();

			TcpMessage tcpMessage = TcpMessage.StringToTcpMessage(message);

			return tcpMessage;
		}

		public void SendMessage(TcpMessage message)
		{
			StreamWriter TcpWriter = new(tcpClient.GetStream());
			TcpWriter.WriteLine(TcpMessage.TcpMessageToString(message));
		}

		private void Dispose()
		{
			tcpClient.Close();
			tcpClient.Dispose();
			server.clientPool.Remove(this);
		}

		private void LogMessage(string sender, string message, bool includeTimeStamp = true) => server.LogMessage(sender, message, includeTimeStamp);
	}
}