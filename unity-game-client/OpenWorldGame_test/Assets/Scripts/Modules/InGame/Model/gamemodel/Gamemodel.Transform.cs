using System;
using UnityEngine;

//Gamemodel.Transform 
public partial class Gamemodel 
{
    /// <summary>
    /// Transform Class used for Game Request
    /// </summary>
    [Serializable]
    public struct GameTransform
    {
        [SerializeField] public Vector3 position;
        [SerializeField] public Vector3 rotation;

        public GameTransform(Transform transform)
        {
            this.position = transform.position;
            this.rotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        }
    }
}
