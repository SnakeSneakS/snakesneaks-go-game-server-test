using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    [SerializeField] bool isLimitFPS = false;
    [SerializeField] int targetFPS = 30;

    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_WEBGL
        if(isLimitFPS) Application.targetFrameRate = targetFPS;
#endif
    }
}
