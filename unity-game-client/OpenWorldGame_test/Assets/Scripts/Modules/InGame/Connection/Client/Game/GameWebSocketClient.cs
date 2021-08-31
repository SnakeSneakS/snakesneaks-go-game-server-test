using System;
using UnityEngine;
using NativeWebSocket;

public class GameWebSocketClient : WebSocketClient
{
    protected override WebSocketMessageEventHandler ReceivedEventHandler{ get;set; } 
    protected override WebSocketCloseEventHandler ClosedEventHandler { get; set; } 
    protected override WebSocketErrorEventHandler ErrorEventHandler { get; set; }

    public GameWebSocketClient()
    {

    }

    public GameWebSocketClient(Protocol protocol,string hostname, string port, string path) :base(protocol, hostname, port, path)
    {

    }
}
