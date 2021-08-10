using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(SceneManager.SceneName.Game);
    }

}
