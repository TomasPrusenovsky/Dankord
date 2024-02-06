using System.Text.Json;

namespace DankordServerApp
{
	[Serializable]
	public readonly struct TcpMessage(TcpMessageType type, string sender, string message)
	{
		public readonly TcpMessageType Type = type;
		public readonly string Sender = sender;
		public readonly string Message = message;

		public readonly bool IsValid()
		{
			if (Enum.IsDefined(typeof(TcpMessageType), Type))
				return false;
			if (string.IsNullOrWhiteSpace(Sender))
				return false;
			if (string.IsNullOrWhiteSpace(Message))
				return false;
			return true;
		}

		public static string TcpMessageToString(TcpMessage message) => JsonSerializer.Serialize(message);

		public static TcpMessage StringToTcpMessage(string message) => JsonSerializer.Deserialize<TcpMessage>(message);
	}

	public enum TcpMessageType
	{
		Invalid = -1,
		PublicMessage,
		PrivateMessage,
		NameChangeRequest,
		ConsoleCommand
	}
}