using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//IngameManager.Instances 
public partial class IngameManager : MonoBehaviour
{
    [SerializeField] private Gamemodel.IngameClientInstance ingameClientInstance;

    Dictionary<uint, Gamemodel.IngameClientInstance> IngameClientsInstances=new Dictionary<uint, Gamemodel.IngameClientInstance>();

    private void NewIngameClientInstance(uint user_id, Gamemodel.IngameClient client)
    {
        if (this.IngameClientsInstances.ContainsKey(user_id))
        {
            
            Debug.LogError("このユーザのインスタンスは既に存在しています。");
            return;
        }
        else
        {
            GameObject player = Instantiate(ingameClientInstance.player);
            PlayerController playerController = player.GetComponent<PlayerController>();
            Gamemodel.IngameClientInstance instance = new Gamemodel.IngameClientInstance() { player = player, playerController=playerController, };
            try
            {
                //オブジェクトの生成などの処理を行う。
                this.IngameClientsInstances.Add(user_id, instance);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }

            instance.playerController.SetUsername(client.info.username);
        }
    }
}
