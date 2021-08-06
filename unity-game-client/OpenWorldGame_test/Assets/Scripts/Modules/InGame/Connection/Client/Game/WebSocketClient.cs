using System;
using UnityEngine;
using WebSocketSharp; 

//Client: WebSocketClient 
public class WebSocketClient 
{
    public WebSocket ws;

    public WebSocketClient()
    {
        this.ws = new WebSocket($"ws://{Model.gameUri}");
        ws.Connect();

        //when received 
        ws.OnMessage += (sender, e) => {
            Debug.Log($"Received Data: \n{e.Data}");
        };

        //when closed
        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket Connection Closed.");
        };

        //when error
        ws.OnError += (sender, e) =>
        {
            Debug.LogError("WebSocket Connection Error.");
        };
    }

    public WebSocketClient(EventHandler<MessageEventArgs> ReceivedEventHandler, EventHandler<CloseEventArgs> ClosedEventHandler, EventHandler<ErrorEventArgs> ErrorEventHandler)
    {
        this.ws = new WebSocket($"ws://{Model.gameUri}");
        ws.Connect();

        //when received 
        ws.OnMessage += ReceivedEventHandler;

        //when closed
        ws.OnClose += ClosedEventHandler;

        //when error
        ws.OnError += ErrorEventHandler;
    }

}
