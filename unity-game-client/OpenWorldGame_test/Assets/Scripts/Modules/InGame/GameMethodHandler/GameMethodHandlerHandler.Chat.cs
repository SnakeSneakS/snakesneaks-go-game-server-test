using System;
using UnityEngine; 

//GameMethod.Chat
public partial class GameMethodHandler
{
    public delegate void ChatEventHandler<T>(T args);
    public event ChatEventHandler<ChatEventArgs> OnChatReceive;

    public struct ChatEventArgs
    {
        public uint user_id;
        public Gamemodel.ChatMethod chatMethod;
    }
    
    /// <summary>
    /// Send Chat Message To Server 
    /// </summary>
    /// <param name="text"></param>
    public void SendChat(string text)
    {
        Gamemodel.ChatMethod chatMethod = new Gamemodel.ChatMethod { text=text };
        string content = JsonUtility.ToJson(chatMethod);
        Gamemodel.GameMethod gameMethod = new Gamemodel.GameMethod { method = Gamemodel.GameMethodType.Chat, content = content };
        Add(gameMethod);
    }

    /// <summary>
    /// Handle Received Message 
    /// </summary>
    /// <param name="json"></param>
    public void ReceiveChat(uint user_id,string json)
    {
        Gamemodel.ChatMethod chatMethod = JsonUtility.FromJson<Gamemodel.ChatMethod>(json);
        Debug.Log($"ReceivedChat: \nuser_id: {user_id}, chat-text: {chatMethod.text}");
        OnChatReceive?.Invoke(new ChatEventArgs {user_id=user_id, chatMethod=chatMethod });
    }
    
   
}
