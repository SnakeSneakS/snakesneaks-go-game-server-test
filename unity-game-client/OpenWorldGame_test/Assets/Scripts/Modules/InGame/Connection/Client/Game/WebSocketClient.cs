using System;
using UnityEngine;
using System.Security.Authentication;
using NativeWebSocket;

//Client: WebSocketClient 
public abstract class WebSocketClient: MonoBehaviour  
{
    [SerializeField] protected Protocol     protocol    = Protocol.ws;
    [SerializeField] protected string       hostname    = "localhost";
    [SerializeField] protected string       port        = "8080";
    [SerializeField] protected string       path        = "/";
    //[SerializeField] protected bool         isCertAll     = false;
    public string uri {
        get {
            return $"{protocol.ToString()}://{hostname}:{port}{path}";
        }
    }

    protected virtual WebSocketMessageEventHandler ReceivedEventHandler { get; set; } 
    protected virtual WebSocketCloseEventHandler ClosedEventHandler { get; set; } 
    protected virtual WebSocketErrorEventHandler ErrorEventHandler { get; set; } 

    public WebSocket ws;


    [Serializable]
    public enum Protocol
    {
        ws,
        wss
    }

    public WebSocketClient()
    {
    }

    public WebSocketClient(Protocol protocol, string hostname,string port,string path)
    {
        this.protocol = protocol;
        this.hostname = hostname;
        this.port = port;
        this.path = path;
    }

    public void Connect()
    {
        if (this.ws == null)
        {
            this.ws = new WebSocket(uri);
            Debug.Log($"uri");

            //when received
            ws.OnMessage += (bytes) => {
                Debug.Log($"Received Data: \n{System.Text.Encoding.UTF8.GetString(bytes)}");
            };
            ws.OnMessage += ReceivedEventHandler;

            //when closed
            ws.OnClose += (e) =>
            {
                Debug.Log($"WebSocket Connection Closed.\n{e}");
            };
            ws.OnClose += ClosedEventHandler;

            //when error
            ws.OnError += (e) =>
            {
                Debug.LogError($"WebSocket Connection Error.\n{e}");
            };
            ws.OnError += ErrorEventHandler;
        }
        ws.Connect();
    }

    public void Send(string text)
    {
        this.ws.SendText(text);
    }
    public void Send(byte[] bytes)
    {
        this.ws.Send(bytes);
    }

    //OnApplicationQuit
    private async void OnApplicationQuit()
    {
        if (this.ws != null)
        {
            await ws.Close();
        }   
    }

    //Update
    public void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (this.ws != null)
        {
            ws.DispatchMessageQueue();
        }
#endif
    }

}
