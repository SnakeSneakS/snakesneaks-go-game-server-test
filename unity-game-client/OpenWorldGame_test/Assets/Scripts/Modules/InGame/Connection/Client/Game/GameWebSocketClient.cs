using System;
using UnityEngine;
using WebSocketSharp;

public class GameWebSocketClient : WebSocketClient
{
    protected override EventHandler<MessageEventArgs> ReceivedEventHandler{ get;set; } 
    protected override EventHandler<CloseEventArgs> ClosedEventHandler { get; set; } 
    protected override EventHandler<ErrorEventArgs> ErrorEventHandler { get; set; }

    public GameWebSocketClient()
    {

    }

    public GameWebSocketClient(string hostname, string port, string path) :base(hostname, port, path)
    {

    }
}
