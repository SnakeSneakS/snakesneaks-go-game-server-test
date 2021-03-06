using System;
using System.Net.Mail;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//Check If Session is Valid or Not 
public class SessionWebClient: WebClient  
{
    [Header("Session Information")]
    [SerializeField] protected SessionRequestData sessionRequestData;

    /// <summary>
    /// Session Request Data: send to Server to check if session is alive or not 
    /// </summary>
    [Serializable]
    public struct SessionRequestData
    {
        [SerializeField] public Model.Session session;

        /// <summary>
        /// COnstructor
        /// </summary>
        /// <param name="session"></param>
        public SessionRequestData(Model.Session session)
        {
            this.session = session;
        }
    }

    /// <summary>
    /// Session Response Data: receive from Server
    /// </summary>
    [Serializable]
    public struct SessionResponseData
    {
        [SerializeField] public Model.ConnStatus status;
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{sessionPath}" with sessionRequestData 
    /// </summary>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public SessionWebClient(ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port, string sessionPath, bool certAllowAll=false) : base(protocol, requestMethod, hostname, port, sessionPath, certAllowAll)
    {
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{sessionPath}" with sessionRequestData 
    /// </summary>
    /// <param name="sessionRequestData"></param>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public SessionWebClient(SessionRequestData sessionRequestData, ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port, string sessionPath, bool certAllowAll=false) : base(protocol, requestMethod, hostname, port, sessionPath, certAllowAll)
    {
        this.sessionRequestData = sessionRequestData;
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{sessionPath}" with sessionRequestData 
    /// </summary>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public SessionWebClient(Model.Session session, ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port, string sessionPath, bool certAllowAll) : base(protocol, requestMethod, hostname, port, sessionPath, certAllowAll)
    {
        SetData(session);
    }

    /// <summary>
    /// Setdata 
    /// </summary>
    /// <param name="session"></param>
    public void SetData(Model.Session session)
    {
        this.sessionRequestData = new SessionRequestData(session);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="srd"></param>
    /// <returns></returns>
    protected bool CheckResponseData(SessionResponseData srd)
    {
        return true;
    }

    /// <summary>
    /// </summary>
    /// <returns>if request data is appropriate or not</returns>
    protected override bool CheckRequestData()
    {
        bool ok = true;
        if (this.sessionRequestData.session==null || this.sessionRequestData.session.Error || this.sessionRequestData.session.SessionID==null)
        {
            ok = false;
            this.message = $"session data error!";
        }
        else
        {
            if (this.sessionRequestData.session.SessionID.Length > Model.SESSION_ID_LENGTH_MAX || this.sessionRequestData.session.SessionID.Length < Model.SESSION_ID_LENGTH_MIN)
            {
                ok = false;
                this.message = $"??????????????????????????????";
            }
            else if (this.sessionRequestData.session.UserID==0)
            {
                ok = false;
                this.message = $"?????????????????????????????????";
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
        byte[] postData = System.Text.Encoding.UTF8.GetBytes( JsonUtility.ToJson(this.sessionRequestData) );
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
    }


    /// <summary>
    /// HandleSuccessData: ???????????????????????????Session?????????????????????????????????
    /// data??????????????? 
    /// </summary>
    /// <param name="response">received data</param>
    /// <returns></returns>
    protected override void HandleSuccessData(string response)
    {
        Debug.Log("Receive data: " + response);
        this.data = JsonUtility.FromJson<SessionResponseData>(response);
        SessionResponseData lrd = (SessionResponseData)this.data;
        if (CheckResponseData(lrd) != true)
        {
            this.message = "Failed to parse response data. ";
            this.result = ResultType.ResponseDataError;
            Debug.Log(this.message);
        }
        else
        {
            //this.message = $"????????????!\nData: {lrd.ToString()}";
            this.message = $"????????????!\nData: status: {lrd.status} ";
        }
    }

    /// <summary>
    /// HandleErrorData: ???????????????????????????Session?????????????????????????????????
    /// </summary>
    protected override void HandleErrorData(string error)
    {
        this.message = $"???????????????\n{error}";
        Debug.Log($"error: \n{error}");
    }

    /// <summary>
    /// HandleInProgressData: ??????????????????????????????Session????????????????????????????????? 
    /// </summary>
    protected override void HandleInProgressData()
    {
        this.message = "?????????...";
        Debug.LogError("Unexpected UnityWebRequest Result");
    }
}
