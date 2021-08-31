
using UnityEngine;
using UnityEngine.UI;
using NativeWebSocket;

//WebSocketTestScene.Controller 
[RequireComponent(typeof(GameWebSocketClient))]
public class WebSocketTestScene : MonoBehaviour
{
    [Header("WebSocket Client")]
    [SerializeField] GameWebSocketClient m_wsclient;

    [Header("WebSocket StateDesplay")]
    [SerializeField] Text WebSocketStateText;

    [Header("WebSocket SendForm")]
    [SerializeField] InputField SendText;
    [SerializeField] Button SendButton;

    [Header("WebSocket Connect")]
    [SerializeField] Button ConnectButton;

    [Header("WebSocket ReceivedContent")]
    [SerializeField] Text ReceivedText;

    [SerializeField] private GameMethodHandler gameMethodHandler;

    void Start()
    {
        Connect();

        //Connect
        ConnectButton.onClick.AddListener(() =>
        {
            Connect();
        });

        //Send
        SendButton.onClick.AddListener(() =>
        {
            if (!(m_wsclient.ws.State==WebSocketState.Open))
            {
                Debug.LogAssertion("WebSocket Connection Not Alive");
                return;
            }
            this.gameMethodHandler.SendChat(SendText.text);
            //m_wsclient.ws.Send(SendText.text);
            Debug.Log($"Send: {SendText.text}");
        });
    }

    private void Connect()
    {
        this.m_wsclient.Connect();
        if (!(this.m_wsclient.ws.State == WebSocketState.Open))
        {
            this.gameMethodHandler.StartHandler(m_wsclient.ws, new Model.Session { });
        }
    }

    private void FixedUpdate()
    {
        WebSocketStateText.text = $"WebSocketState: \nurl: {m_wsclient.uri}, alive: {m_wsclient.ws.State==WebSocketState.Open}";
    }
}
