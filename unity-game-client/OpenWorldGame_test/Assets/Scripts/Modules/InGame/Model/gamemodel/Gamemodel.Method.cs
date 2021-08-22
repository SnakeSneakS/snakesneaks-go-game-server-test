using System;
using UnityEngine;

//Gamemodel.GameMethod 
public partial class Gamemodel
{
    [Serializable]
    public struct GameMethod
    {
        [SerializeField] public GameMethodType method;
        [SerializeField] public string content;  //GameMethod: 
    }

    [Serializable]
    public enum GameMethodType
    {
        Idle,                   //idle 
        EnterWorld,             //notify a new user entered world. 
        ExitWorld,              //notify user exit world. 
        Chat,                   //notify chat message
        Move,                   //notify move action
        GetIngameClientsData,   //get all gameClientData info (receive from server)
    }

    //Idle
    [SerializeField]
    public struct IdleMethod
    {

    }

    //EnterWorld
    [SerializeField]
    public struct EnterWorldMethod
    {
        [SerializeField] public Gamemodel.IngameClient ingame_client_data;
    }

    //ExitWorld
    [SerializeField]
    public struct ExitWorldMethod
    {

    }

    //Chat
    [SerializeField]
    public struct ChatMethod
    {
        [SerializeField] public string text;
    }

    //Move
    [Serializable]
    public struct MoveMethod
    {
        [SerializeField] public Gamemodel.GameTransform to;
    }

    //GetData
    [Serializable]
    public struct GetIngameClientsData
    {
        [SerializeField] public IngameClient[] clients;
    }
}
