using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ClientManager 
public class ClientManager : MonoBehaviour
{
    public static Model.Session Session=new Model.Session();

    //Load
    public static void LoadLocalData()
    {
        LoadSession();
    }

    public static void LoadSession()
    {
        
        string user_id = PlayerPrefs.GetString(PlayerPrefsSystemDefine.CLIENT_USER_ID);
        string session_id = PlayerPrefs.GetString(PlayerPrefsSystemDefine.CLIENT_SESSION_ID);
        Session = new Model.Session(user_id,session_id);
        Debug.Log($"Load Session Data\nUserID: {Session.UserID}, SessionID: {Session.SessionID}");
    }

    //Save
    public static void SaveLocalData()
    {
        SaveSession();
    }

    public static void SaveSession()
    {
        PlayerPrefs.SetString(PlayerPrefsSystemDefine.CLIENT_USER_ID, Session.UserID);
        PlayerPrefs.SetString(PlayerPrefsSystemDefine.CLIENT_SESSION_ID, Session.SessionID);
        Debug.Log($"Save Session Data\nUserID: {Session.UserID}, SessionID: {Session.SessionID}");
    }

    //Update: like when session expired
    public static void UpdateSession(Model.Session session) {
        Session = session;
        Debug.Log($"Session Updated!\nUserID: {session.UserID}, SessionID: {session.SessionID}");
        SaveSession();
    }
}
