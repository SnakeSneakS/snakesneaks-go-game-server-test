//Model.Session 
public partial class Model 
{
    public const int USER_ID_LENGTH_MIN = 1;
    public const int USER_ID_LENGTH_MAX=20;
    public const int SESSION_ID_LENGTH_MIN = 1;
    public const int SESSION_ID_LENGTH_MAX = 200;

    [System.Serializable]
    public class Session
    {
        [UnityEngine.SerializeField] private uint user_id;
        public uint UserID {
            get
            {
                return this.user_id;
            }
            set
            {
                this.user_id = value;
            }
        }
        [UnityEngine.SerializeField] private string session_id;
        public string SessionID
        {
            get
            {
                return this.session_id;
            }
            set
            {
                this.session_id = (value == null || value.Length>SESSION_ID_LENGTH_MAX || value.Length<SESSION_ID_LENGTH_MIN)?null:value;
            }
        }
        private bool error;
        public bool Error
        {
            get
            {
                return this.error;
            }
            private set
            {
                this.error = value;
            }
        }

        /// <summary>
        /// Session constructor: for default 
        /// </summary>
        public Session()
        {
            this.user_id = 0;
            this.session_id = null;
            this.error = true;
        }

        /// <summary>
        /// Session constructor: for logout 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="sessionID"></param>
        public Session(uint userID,string sessionID)
        {
            this.user_id = userID;
            this.session_id = sessionID;
            this.error = false;

            if (this.user_id == 0 || this.session_id == null) this.error = true;
        }
    }
}
