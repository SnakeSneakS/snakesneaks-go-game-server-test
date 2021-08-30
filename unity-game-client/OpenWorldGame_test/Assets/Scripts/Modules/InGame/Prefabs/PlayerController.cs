using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] bool isMyPlayer = false;
    [SerializeField] TextMeshProUGUI usernameTmp;

    public void SetUsername(string username)
    {
        this.usernameTmp.text = username;
    }

    public void MoveTo()
    {

    }
}
