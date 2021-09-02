using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//GameSceneController.Base 
[RequireComponent(typeof(GameMethodHandler))]
public partial class GameSceneController : MonoBehaviour
{
    [Header("Module")]
    GameWebSocketClient gameWebSocketClient;
    [SerializeField] GameMethodHandler gameMethodHandler;
    [SerializeField] IngameManager ingameManager;

    const float ConnectionCheckInterval = 5.0f;

    private void Awake()
    {
        if (EnvManager.Read("USE_TLS") == "True")
        {
            this.gameWebSocketClient = new GameWebSocketClient(WebSocketClient.Protocol.wss, EnvManager.Read("HOST_NAME"), EnvManager.Read("GO_GAME_SERVER_PORT_TLS"), "/api/game/websocket");
        }
        else
        {
            this.gameWebSocketClient = new GameWebSocketClient(WebSocketClient.Protocol.ws, EnvManager.Read("HOST_NAME"), EnvManager.Read("GO_GAME_SERVER_PORT"), "/api/game/websocket");
        }
        this.gameWebSocketClient.Connect();
        this.gameMethodHandler.StartHandler(this.gameWebSocketClient.ws, ClientManager.Session);
    }

    private void Start()
    {
        SetUpUiInteractionEvent();
        SetUpConnectionEvent();
        //StartDispatcher();

        //GameMethod 
        StartMoveInterval();

        //Send Enterworld Message to Server 
        this.gameMethodHandler.SendEnterWorld();
    }

    private void SetUpConnectionEvent()
    {
        //when connection failed
        this.gameWebSocketClient.ws.OnClose += (e) =>
        {
            Debug.Log("Connection Failed!");
            OnConnectionOut();
        };
        //when connection failed
        this.gameWebSocketClient.ws.OnError += (e) =>
        {
            Debug.Log("Connection Error!");
            OnConnectionOut();
        };
        //when GameMethodHandler Failed
        this.gameMethodHandler.OnGameMethodHandlerFailed += (sender, e) =>
        {
            Debug.LogError("Stopped");
            OnConnectionOut();
        };
        //receive chat 
        this.gameMethodHandler.OnChatReceive += (e) =>
        {
            Debug.Log($"Ingame Chat Received: \nuser_id: {e.user_id}, text: {e.chatMethod.text}");
            OnReceiveChat(e.user_id, e.chatMethod);
        };
        //receive enterworld 
        this.gameMethodHandler.OnEnterWorldReceive += (e) =>
        {
            Debug.Log($"Ingame EnterWorld Received: \nuser_id: {e.user_id}, username: {e.enterWorldMethod.ingame_client_data.info.username}");
            OnReceiveEnterWorld(e.user_id, e.enterWorldMethod);
        };
        //receive getIngameClientsData
        this.gameMethodHandler.OnGetIngameClientsDataReceive += (e) =>
        {
            Debug.Log($"Ingame GetIngameClientsInfo Received: \nuser_id: {e.user_id}");
            OnReceivedGetIngameClientsInfo(e.user_id, e.getIngameClientsData);
        };
        //receive move
        this.gameMethodHandler.OnMoveReceive += (e) =>
        {
            Debug.Log($"Ingame Move Received: \nuser_id: {e.user_id}");
            OnReceiveMove(e.user_id, e.moveMethod);
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
        //UpdateDispatcher();
        this.gameWebSocketClient.OnUpdate();
    }

    private void SetUpUiInteractionEvent()
    {
        //SendChat  
        /*chatSendButton.onClick.AddListener(() =>
        {
            SendChat();
        });*/
        chatInputField.onEndEdit.AddListener((string text) => { SendChat(); });
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
