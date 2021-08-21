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
        Idle,
        EnterWorld,
        ExitWorld,
        Chat,
        Move,
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
}
