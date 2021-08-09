using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public enum SceneName
    {
        Title,
        Login, //login and signup
        UserInfo,
        LoadGame,
        Game,
    }
    public static void LoadScene(SceneName name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name.ToString());
    }
}
