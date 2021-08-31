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
        if(isLimitFPS) Application.targetFrameRate = targetFPS;
    }
}
