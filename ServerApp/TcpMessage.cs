using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServerApp
{
	[Serializable]
	public struct TcpMessage(TcpMessageType type, string sender, string message)
	{
		public TcpMessageType Type { get; set; } = type;
		public string Sender { get; set; } = sender;
		public string Message { get; set; } = message;

		public static string TcpMessageToString(TcpMessage message) => JsonSerializer.Serialize(message);

		public static TcpMessage StringToTcpMessage(string message) => JsonSerializer.Deserialize<TcpMessage>(message);
	}

	public enum TcpMessageType
	{
		PublicMessage,
		PrivateMessage,
		NameChangeRequest
	}
}