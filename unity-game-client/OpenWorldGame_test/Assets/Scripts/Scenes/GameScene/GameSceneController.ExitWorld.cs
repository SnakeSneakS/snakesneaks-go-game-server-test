using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GameSceneController.ExitWorld
public partial class GameSceneController : MonoBehaviour
{
    public void OnReceiveExitWorld(uint user_id)
    {
        DeleteOldIngameClient(user_id);
    }

    private void DeleteOldIngameClient(uint user_id)
    {
        try
        {
            this.ingameManager.DeleteIngameClient(user_id);
        }catch(Exception e){
            Debug.LogError(e);
        }
    }
}
