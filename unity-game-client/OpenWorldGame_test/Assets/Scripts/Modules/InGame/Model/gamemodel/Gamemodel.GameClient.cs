using System;
using UnityEngine;

//Gamemodel.Gameclient 
public partial class Gamemodel 
{
    [Serializable]
    public enum ClientConnectionState
    {
        Ready,
        Active,
        Disconnected
    }

    [Serializable]
    public struct GameClient
    {
        [SerializeField] public GameClientConnection conn;
        [SerializeField] public GameClientInfo info;
        [SerializeField] public GameClientIngameInfo ingame_info;
    }

    [Serializable]
    public struct GameClientConnection
    {
        [SerializeField] public ClientConnectionState conn_state;
    }

    [Serializable]
    public struct GameClientInfo
    {
        [SerializeField] public uint user_id;
        [SerializeField] public string username;
    }

    [Serializable]
    public struct GameClientIngameInfo
    {
        [SerializeField] public GameTransform transform;
    }
    
}
