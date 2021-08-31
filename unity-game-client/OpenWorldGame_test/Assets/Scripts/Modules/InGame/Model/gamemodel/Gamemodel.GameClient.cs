using System;
using UnityEngine;
using TMPro;

//Gamemodel.Gameclient 
public partial class Gamemodel 
{
    [Serializable]
    public enum IngameClientConnectionState
    {
        Ready,
        Active,
        Disconnected
    }

    [Serializable]
    public struct IngameClient
    {
        [SerializeField] public IngameClientConnection conn;
        [SerializeField] public IngameClientInfo info;
        [SerializeField] public IngameClientIngameInfo ingame_info;
    }

    [Serializable]
    public struct IngameClientConnection
    {
        [SerializeField] public IngameClientConnectionState conn_state;
    }

    [Serializable]
    public struct IngameClientInfo
    {
        [SerializeField] public uint user_id;
        [SerializeField] public string username;
    }

    [Serializable]
    public struct IngameClientIngameInfo
    {
        [SerializeField] public GameTransform transform;
    }

    [Serializable]
    public struct IngameClientInstance
    {
        [SerializeField] public GameObject playerPrefab; //動くプレイヤー
        [SerializeField] public PlayerController playerController; //プレイヤーを動かすコントローラ
    }
}
