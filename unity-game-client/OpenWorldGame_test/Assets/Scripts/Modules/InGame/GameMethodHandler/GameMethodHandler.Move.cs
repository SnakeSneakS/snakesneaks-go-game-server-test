using UnityEngine;

//GameMethod.Move
public partial class GameMethodHandler 
{
    public delegate void MoveEventHandler<T>(T args);
    public event MoveEventHandler<MoveEventArgs> OnMoveReceive;

    public struct MoveEventArgs
    {
        public uint user_id;
        public Gamemodel.MoveMethod moveMethod;
    }


    /// <summary>
    /// Send Move Message To Server 
    /// </summary>
    /// <param name="transform">move object's transform property </param>
    public void SendMove(Transform transform)
    {
        Gamemodel.MoveMethod moveMethod = new Gamemodel.MoveMethod { to=new Gamemodel.GameTransform(transform) };
        string content = JsonUtility.ToJson(moveMethod);
        Gamemodel.GameMethod gameMethod = new Gamemodel.GameMethod { method = Gamemodel.GameMethodType.Chat, content = content };
        Add(gameMethod);
    }

    /// <summary>
    /// Handle Received Move Message 
    /// </summary>
    /// <param name="json"></param>
    public void ReceiveMove(uint user_id, string json)
    {
        Gamemodel.MoveMethod moveMethod = JsonUtility.FromJson<Gamemodel.MoveMethod>(json);
        Debug.Log($"ReceivedMove: \nuser_id: {user_id}, move-position: {moveMethod.to.position}, move-rotation: {moveMethod.to.rotation}");
        OnMoveReceive?.Invoke(new MoveEventArgs { user_id=user_id, moveMethod=moveMethod });
    }
}
