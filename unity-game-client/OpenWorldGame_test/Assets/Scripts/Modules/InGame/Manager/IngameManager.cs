using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ingameを司る。
//ゲームデータの保存など。
public class IngameManager 
{
    public Dictionary<uint,Gamemodel.GameClient> GameClientsData = new Dictionary<uint, Gamemodel.GameClient>(); //user_id, gameclient

    public IngameManager()
    {
        this.GameClientsData = new Dictionary<uint, Gamemodel.GameClient>();
    }

    public IngameManager(Gamemodel.GameClient[] gameClientsData)
    {
        this.GameClientsData = new Dictionary<uint, Gamemodel.GameClient>();
        for (int i = 0; i < gameClientsData.Length; i++)
        {
            this.GameClientsData.Add(gameClientsData[i].info.user_id, gameClientsData[i]);
        }
    }

    public Gamemodel.GameClient GetGameClientData(uint user_id)
    {
        Gamemodel.GameClient gameClient;
        if (GameClientsData.ContainsKey(user_id)) gameClient = new Gamemodel.GameClient { };
        //return
        return new Gamemodel.GameClient { };
    }
}
