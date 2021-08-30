using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GameSceneController.GetIngameClientsInfo
public partial class GameSceneController : MonoBehaviour
{
    public void OnReceivedGetIngameClientsInfo(uint user_id, Gamemodel.GetIngameClientsData clientsData)
    {
        if (user_id != ClientManager.Session.UserID)
        {
            Debug.LogError($"this GetIngameClientsInfo' user-id {user_id} is not equal with {ClientManager.Session.UserID}");
        }

        for (int i = 0; i < clientsData.clients.Length; i++)
        {
            if (clientsData.clients[i].info.user_id != ClientManager.Session.UserID)
            {
                Debug.Log($"NEW USER: {clientsData.clients[i].info.user_id}");
                try
                {
                    dispatcher.Invoke(() =>
                    {
                        this.ingameManager.NewIngameClient(clientsData.clients[i].info.user_id, clientsData.clients[i]);
                    });
                }
                catch(System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        
    }
}
