using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//GameSceneController.Chat
public partial class GameSceneController : MonoBehaviour
{
    [Header("Chat")]
    [SerializeField] InputField chatInputField;
    [SerializeField] Text chatDisplayText;
    [SerializeField] Button chatSendButton;

    public void SendChat()
    {
        this.gameMethodHandler.SendChat(chatInputField.text);
        chatInputField.text = "";
    }

    public void OnReceiveChat(string user_id, string text)
    {
        dispatcher.Invoke(() =>
        {
            this.chatDisplayText.text = $"{user_id}: {text}";
        });
    }
}
