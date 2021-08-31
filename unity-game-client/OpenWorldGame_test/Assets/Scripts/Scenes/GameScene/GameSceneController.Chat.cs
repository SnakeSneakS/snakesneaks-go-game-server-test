using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//GameSceneController.Chat
public partial class GameSceneController : MonoBehaviour
{
    [Header("Chat")]
    [SerializeField] InputField chatInputField;
    [SerializeField] ScrollRect chatOutputVerticalScrollRect;
    [SerializeField] TextMeshProUGUI chatDisplayText;
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
    {
        bool isChatScrollBottom = (this.chatOutputVerticalScrollRect.verticalNormalizedPosition<=0);

        string name = user_id.ToString();
        try { name = this.ingameManager.IngameClientsData[user_id].info.username; }
        catch { Debug.LogError($"Failed to find gameClientData[{user_id}]!"); }
        string new_text = this.chatDisplayText.text + SanitizeRichText($"{name}:\u00A0{chatMethod.text}\n");
        chatTextUnitNum++;
        if (chatTextUnitNum > chatTextUnitMax)
        {
            new_text = new_text.Substring(this.chatDisplayText.text.IndexOf("\n") +1 );
            chatTextUnitNum--;
        }
        //dispatcher.Invoke(() => {  });
        this.chatDisplayText.text = new_text;

        Canvas.ForceUpdateCanvases();
         this.chatOutputVerticalScrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }

    private string SanitizeRichText(string text)
    {
        string new_text = text;
        if (new_text.Length > chatTextLengthMaxPerUnit) new_text = new_text.Substring(0, chatTextLengthMaxPerUnit);
        new_text.Replace(" ", "\u00A0"); //改行しないスペース
        new_text = "<noparse>"+new_text+"</noparse>";
        return new_text;
    }
}
