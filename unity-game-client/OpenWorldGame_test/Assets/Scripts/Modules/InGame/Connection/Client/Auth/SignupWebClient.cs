using System;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SignupWebClient : WebClient 
{
    [Header("Signup Information")]
    [SerializeField] protected SignupRequestData signupRequestData;

    /// <summary>
    /// Signup Request Data: send to Server
    /// </summary>
    [Serializable]
    public struct SignupRequestData
    {
        [SerializeField] public string username;
        [SerializeField] public string email;
        [SerializeField] public string password;

        /// <summary>
        /// COnstructor
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public SignupRequestData(string username, string email,string password)
        {
            this.username = username;
            this.email = email;
            this.password = password;
        }
    }

    /// <summary>
    /// Signup Response Data: receive from Server
    /// </summary>
    [Serializable]
    public struct SignupResponseData
    {
        [SerializeField] public Model.ConnStatus status;
        [SerializeField] public Model.Session session;
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{signupPath}" with signupRequestData 
    /// </summary>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public SignupWebClient(ProtocolType protocol,HttpRequestMethod requestMethod, string hostname, string port, string signupPath) : base(protocol, requestMethod, hostname, port, signupPath)
    {
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{signupPath}" with signupRequestData 
    /// </summary>
    /// <param name="signupRequestData"></param>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public SignupWebClient(SignupRequestData signupRequestData, ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port, string signupPath) : base(protocol,requestMethod,hostname, port, signupPath)
    {
        this.signupRequestData = signupRequestData;
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{signupPath}" with signupRequestData 
    /// </summary>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public SignupWebClient(string username, string email, string password,ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port, string signupPath) : base(protocol,requestMethod, hostname, port, signupPath)
    {
        SetData(username, email,password);
    }

    /// <summary>
    /// Setdata 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    public void SetData(string username, string email, string password)
    {
        this.signupRequestData = new SignupRequestData(username, email, password);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lrd"></param>
    /// <returns></returns>
    protected bool CheckResponseData(SignupResponseData lrd)
    {
        return true;
    }

    /// <summary>
    /// </summary>
    /// <returns>if request data is appropriate or not</returns>
    protected override bool CheckRequestData()
    {
        bool ok = true;
        if (this.signupRequestData.email.Length > Model.EMAIL_LENGTH_MAX || this.signupRequestData.email.Length< Model.EMAIL_LENGTH_MIN)
        {
            ok = false;
            this.message = $"不適切なメールアドレスです!\n{Model.EMAIL_LENGTH_MIN}文字〜{Model.EMAIL_LENGTH_MAX}文字で入力してください。";
        }else if (this.signupRequestData.password.Length > Model.PASSWORD_LENGTH_MAX || this.signupRequestData.password.Length< Model.PASSWORD_LENGTH_MIN)
        {
            ok = false;
            this.message = $"不適切なパスワードです!\n{Model.PASSWORD_LENGTH_MIN}文字〜{Model.PASSWORD_LENGTH_MAX}文字で入力してください。";
        }
        else
        {
            try
            {
                new MailAddress(this.signupRequestData.email);
            }
            catch
            {
                ok = false;
                this.message = "不適切なメールアドレスです！\n間違っていないか確認してください。";
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
        byte[] postData = System.Text.Encoding.UTF8.GetBytes("{\"user\":" + JsonUtility.ToJson(this.signupRequestData) + "}");
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
    }


    /// <summary>
    /// HandleSuccessData: 通信に成功した時にSignupクライアントが行う処理
    /// dataに値を保存 
    /// </summary>
    /// <param name="response">received data</param>
    /// <returns></returns>
    protected override void HandleSuccessData(string response)
    {
        Debug.Log("Receive data: " +response);
        this.data = JsonUtility.FromJson<SignupResponseData>(response);
        SignupResponseData lrd = (SignupResponseData)this.data;
        if (CheckResponseData(lrd)!=true)
        {
            this.message = "Failed to parse response data. ";
            this.result = ResultType.ResponseDataError;
            Debug.Log(this.message);
        }
        else
        {
            //this.message = $"通信成功!\nData: {lrd.ToString()}";
            this.message = $"通信成功!\nData: status: {lrd.status}, session: {{user_id: {lrd.session.UserID}, session_id: {lrd.session.SessionID}}} ";
        }
    }

    /// <summary>
    /// HandleErrorData: 通信に失敗した時にSignupクライアントが行う処理
    /// </summary>
    protected override void HandleErrorData(string error)
    {
        this.message = $"通信失敗！\n{error}";
        Debug.Log($"error: \n{error}");
    }

    /// <summary>
    /// HandleInProgressData: 通信に途中だった時にSignupクライアントが行う処理 
    /// </summary>
    protected override void HandleInProgressData()
    {
        this.message = "通信中..."; 
        Debug.LogError("Unexpected UnityWebRequest Result");
    }
}
