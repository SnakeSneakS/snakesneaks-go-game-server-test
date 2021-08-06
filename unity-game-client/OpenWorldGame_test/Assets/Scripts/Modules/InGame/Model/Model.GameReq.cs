using System;
using UnityEngine;

//Model.GameReq 
public partial class Model
{
    /// <summary>
    /// Game Request Class
    /// </summary>
    [Serializable]
    public class GameReq
    {
        [SerializeField] private Session session;
        [SerializeField] private GameReqTransform transform;
        [SerializeField] private string text;

        /// <summary>
        /// GameRequest Constructor 
        /// </summary>
        /// <param name="session">Model.Session</param>
        /// <param name="transform">UnityEngine.Transform</param>
        public GameReq(Session session,Transform transform)
        {
            this.session = session;
            this.transform = new GameReqTransform(transform);
            this.text = null;
        }
        /// <summary>
        /// GameRequest Constructor 
        /// </summary>
        /// <param name="session">Model.Session</param>
        /// <param name="gtransform">Model.GameReqTransform</param>
        public GameReq(Session session, GameReqTransform gtransform)
        {
            this.session = session;
            this.transform = gtransform;
            this.text = null;
        }
        /// <summary>
        /// GameRequest Constructor 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="text"></param>
        public GameReq(Session session, string text)
        {
            this.session = session;
            this.transform = null;
            this.text = text;
        }
    }

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
