using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ServerApp
{
	internal class Server
	{
		public const string WindowHeader = "Server";

		private bool IsRunning;

		private readonly TcpListener listener;

		private readonly Thread poolManager;
		private List<Client> clientPool;

		public string messageLog = string.Empty;

		public Server(int port)
		{
			listener = new TcpListener(IPAddress.Any, port);
			clientPool = new();
			IsRunning = false;

			poolManager = new Thread(Start);
			poolManager.Start();
		}

		public async void Start()
		{
			try
			{
				LogMessage(WindowHeader, "Starting Server...");
				listener.Start();
				IsRunning = true;
				LogMessage(WindowHeader, "Server started!");
				while (IsRunning)
				{
					await HandleClientTcpRequest();
				}
			}
			catch (ThreadStartException ex)
			{
				LogError(WindowHeader, ex.Message);
			}
		}

		public void Stop()
		{
			try
			{
				LogMessage(WindowHeader, "Stopping server...");
				listener.Stop();
				IsRunning = false;
				LogMessage(WindowHeader, "Server stopped.");
			}
			catch (ThreadAbortException ex)
			{
				LogError(WindowHeader, ex.Message);
			}
		}

		public async Task HandleClientTcpRequest()
		{
			try
			{
				LogMessage(WindowHeader, "Listening for clients...");
				Client client = new(await listener.AcceptTcpClientAsync(), this);
				clientPool.Add(client);
				LogMessage(WindowHeader, "Added to pool!");
			}
			catch (SocketException ex)
			{
				LogError(WindowHeader, ex.Message);
			}
		}

		public void BroadcastMessage()
		{

		}

		public void LogMessage(string sender, string message)
		{
			messageLog += $"{sender}: {message}\n";
			ServerForm.__instance.Invoke(new MethodInvoker(
				ServerForm.__instance.RefreshOutputWindow
			));
		}

		public void LogError(string sender, string message) => LogMessage(sender, message); // zmenit az zavedeme barevny text bo to by bylo hrozne cool kamo, proste predstav si ze by ten text mohl byt barevny, kazdy uzivatel by si mohl vybrat vlastni barvicku, a chyby by hazely cerveny text, takze by jen nikdo nemohl mit cervenou aby se to nepletlo, ale pak to bude hrozne dobry bo ty errory uvidis hodne jednoduse
	}
}