using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//IngameManager.Instances 
public partial class IngameManager : MonoBehaviour
{
    [Header("Ingame Client Instances")]
    [Tooltip("My Client")]
    [SerializeField] private Gamemodel.IngameClientInstance ingameMyClientInstance;
    [Tooltip("Other Client")]
    [SerializeField] private Gamemodel.IngameClientInstance ingameOtherClientPrefab;

    public Dictionary<uint, Gamemodel.IngameClientInstance> IngameClientsInstances=new Dictionary<uint, Gamemodel.IngameClientInstance>();

    private void NewIngameClientInstance(uint user_id, Gamemodel.IngameClient client)
    {
        if (this.IngameClientsInstances.ContainsKey(user_id))
        {
            Debug.LogError("このユーザのインスタンスは既に存在しています。");
            return;
        }
        else
        {
            Gamemodel.IngameClientInstance instance;
            if (user_id == ClientManager.Session.UserID) //自分のプレイヤーの初期化処理を行う。
            {
                instance = ingameMyClientInstance;
            }
            else //他のプレイヤーの初期化処理を行う
            {
                GameObject player = Instantiate(ingameOtherClientPrefab.playerPrefab);
                PlayerController playerController = player.GetComponent<PlayerController>();
                instance = new Gamemodel.IngameClientInstance() { playerPrefab = player, playerController = playerController, };
            }
            
            
            try
            {
                this.IngameClientsInstances.Add(user_id, instance);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }

            instance.playerController.SetUsername(client.info.username);
        }
    }

    private void DeleteIngameClientInstance(uint user_id)
    {
        if (!this.IngameClientsInstances.ContainsKey(user_id))
        {
            Debug.LogError("このユーザのインスタンスは存在していません。");
            return;
        }
        else
        {
            try
            {
                GameObject.Destroy(this.IngameClientsInstances[user_id].playerPrefab.gameObject);
                this.IngameClientsInstances.Remove(user_id);
            }
            catch(Exception e) {
                Debug.LogError(e);
            }
            
        }
    }
}
