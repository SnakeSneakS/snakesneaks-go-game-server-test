using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GameSceneController.EnterWorld 
public partial class GameSceneController : MonoBehaviour
{

    public void OnReceiveEnterWorld(uint user_id, Gamemodel.EnterWorldMethod enterWorldMethod)
    {
        InitializeNewIngameClient(user_id, enterWorldMethod);
    }

    public void InitializeNewIngameClient(uint user_id, Gamemodel.EnterWorldMethod enterWorldMethod)
    {
        Debug.Log($"NEW USER: {user_id}");
        try
        {
            //dispatcher.Invoke(() => { this.ingameManager.NewIngameClient(user_id, enterWorldMethod.ingame_client_data); });
            this.ingameManager.NewIngameClient(user_id, enterWorldMethod.ingame_client_data);
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }
}
