
using UnityEngine;
using UnityEngine.UI;

public class WebSocketTestScene : MonoBehaviour
{
    [Header("WebSocket StateDesplay")]
    [SerializeField] Text WebSocketStateText;

    [Header("WebSocket SendForm")]
    [SerializeField] InputField SendText;
    [SerializeField] Button SendButton;

    [Header("WebSocket Connect")]
    [SerializeField] Button ConnectButton;

    [Header("WebSocket ReceivedContent")]
    [SerializeField] Text ReceivedText;

    private WebSocketClient m_wsclient; 


    void Start()
    {
        this.m_wsclient = new WebSocketClient();

        //Connect
        ConnectButton.onClick.AddListener(() =>
        {
            m_wsclient.ws.Connect();
        });

        //Send
        SendButton.onClick.AddListener(() =>
        {
            if (!m_wsclient.ws.IsAlive)
            {
                Debug.LogAssertion("WebSocket Connection Not Alive");
                return;
            }
            m_wsclient.ws.Send(SendText.text);
        });
    }

    private void FixedUpdate()
    {
        WebSocketStateText.text = $"WebSocketState: \nurl: {m_wsclient.ws.Url}, alive: {m_wsclient.ws.IsAlive}";
    }
}
