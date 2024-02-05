using System.Net.Sockets;

namespace Chat_App
{
	internal class Client
	{
		private string _name = "Danek";
		private StreamReader _STR;
		private StreamWriter _STW;
		private TcpClient _tcpClient;

		public async void Connect(TcpListener listener)
		{
			_tcpClient = await listener.AcceptTcpClientAsync();
			_STR = new StreamReader(_tcpClient.GetStream());
			_STW = new StreamWriter(_tcpClient.GetStream());
			_STW.AutoFlush = true;
		}
	}
}