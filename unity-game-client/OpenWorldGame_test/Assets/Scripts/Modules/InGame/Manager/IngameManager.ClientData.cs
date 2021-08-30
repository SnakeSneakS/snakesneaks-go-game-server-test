using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//IngameManager.ClientData
public partial class IngameManager : MonoBehaviour
{
    public Dictionary<uint, Gamemodel.IngameClient> IngameClientsData=new Dictionary<uint, Gamemodel.IngameClient>(); //user_id, gameclient

    public Gamemodel.IngameClient GetIngameClientData(uint user_id)
    {
        Gamemodel.IngameClient gameClient;
        if (IngameClientsData.ContainsKey(user_id)) gameClient = new Gamemodel.IngameClient { };
        //return
        return new Gamemodel.IngameClient { };
    }

    private void NewIngameClientData(uint user_id, Gamemodel.IngameClient ingameClientData)
    {
        if (this.IngameClientsData.ContainsKey(user_id))
        {
            Debug.LogError("このユーザは既に存在しています。");
        }
        else
        {
            try
            {
                this.IngameClientsData.Add(user_id, ingameClientData);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
    }
}
