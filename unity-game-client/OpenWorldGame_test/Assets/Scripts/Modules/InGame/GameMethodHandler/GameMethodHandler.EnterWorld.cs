using UnityEngine;

//GameMethod.EnterWorld 
public partial class GameMethodHandler 
{
    public delegate void EnterWorldEventHandler<T>(T args);
    public event EnterWorldEventHandler<EnterWorldEventArgs> OnEnterWorldReceive;

    public struct EnterWorldEventArgs
    {
        public uint user_id;
        public Gamemodel.EnterWorldMethod enterWorldMethod;
    }

    /// <summary>
    /// Send EnterWorld Message To Server 
    /// </summary>
    public void SendEnterWorld()
    {
        Gamemodel.EnterWorldMethod enterWorldMethod = new Gamemodel.EnterWorldMethod {
            ingame_client_data = new Gamemodel.IngameClient() {
                info=new Gamemodel.IngameClientInfo {
                    user_id=ClientManager.Session.UserID,
                    username=ClientManager.Username
                }
            },
        };

        string content = JsonUtility.ToJson(enterWorldMethod);
        Gamemodel.GameMethod gameMethod = new Gamemodel.GameMethod { method = Gamemodel.GameMethodType.EnterWorld, content = content };
        Add(gameMethod);
    }

    /// <summary>
    /// Recieve EnterWork Message From Server: add client data in local 
    /// </summary>
    /// <param name="client"></param>
    public void ReceiveEnterWorld(uint user_id, string json)
    {
        Gamemodel.EnterWorldMethod enterWorldMethod = JsonUtility.FromJson<Gamemodel.EnterWorldMethod>(json);
        Debug.Log($"ReceivedEnterWorld: \nuser_id: {user_id}, username: {enterWorldMethod.ingame_client_data.info.username}");
        OnEnterWorldReceive?.Invoke(new EnterWorldEventArgs { user_id = user_id, enterWorldMethod=enterWorldMethod, });
    }
}
