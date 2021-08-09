using System;
using UnityEngine;

//Model.GameReq 
public partial class Model
{
#if false
    /// <summary>
    /// Game Request Class
    /// </summary>
    [Serializable]
    public class GameReq
    {
        [SerializeField] private Session session;
        [SerializeField] private Gamemodel.GameReqTransform transform;
        [SerializeField] private string text;

        /// <summary>
        /// GameRequest Constructor 
        /// </summary>
        /// <param name="session">Model.Session</param>
        /// <param name="transform">UnityEngine.Transform</param>
        public GameReq(Session session,Transform transform)
        {
            this.session = session;
            this.transform = new Gamemodel.GameReqTransform(transform);
            this.text = null;
        }
        /// <summary>
        /// GameRequest Constructor 
        /// </summary>
        /// <param name="session">Model.Session</param>
        /// <param name="gtransform">Model.GameReqTransform</param>
        public GameReq(Session session, Gamemodel/GameReqTransform gtransform)
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
#endif
    
}
