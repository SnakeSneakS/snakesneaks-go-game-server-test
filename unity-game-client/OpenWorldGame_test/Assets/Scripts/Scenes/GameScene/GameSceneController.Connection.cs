using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GameSceneController.Connection 
public partial class GameSceneController : MonoBehaviour
{
    

    public void OnConnectionOut()
    {
        Debug.Log("<color=\"red\">Connection Out!!</color>");
        //dispatcher.Invoke(() => { RequireReLogin(); });
        RequireReLogin();
    }

    private void RequireReLogin()
    {
        Debug.Log("Require Re-Login.");
        SceneManager.LoadScene(SceneManager.SceneName.Title);
    }
}
