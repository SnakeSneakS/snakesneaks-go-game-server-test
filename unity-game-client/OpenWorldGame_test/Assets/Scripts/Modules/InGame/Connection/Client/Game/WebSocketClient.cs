using System;
using UnityEngine;
using WebSocketSharp;
using System.Security.Authentication;

//Client: WebSocketClient 
public abstract class WebSocketClient: MonoBehaviour  
{
    [SerializeField] protected Protocol     protocol    = Protocol.ws;
    [SerializeField] protected string       hostname    = "localhost";
    [SerializeField] protected string       port        = "8000";
    [SerializeField] protected string       path        = "/";
    [SerializeField] protected bool         isCertAll     = false;

    protected virtual EventHandler<MessageEventArgs> ReceivedEventHandler { get; set; } 
    protected virtual EventHandler<CloseEventArgs> ClosedEventHandler { get; set; } 
    protected virtual EventHandler<ErrorEventArgs> ErrorEventHandler { get; set; } 

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
            this.ws = new WebSocket($"{protocol.ToString()}://{hostname}:{port}{path}");
            Debug.Log(ws.Url);
            if (this.ws.IsSecure && isCertAll)
            {
                ws.SslConfiguration.ServerCertificateValidationCallback = (sender, cert, chain, err) =>
                {
                    return true;
                };
                ws.SslConfiguration.EnabledSslProtocols = SslProtocols.None | SslProtocols.Ssl2 | SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Default | SslProtocols.Tls11 | SslProtocols.Tls12;
            }

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
