using System;
using UnityEngine;

//Gamemodel.Transform 
public partial class Gamemodel 
{
/// <summary>
/// Transform Class used for Game Request
/// </summary>
[Serializable]
public class GameReqTransform
{
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;

    public GameReqTransform(Transform transform)
    {
        this.position = transform.position;
        this.rotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
    }
}
}
