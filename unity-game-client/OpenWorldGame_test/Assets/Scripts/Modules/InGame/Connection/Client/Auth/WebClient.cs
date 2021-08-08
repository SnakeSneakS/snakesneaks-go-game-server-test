using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class WebClient : MonoBehaviour
{
    [Header("Basic Information")]
    [SerializeField] protected  ProtocolType        protocol = ProtocolType.https;
    [SerializeField] protected  string              hostname            = "localhost";
    [SerializeField] protected  string              port                = "8000";
    [SerializeField] protected  string              path                = "/";
    [SerializeField] protected  HttpRequestMethod   httpRequestMethod   = HttpRequestMethod.Get;

    [SerializeField] protected bool certAllowAll = false;

    //store data read from response 
    public object data { get; protected set; } //parsed data
    public string message { get; protected set; } //message show to users or developers 
    public bool isSuccess { get; protected set; } //connection and data parse success 
    public bool isInProgress { get; private set; } //connection in progress 
    

    [Serializable]
    public enum ProtocolType
    {
        http,
        https,
    }

    /// <summary>
    /// Http Request Method Type 
    /// </summary>
    [Serializable] 
    public enum HttpRequestMethod
    {
        Get,
        //Head,
        Post,
        Put,
        Delete,
        //Connect,
        //Option,
        //Trace,
        //Patch
    }

    /// <summary>
    /// Constructor: requestMethod to $"(hostname}:{port}{path}"
    /// </summary>
    /// <param name="protocol">http, https</param>
    /// <param name="requestMethod"></param>
    /// <param name="hostname"></param>
    /// <param name="port"></param>
    /// <param name="path">default "/"</param>
    public WebClient(ProtocolType protocol, HttpRequestMethod requestMethod, string hostname, string port,string path="/")
    {
        this.protocol = protocol;
        this.hostname = hostname;
        this.port = port;
        this.httpRequestMethod = requestMethod;
        this.path = path;
    }

    /// <summary>
    /// Send To Server 
    /// </summary>
    /// <returns></returns>
    public IEnumerator Send()
    {
        if (this.isInProgress)
        {
            this.isSuccess = false;
            this.message = "Connection is in Progress...";
            Debug.Log("<color=\"red\">Previous WWW connection is in Progress...</color>");
            yield break;
        }

        if (CheckRequestData() != true)
        {
            this.isSuccess = false;
            Debug.Log("<color=\"red\">Invalid Data. So Stopped to Send Data to Server.</color>");
            yield break;
        }

        Refresh();
        using (UnityWebRequest www = new UnityWebRequest($"{protocol.ToString()}://{hostname}:{port}{path}", this.httpRequestMethod.ToString()))
        {
            isInProgress = true;

            //Certification
            //Note that this force all true
            if (certAllowAll)
            {
                www.certificateHandler = new ForceAllCertificationHandler();
            }

            //set up data sent to server 
            HandleSetupWebRequestData(www);

            //send data to server, and wait for response
            //define uploadHandler and downloadHandler 
            yield return www.SendWebRequest();

            //show response 
            Debug.Log($"Send data: {System.Text.Encoding.UTF8.GetString(www.uploadHandler.data)}\n To: {www.url}, Method: {www.method}");
            Debug.Log($"Response code: {www.responseCode}");

            //success
            if (www.result==UnityWebRequest.Result.Success)
            {
                isSuccess = true;
                this.message = "通信成功";
                HandleSuccessData(www.downloadHandler.text);
            }
            //in progress
            else if (www.result==UnityWebRequest.Result.InProgress)
            {
                HandleInProgressData();
            }
            //error 
            else
            {
                isSuccess = false;
                this.message = "通信失敗: \n"+www.error;
                HandleErrorData(www.error);
            }

            isInProgress = false;
            Debug.Log(this.message);
            //www.Dispose();
        }

        yield break;
    }

    /// <summary>
    /// Refresh before start new connection 
    /// </summary>
    private void Refresh()
    {
        this.data = null;
        this.message = null;
        this.isSuccess = false;
        this.isInProgress = true;
    }

    /// <summary>
    /// CheckRequestData Check Data before send data 
    /// </summary>
    protected abstract bool CheckRequestData();

    /// <summary>
    /// HandleSetupWebRequest: define web request  
    /// </summary>
    /// <returns></returns>
    protected abstract void HandleSetupWebRequestData(UnityWebRequest www);

    /// <summary>
    /// HandleSuccessData: define the way to handle the received data: when connection succeeded
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    protected abstract void HandleSuccessData(string response);

    /// <summary>
    /// HandleErrorData: define the way to handle connection error: when connection not succeeded 
    /// </summary>
    protected abstract void HandleErrorData(string error);

    /// <summary>
    /// HandleInProgressData: define the way to handle when inprogress 
    /// </summary>
    protected abstract void HandleInProgressData();
}
