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
        try
        {
            this.ingameManager.GameClientsData.Add(user_id, enterWorldMethod.ingame_client_data);
            //オブジェクトの生成などの処理を行う。
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            Debug.LogError("このユーザは既に存在しています。");
        }
    }
}
