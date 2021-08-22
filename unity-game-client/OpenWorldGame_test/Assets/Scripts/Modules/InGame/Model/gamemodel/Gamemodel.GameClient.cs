using System;
using UnityEngine;

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
        [SerializeField] public GameClientConnection conn;
        [SerializeField] public GameClientInfo info;
        [SerializeField] public GameClientIngameInfo ingame_info;
    }

    [Serializable]
    public struct GameClientConnection
    {
        [SerializeField] public IngameClientConnectionState conn_state;
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
