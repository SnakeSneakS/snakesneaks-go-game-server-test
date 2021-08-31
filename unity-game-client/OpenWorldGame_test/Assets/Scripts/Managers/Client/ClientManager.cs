using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ClientManager 
public class ClientManager : MonoBehaviour
{
    public static Model.Session Session=new Model.Session();

    public static string Username = "";


    //Load
    public static void LoadLocalData()
    {
        LoadSession();
        LoadUsername();
    }

    public static void LoadSession()
    {

        uint user_id;
        try { user_id = System.Convert.ToUInt32(PlayerPrefs.GetString(PlayerPrefsSystemDefine.CLIENT_USER_ID));}
        catch { user_id = 0; }
        string session_id = PlayerPrefs.GetString(PlayerPrefsSystemDefine.CLIENT_SESSION_ID);
        Session = new Model.Session(user_id,session_id);
        Debug.Log($"Load Session Data\nUserID: {Session.UserID}, SessionID: {Session.SessionID}");
    }

    public static void LoadUsername()
    {
        Username = PlayerPrefs.GetString(PlayerPrefsSystemDefine.CLIENT_USERNAME);
        Debug.Log($"Load Username Data\nUsername: {Username}");
    }


    //Save
    public static void SaveLocalData()
    {
        SaveSession();
        SaveUsername();
    }

    public static void SaveSession()
    {
        PlayerPrefs.SetString(PlayerPrefsSystemDefine.CLIENT_USER_ID, Session.UserID.ToString());
        PlayerPrefs.SetString(PlayerPrefsSystemDefine.CLIENT_SESSION_ID, Session.SessionID);
        Debug.Log($"Save Session Data\nUserID: {Session.UserID}, SessionID: {Session.SessionID}");
    }

    public static void SaveUsername()
    {
        PlayerPrefs.SetString(PlayerPrefsSystemDefine.CLIENT_USERNAME, Username);
        Debug.Log($"Save Username Data\nUsername: {Username}");
    }


    //Update: like when session expired
    public static void UpdateSession(Model.Session session) {
        Session = session;
        SaveSession();
    }

    public static void UpdateUsername(string username)
    {
        Username = username;
        SaveUsername();
    }

    //Delete
    public static void DeleteLocalData()
    {
        Session.UserID = 0;
        Session.SessionID = null;
        PlayerPrefs.DeleteKey(PlayerPrefsSystemDefine.CLIENT_USER_ID);
        PlayerPrefs.DeleteKey(PlayerPrefsSystemDefine.CLIENT_SESSION_ID);
        PlayerPrefs.DeleteKey(PlayerPrefsSystemDefine.CLIENT_USERNAME);
    }
}
