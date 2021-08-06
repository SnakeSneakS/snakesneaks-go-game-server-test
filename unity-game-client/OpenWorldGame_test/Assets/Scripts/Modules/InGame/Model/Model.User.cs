using System.Collections;
using System.Net.Mail;

//Model.User
public partial class Model 
{
    const int USERNAME_LENGTH_MAX = 10;
    const int EMAIL_LENGTH_MAX = 50;
    const int PASSWORD_LENGTH_MAX = 50;

    [System.Serializable]
    public class User
    {
        [UnityEngine.SerializeField] public uint id;
        [UnityEngine.SerializeField] private string username;
        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = (value==null || value.Length > USERNAME_LENGTH_MAX) ? null : value;
            }
        }
        [UnityEngine.SerializeField] private string email;
        public string Email {
            get
            {
                return this.email;
            }
            set
            {
                try
                {
                    this.email = (value == null || value.Length>PASSWORD_LENGTH_MAX)?null: new MailAddress(value).Address;
                }
                catch
                {
                    this.email = null;
                }
            }
        }
        [UnityEngine.SerializeField] private string password;
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = (value == null || value.Length > PASSWORD_LENGTH_MAX) ? null : value;
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
        /// User Constructor: for default
        /// </summary>
        public User()
        {
            this.Username = null;
            this.Email = null;
            this.Password = null;
            this.error = true;
        }

        ///<summary>
        /// User Constructor: for login or signup
        /// <param name="username">username</param>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// </summary>
        public User(string username, string email, string password)
        {
            this.Username = username;
            this.Email = email;
            this.Password = password;
            this.error = false;

            if (this.Username == null || this.Email == null || this.Password == null) this.Error = true; 
        }
    }
}
