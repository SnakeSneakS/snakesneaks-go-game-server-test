//Model.Session 
public partial class Model 
{
    const int USER_ID_LENGTH_MAX=20;
    const int SESSION_ID_LENGTH_MAX = 200;

    [System.Serializable]
    public class Session
    {
        [UnityEngine.SerializeField] private string user_id;
        public string UserID {
            get
            {
                return this.user_id;
            }
            set
            {
                this.user_id = (value == null || value.Length > USER_ID_LENGTH_MAX) ? null : value;
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
                this.session_id = (value == null || value.Length>SESSION_ID_LENGTH_MAX)?null:value;
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
            this.user_id = null;
            this.session_id = null;
            this.error = true;
        }

        /// <summary>
        /// Session constructor: for logout 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="sessionID"></param>
        public Session(string userID,string sessionID)
        {
            this.user_id = userID;
            this.session_id = sessionID;
            this.error = false;

            if (this.user_id == null || this.session_id == null) this.error = true;
        }
    }
}
