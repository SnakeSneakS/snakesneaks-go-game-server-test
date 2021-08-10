using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//GameSceneController.Base 
[RequireComponent(typeof(GameWebSocketClient))]
[RequireComponent(typeof(GameMethodHandler))]
public partial class GameSceneController : MonoBehaviour
{
    [Header("Module")]
    [SerializeField] GameWebSocketClient gameWebSocketClient;
    [SerializeField] GameMethodHandler gameMethodHandler;

    const float ConnectionCheckInterval = 5.0f;

    private void Awake()
    {
        this.gameWebSocketClient.Connect();
        this.gameMethodHandler.StartHandler(this.gameWebSocketClient.ws, ClientManager.Session);
    }

    private void Start()
    {
        SetUpButtonEvent();
        SetUpConnectionEvent();
        StartDispatcher();
    }

    private void SetUpConnectionEvent()
    {
        //when connection failed
        this.gameWebSocketClient.ws.OnClose += (sender, e) =>
        {
            Debug.Log("Connection Failed!");
            OnConnectionOut();
        };
        //when GameMethodHandler Failed
        this.gameMethodHandler.OnGameMethodHandlerFailed += (sender, e) =>
        {
            Debug.LogError("Stopped");
            OnConnectionOut();
        };
        //chat receive
        this.gameMethodHandler.OnChatReceive += (e) =>
        {
            Debug.Log($"Ingame Chat Received: \nuser_id: {e.user_id}, text: {e.text}");
            OnReceiveChat(e.user_id, e.text);
        };
        //Session Failed event
        this.gameMethodHandler.OnSessionFailed += (sender,e) =>
        {
            Debug.LogError("Session Configuration Failed!");
            OnConnectionOut();
        };
    }

    private void Update()
    {
        UpdateDispatcher();
    }

    private void SetUpButtonEvent()
    {
        //SendChat  
        chatSendButton.onClick.AddListener(() =>
        {
            SendChat();
        });
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }
}
