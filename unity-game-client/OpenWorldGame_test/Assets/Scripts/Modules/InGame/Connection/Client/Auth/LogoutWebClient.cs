using System;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LogoutWebClient : WebClient
{

    [Header("Logout Information")]
    [SerializeField] protected LogoutRequestData logoutRequestData;

    /// <summary>
    /// Logout Request Data: send to Server
    /// </summary>
    [Serializable]
    public struct LogoutRequestData
    {
        [SerializeField] public uint user_id;
        [SerializeField] public string session_id;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="session_id"></param>
        public LogoutRequestData(uint user_id, string session_id)
        {
            this.user_id = user_id;
            this.session_id = session_id;
        }
    }

    /// <summary>
    /// Logout Response Data: receive from Server
    /// </summary>
    [Serializable]
    public struct LogoutResponseData
    {
        [SerializeField] public Model.ConnStatus status;
    }


    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{logoutPath}" with logoutRequestData 
    /// </summary>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public LogoutWebClient(ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port, string logoutPath) : base(protocol, requestMethod, hostname, port, logoutPath)
    {
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{logoutPath}" with logoutRequestData 
    /// </summary>
    /// <param name="logoutRequestData"></param>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public LogoutWebClient(LogoutRequestData logoutRequestData, ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port, string logoutPath) : base(protocol, requestMethod, hostname, port, logoutPath)
    {
        this.logoutRequestData = logoutRequestData;
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{logoutPath}" with logoutRequestData 
    /// </summary>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public LogoutWebClient(uint user_id, string session_id, ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port, string logoutPath) : base(protocol, requestMethod, hostname, port, logoutPath)
    {
        SetData(user_id,session_id);
    }

    /// <summary>
    /// Setdata 
    /// </summary>
    /// <param name="user_id"></param>
    /// <param name="session_id"></param>
    public void SetData(uint user_id, string session_id)
    {
        this.logoutRequestData = new LogoutRequestData(user_id,session_id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lrd"></param>
    /// <returns></returns>
    protected bool CheckResponseData(LogoutResponseData lrd)
    {
        return true;
    }

    /// <summary>
    /// </summary>
    /// <returns>if request data is appropriate or not</returns>
    protected override bool CheckRequestData()
    {
        bool ok = true;
        if (this.logoutRequestData.user_id==0)
        {
            ok = false;
            this.message = $"ログインしていません。";
        }
        else if (string.IsNullOrEmpty(this.logoutRequestData.session_id))
        {
            ok = false;
            this.message = $"ログインしていません。";
        }

        return ok;
    }

    /// <summary>
    /// Setup Web Request Data 
    /// </summary>
    /// <returns></returns>
    protected override void HandleSetupWebRequestData(UnityWebRequest www)
    {
        byte[] postData = System.Text.Encoding.UTF8.GetBytes("{\"session\":" + JsonUtility.ToJson(this.logoutRequestData) + "}");
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
    }


    /// <summary>
    /// HandleSuccessData: 通信に成功した時にLogoutクライアントが行う処理
    /// dataに値を保存 
    /// </summary>
    /// <param name="response">received data</param>
    /// <returns></returns>
    protected override void HandleSuccessData(string response)
    {
        Debug.Log("Receive data: " + response);
        this.data = JsonUtility.FromJson<LogoutResponseData>(response);
        LogoutResponseData lrd = (LogoutResponseData)this.data;
        if (CheckResponseData(lrd) != true)
        {
            this.message = "Failed to parse response data. ";
            this.result = ResultType.ResponseDataError;
            Debug.Log(this.message);
        }
        else
        {
            //this.message = $"通信成功!\nData: {lrd.ToString()}";
            this.message = $"通信成功!\nData: status: {lrd.status}}} ";
        }
    }

    /// <summary>
    /// HandleErrorData: 通信に失敗した時にLogoutクライアントが行う処理
    /// </summary>
    protected override void HandleErrorData(string error)
    {
        this.message = $"通信失敗！\n{error}";
        Debug.Log($"error: \n{error}");
    }

    /// <summary>
    /// HandleInProgressData: 通信に途中だった時にLogoutクライアントが行う処理 
    /// </summary>
    protected override void HandleInProgressData()
    {
        this.message = "通信中...";
        Debug.LogError("Unexpected UnityWebRequest Result");
    }
}
