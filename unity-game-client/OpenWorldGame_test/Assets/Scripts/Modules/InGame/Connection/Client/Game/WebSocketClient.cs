using System;
using UnityEngine;
using WebSocketSharp; 

//Client: WebSocketClient 
public abstract class WebSocketClient: MonoBehaviour  
{
    [SerializeField] protected string hostname="localhost";
    [SerializeField] protected string port="8000";
    [SerializeField] protected string path="/";

    protected virtual EventHandler<MessageEventArgs> ReceivedEventHandler { get; set; } 
    protected virtual EventHandler<CloseEventArgs> ClosedEventHandler { get; set; } 
    protected virtual EventHandler<ErrorEventArgs> ErrorEventHandler { get; set; } 

    public WebSocket ws;

    public WebSocketClient()
    {
    }

    public WebSocketClient(string hostname,string port,string path)
    {
        this.hostname = hostname;
        this.port = port;
        this.path = path;
    }

    public void Connect()
    {
        if (this.ws == null)
        {
            this.ws = new WebSocket($"ws://{hostname}:{port}{path}");
            //when received
            ws.OnMessage += (sender, e) => {
                Debug.Log($"Received Data: \n{e.Data}");
            };
            ws.OnMessage += ReceivedEventHandler;

            //when closed
            ws.OnClose += (sender, e) =>
            {
                Debug.Log($"WebSocket Connection Closed.\n{e}");
            };
            ws.OnClose += ClosedEventHandler;

            //when error
            ws.OnError += (sender, e) =>
            {
                Debug.LogError($"WebSocket Connection Error.\n{e}");
            };
            ws.OnError += ErrorEventHandler;
        }
        ws.Connect();
    }
    
}
