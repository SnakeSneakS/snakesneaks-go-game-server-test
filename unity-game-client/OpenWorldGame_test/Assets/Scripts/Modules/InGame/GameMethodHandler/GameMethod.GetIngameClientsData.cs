using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GameMethodHandler.GetIngameClientsData
public partial class GameMethodHandler
{
    public delegate void GetIngameClientsDataEventHandler<T>(T args);
    public event GetIngameClientsDataEventHandler<GetIngameClientsDataEventArgs> OnGetIngameClientsDataReceive;

    public struct GetIngameClientsDataEventArgs
    {
        public uint user_id;
        public Gamemodel.GetIngameClientsData getIngameClientsData;
    }

    /// <summary>
    /// Recieve GetIngameClientsDataMethod Message From Server: add existing clients information.  
    /// </summary>
    public void ReceiveGetIngameClientsData(uint user_id, string json)
    {
        try
        {
            Gamemodel.GetIngameClientsData getIngameClientsData= getIngameClientsData = JsonUtility.FromJson<Gamemodel.GetIngameClientsData>(json);     
            Debug.Log($"ReceivedEnterWorld: \nuser_id: {user_id}");
            OnGetIngameClientsDataReceive?.Invoke(new GetIngameClientsDataEventArgs { user_id = user_id, getIngameClientsData = getIngameClientsData });
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
    }
}
