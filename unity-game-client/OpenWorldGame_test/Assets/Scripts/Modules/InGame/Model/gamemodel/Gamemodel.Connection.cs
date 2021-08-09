using System;
using UnityEngine;

//Gamemodel.Connection 
public partial class Gamemodel
{
    

    [Serializable]
    public struct GameRes
    {
        [SerializeField] public Model.ConnStatus    status;
        [SerializeField] public GameResUnit[]       response;
    }

    [Serializable]
    public struct GameResUnit
    {
        [SerializeField] public string          user_id;
        [SerializeField] public GameMethod[]    methods;
    }

    [Serializable]
    public struct GameReq
    {
        [SerializeField] public Model.Session   session;
        [SerializeField] public GameMethod[]    methods;
    }

    [Serializable]
    public struct GameMethod
    {
        [SerializeField] public GameMethodType  method;
        [SerializeField] public string          content; 
    }
}