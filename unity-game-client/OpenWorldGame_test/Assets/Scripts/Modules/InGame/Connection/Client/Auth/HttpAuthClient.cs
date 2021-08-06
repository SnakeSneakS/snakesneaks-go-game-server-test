using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 

public class HttpAuthClient 
{
    private Uri targetUri;
    public UnityWebRequest www;

    public HttpAuthClient(Uri targetUri)
    {
        this.targetUri = targetUri;
        this.www =null;
    }

    /// <summary>
    /// Signup Request 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public IEnumerator Signup(Model.User user)
    {
        if (www != null && www.result == UnityWebRequest.Result.InProgress)
        {
            Debug.LogError("already connecting");
            yield break;
        }


        byte[] postData = System.Text.Encoding.ASCII.GetBytes("{\"user\":" + JsonUtility.ToJson(user) + "}");
        www = new UnityWebRequest(targetUri, "Post");
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        Debug.Log("send data: "+System.Text.Encoding.ASCII.GetString(www.uploadHandler.data));

        switch (www.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("receive data: "+www.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(www.error);
                break;
            default:
                Debug.LogError("Unexpected UnityWebRequest Result");
                break;
        }
        Debug.Log($"response code: {www.responseCode}");

        yield break;
    } 

    /// <summary>
    /// Login Request 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public IEnumerator Login(Model.User user)
    {
        if (www!=null && www.result==UnityWebRequest.Result.InProgress)
        {
            Debug.LogError("already connecting");
            yield break;
        }

        byte[] postData = System.Text.Encoding.ASCII.GetBytes("{\"user\":" + JsonUtility.ToJson(user) + "}");
        www = new UnityWebRequest(targetUri, "Post");
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        Debug.Log("send data: " + System.Text.Encoding.ASCII.GetString(www.uploadHandler.data));

        switch (www.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(www.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(www.error);
                break;
            default:
                Debug.LogError("Unexpected UnityWebRequest Result");
                break; 
        }
        Debug.Log($"response code: {www.responseCode}");

        yield break;
    }

    /// <summary>
    /// Logout Request 
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    public IEnumerator Logout(Model.Session session)
    {
        if (www != null && www.result == UnityWebRequest.Result.InProgress)
        {
            Debug.LogError("already connecting");
            yield break;
        }

        byte[] postData = System.Text.Encoding.ASCII.GetBytes("{\"session\":" + JsonUtility.ToJson(session) + "}");
        www = new UnityWebRequest(targetUri, "Post");
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        Debug.Log("send data: " + System.Text.Encoding.ASCII.GetString(www.uploadHandler.data));

        switch (www.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(www.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(www.error);
                break;
            default:
                Debug.LogError("Unexpected UnityWebRequest Result");
                break;
        }
        Debug.Log($"response code: {www.responseCode}");

        yield break;
    }
}
