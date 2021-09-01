using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatDisplayTextUnitController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChatNameText;
    [SerializeField] TextMeshProUGUI ChatDivideText;
    [SerializeField] TextMeshProUGUI ChatContentText;

    public GameObject Create()
    {
        return GameObject.Instantiate(this.gameObject);
    }

    public GameObject Create(string name, string content)
    {
        Set(name, content);
        GameObject obj= Create();
        return obj;
    }

    public void Set(string name, string content)
    {
        ChatNameText.text = name;
        ChatContentText.text = content;
    }

    public void Set(string name, string content,Color color)
    {
        ChatNameText.text = $"<color={color}>{name}</color>";
        ChatDivideText.text = $"<color={color}></color>";
        ChatContentText.text = $"<color={color}>{content}</color>";
    }

    public void Delete()
    {
        GameObject.Destroy(this.gameObject);
    }
}
