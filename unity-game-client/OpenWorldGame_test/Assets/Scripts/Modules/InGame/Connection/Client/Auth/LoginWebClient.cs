using System;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginWebClient : WebClient
{

    [Header("Login Information")]
    [SerializeField] protected LoginRequestData loginRequestData;

    /// <summary>
    /// Login Request Data: send to Server
    /// </summary>
    [Serializable]
    public struct LoginRequestData
    {
        [SerializeField] public string email;
        [SerializeField] public string password;

        /// <summary>
        /// COnstructor
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public LoginRequestData(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }

    /// <summary>
    /// Login Response Data: receive from Server
    /// </summary>
    [Serializable]
    public struct LoginResponseData
    {
        [SerializeField] public Model.ConnStatus status;
        [SerializeField] public Model.Session session;
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{loginPath}" with loginRequestData 
    /// </summary>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public LoginWebClient(ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port, string loginPath, bool certAllowAll) : base(protocol, requestMethod, hostname, port, loginPath, certAllowAll)
    {
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{loginPath}" with loginRequestData 
    /// </summary>
    /// <param name="loginRequestData"></param>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public LoginWebClient(LoginRequestData loginRequestData, ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port, string loginPath) : base(protocol, requestMethod, hostname, port, loginPath)
    {
        this.loginRequestData = loginRequestData;
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{loginPath}" with loginRequestData 
    /// </summary>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public LoginWebClient(string email, string password, ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port, string loginPath) : base(protocol, requestMethod, hostname, port, loginPath)
    {
        SetData(email, password);
    }

    /// <summary>
    /// Setdata 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    public void SetData(string email, string password)
    {
        this.loginRequestData = new LoginRequestData(email, password);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lrd"></param>
    /// <returns></returns>
    protected bool CheckResponseData(LoginResponseData lrd)
    {
        bool error = (lrd.status == Model.ConnStatus.success && (lrd.session.UserID == 0 || lrd.session.SessionID == ""));
        if (error) this.result = ResultType.ResponseDataError;
        return (!error);
    }

    /// <summary>
    /// </summary>
    /// <returns>if request data is appropriate or not</returns>
    protected override bool CheckRequestData()
    {
        bool ok = true;
        if (this.loginRequestData.email.Length > Model.EMAIL_LENGTH_MAX || this.loginRequestData.email.Length < Model.EMAIL_LENGTH_MIN)
        {
            ok = false;
            this.message = $"???????????????????????????????????????!\n{Model.EMAIL_LENGTH_MIN}?????????{Model.EMAIL_LENGTH_MAX}????????????????????????????????????";
        }
        else if (this.loginRequestData.password.Length > Model.PASSWORD_LENGTH_MAX || this.loginRequestData.password.Length < Model.PASSWORD_LENGTH_MIN)
        {
            ok = false;
            this.message = $"?????????????????????????????????!\n{Model.PASSWORD_LENGTH_MIN}?????????{Model.PASSWORD_LENGTH_MAX}????????????????????????????????????";
        }
        else
        {
            try
            {
                new MailAddress(this.loginRequestData.email);
            }
            catch
            {
                ok = false;
                this.message = "??????????????????????????????????????????\n???????????????????????????????????????????????????";
            }
        }

        return ok;
    }

    /// <summary>
    /// Setup Web Request Data 
    /// </summary>
    /// <returns></returns>
    protected override void HandleSetupWebRequestData(UnityWebRequest www)
    {
        byte[] postData = System.Text.Encoding.UTF8.GetBytes("{\"user\":" + JsonUtility.ToJson(this.loginRequestData) + "}");
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
    }


    /// <summary>
    /// HandleSuccessData: ???????????????????????????Login?????????????????????????????????
    /// data??????????????? 
    /// </summary>
    /// <param name="response">received data</param>
    /// <returns></returns>
    protected override void HandleSuccessData(string response)
    {
        Debug.Log("Receive data: " + response);
        this.data = JsonUtility.FromJson<LoginResponseData>(response);
        LoginResponseData lrd = (LoginResponseData)this.data;
        if (CheckResponseData(lrd) != true)
        {
            this.message = "Failed to parse response data. ";
            this.result = ResultType.ResponseDataError;
            Debug.Log(this.message);
        }
        else
        {
            //this.message = $"????????????!\nData: {lrd.ToString()}";
            this.message = $"????????????!\nData: status: {lrd.status}, session: {{user_id: {lrd.session.UserID}, session_id: {lrd.session.SessionID}}} ";
        }
    }

    /// <summary>
    /// HandleErrorData: ???????????????????????????Login?????????????????????????????????
    /// </summary>
    protected override void HandleErrorData(string error)
    {
        this.message = $"???????????????\n{error}";
        Debug.Log($"error: \n{error}");
    }

    /// <summary>
    /// HandleInProgressData: ??????????????????????????????Login????????????????????????????????? 
    /// </summary>
    protected override void HandleInProgressData()
    {
        this.message = "?????????...";
        Debug.LogError("Unexpected UnityWebRequest Result");
    }
}
