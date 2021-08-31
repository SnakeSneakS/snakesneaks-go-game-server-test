using System;
using System.Collections;
using System.Collections.Generic;
using NativeWebSocket;
using UnityEngine;

public partial class GameMethodHandler : MonoBehaviour
{
    private const float SendCoroutineIntervalSeconds = 0.1f;

    private List<Gamemodel.GameMethod> storedMethods;
    private Model.Session session;
    private WebSocket webSocket;

    private bool isWorking=false;

    public event EventHandler OnSessionFailed;
    public event EventHandler OnGameMethodHandlerFailed;

    public void StartHandler(WebSocket webSocket, Model.Session session)
    {
        if (isWorking == true)
        {
            Debug.Log("Already Started GameMethod Handler.");
            return;
        }
        storedMethods = new List<Gamemodel.GameMethod>();
        this.session = session;
        this.webSocket = webSocket;
        this.isWorking = false;

        this.webSocket.OnMessage += (bytes) =>
        {
            OnReceive(System.Text.Encoding.UTF8.GetString(bytes));
        };

        StartCoroutine(SendCoroutine());
    }

    public IEnumerator SendCoroutine()
    {
        if (isWorking == true)
        {
            Debug.Log("GameMethodHandler is already working!");
        }

        isWorking = true;

        while (isWorking)
        {
            Send();
            yield return new WaitForSeconds(SendCoroutineIntervalSeconds );
        }
        Debug.Log("Stopped Working For Some Reason!");
        isWorking = false;
        OnGameMethodHandlerFailed?.Invoke(this, null);
        yield break;
    }

    /// <summary>
    /// Add to send later 
    /// </summary>
    /// <param name="gameMethod"></param>
    public void Add(Gamemodel.GameMethod gameMethod)
    {
        /*
         * By avoid to duplicate, I can reduce amount of data.
         */
        storedMethods.Add(gameMethod);
    }

    private void Send()
    {
        if (storedMethods.Count == 0) return;
        Gamemodel.GameReq gr = new Gamemodel.GameReq { session = this.session, methods = this.storedMethods.ToArray() };
        this.storedMethods.Clear();
        string data = JsonUtility.ToJson(gr);
        Debug.Log("Send Data: " + data);
        if (webSocket.State==WebSocketState.Open)
        {
            this.webSocket.SendText(data);
        }
        else
        {
            isWorking = false;
            Debug.Log("Websocket Connection is not alive");
        }
        
    }

    public void OnReceive(string response)
    {
        Gamemodel.GameRes gameRes = JsonUtility.FromJson<Gamemodel.GameRes>(response);
        if (gameRes.status==Model.ConnStatus.success)
        {
            foreach(Gamemodel.GameResUnit gameResUnit in gameRes.response)
            {
                uint user_id = gameResUnit.user_id;
                Gamemodel.GameMethod[] methods = gameResUnit.methods;
                foreach(Gamemodel.GameMethod method in methods)
                {
                    switch (method.method)
                    {
                        case Gamemodel.GameMethodType.Idle:
                            break;
                        case Gamemodel.GameMethodType.EnterWorld:
                            ReceiveEnterWorld(user_id, method.content);
                            break;
                        case Gamemodel.GameMethodType.ExitWorld:
                            break;
                        case Gamemodel.GameMethodType.Chat:
                            ReceiveChat(user_id,method.content);
                            break;
                        case Gamemodel.GameMethodType.Move:
                            ReceiveMove(user_id, method.content);
                            break;
                        case Gamemodel.GameMethodType.GetIngameClientsData:
                            ReceiveGetIngameClientsData(user_id, method.content);
                            break;
                    }
                }
            }
        }
        else
        {
            OnSessionFailed?.Invoke(this,null);
        }
    }
}
