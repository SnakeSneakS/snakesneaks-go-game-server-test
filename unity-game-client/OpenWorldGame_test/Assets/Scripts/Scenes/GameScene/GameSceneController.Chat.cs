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

    private const int chatTextLengthMaxPerUnit = 60;

    private int chatTextUnitNum = 0;
    private const int chatTextUnitMax = 50;

    public void SendChat()
    {
        if (chatInputField.text.Length > chatTextLengthMaxPerUnit) chatInputField.text = chatInputField.text.Substring(0, chatTextLengthMaxPerUnit);
        this.gameMethodHandler.SendChat( chatInputField.text );
        chatInputField.text = "";
    }

    public void OnReceiveChat(uint user_id, Gamemodel.ChatMethod chatMethod)
    {;
        string new_text = this.chatDisplayText.text + SerializeText($"{user_id}:\u00A0{chatMethod.text}\n");
        chatTextUnitNum++;
        if (chatTextUnitNum > chatTextUnitMax)
        {
            new_text = new_text.Substring(this.chatDisplayText.text.IndexOf("\n") +1 );
            chatTextUnitNum--;
        }
        dispatcher.Invoke(() => { this.chatDisplayText.text = new_text; });
    }

    private string SerializeText(string text)
    {
        string new_text = text;
        if (new_text.Length > chatTextLengthMaxPerUnit) new_text = new_text.Substring(0, chatTextLengthMaxPerUnit);
        new_text.Replace("<", "＜");
        new_text.Replace(">","＞");
        new_text.Replace("\\","＼");
        new_text.Replace("/", "／");
        new_text.Replace(" ", "\u00A0"); //改行しないスペース 
        return new_text;
    }
}
