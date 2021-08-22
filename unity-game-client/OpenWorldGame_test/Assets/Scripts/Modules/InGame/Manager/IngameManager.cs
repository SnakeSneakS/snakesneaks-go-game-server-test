using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ingameを司る。
//ゲームデータの保存など。
public class IngameManager 
{
    public Dictionary<uint,Gamemodel.IngameClient> GameClientsData = new Dictionary<uint, Gamemodel.IngameClient>(); //user_id, gameclient

    public IngameManager()
    {
        this.GameClientsData = new Dictionary<uint, Gamemodel.IngameClient>();
    }

    public IngameManager(Gamemodel.IngameClient[] gameClientsData)
    {
        this.GameClientsData = new Dictionary<uint, Gamemodel.IngameClient>();
        for (int i = 0; i < gameClientsData.Length; i++)
        {
            this.GameClientsData.Add(gameClientsData[i].info.user_id, gameClientsData[i]);
        }
    }

    public Gamemodel.IngameClient GetGameClientData(uint user_id)
    {
        Gamemodel.IngameClient gameClient;
        if (GameClientsData.ContainsKey(user_id)) gameClient = new Gamemodel.IngameClient { };
        //return
        return new Gamemodel.IngameClient { };
    }
}
