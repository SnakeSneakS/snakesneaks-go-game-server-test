using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ingameを司る。
//ゲームデータの保存など。
public partial class IngameManager: MonoBehaviour 
{
    /*
    public IngameManager()
    {
        this.IngameClientsData = new Dictionary<uint, Gamemodel.IngameClient>();
        this.IngameClientsInstances = new Dictionary<uint, Gamemodel.IngameClientInstance>();
    }
    */


    public void NewIngameClient(uint user_id, Gamemodel.IngameClient ingameClientData)
    {
        NewIngameClientInstance(user_id,ingameClientData);
        NewIngameClientData(user_id,ingameClientData);
    }

    public void DeleteIngameClient(uint user_id)
    {
        DeleteIngameClientInstance(user_id);
        DeleteIngameClientData(user_id);
    }
}
