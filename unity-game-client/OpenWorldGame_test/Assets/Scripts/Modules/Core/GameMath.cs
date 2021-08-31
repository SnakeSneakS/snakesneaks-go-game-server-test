using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMath : MonoBehaviour
{
    public static Quaternion Vector3toQuaternion(Vector3 vec)
    {
        float x = 0; // Vector3.Angle(Vector3.forward,new Vector3(0,vec.y,vec.z));
        float y = Vector3.Angle(Vector3.forward, new Vector3(vec.x, 0, vec.z));
        float z = 0; // Vector3.Angle(Vector3.up, new Vector3(vec.x, vec.y, 0));
        Vector3 angle = new Vector3(x, y, z);
        return Quaternion.Euler(angle);
    }
}
