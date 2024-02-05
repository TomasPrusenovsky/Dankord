using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

[Serializable]
public struct TcpMessage
{
	public TcpMessageType Type { get; set; }
	public string Message { get; set; }

	public TcpMessage(TcpMessageType type, string message)
	{
		Type = type;
		Message = message;
	}
}

public enum TcpMessageType
{
	PublicMessage,
	PrivateMessage,
	NameChangeRequest
}