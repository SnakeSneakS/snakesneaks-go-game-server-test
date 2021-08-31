using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] bool isMyPlayer = false;
    [SerializeField] TextMeshProUGUI usernameTmp;

    [Header("Move State")]
    bool isMoving = false;
    float checkReachDistance = 0.1f;
    float checkMoveMaxDistance = 5.0f; //if distance is larger than this, player don't move but do teleportation. 
    float moveSpeedTime = 1.0f; //次のポイントへの移動に何秒かかるか 
    float magnitudeToVelocityRatio = 2.0f;

    Vector3 toPosition;
    Vector3 toVelocity;
    Vector3 toRotation;

    public void SetUsername(string username)
    {
        this.usernameTmp.text = username;
    }

    public Vector3 NextPositionFromVector2(Vector2 vec2)
    {
        return this.gameObject.transform.position + magnitudeToVelocityRatio * new Vector3(vec2.x,0,vec2.y);
    }

    public void MoveTo(Gamemodel.GameTransform to)
    {
        isMoving = true;
        toPosition = to.position;
        toRotation = to.rotation;

        Vector3 DiffPos = to.position - this.gameObject.transform.position;

        if ( DiffPos.magnitude > checkMoveMaxDistance)
        {
            isMoving = false;
            CheckAndReach();
        }
        else
        {
            isMoving = true;
            toVelocity = DiffPos / moveSpeedTime;
        }

        Debug.Log($"Move to: ({toPosition.x},{toPosition.y},{toPosition.z})");
    }

    private void CheckAndReach()
    {
        isMoving = false;
        this.gameObject.transform.position = toPosition;
        this.gameObject.transform.rotation = Quaternion.Euler(toRotation);
    }

    private void Update()
    {
#if false
        if (isMyPlayer)
        {

        }
        else
        {
#endif
            if (isMoving)
            {
                if ((this.transform.position - toPosition).magnitude < checkReachDistance)
                {
                    CheckAndReach();
                }
                this.transform.position += Time.deltaTime * toVelocity;
                this.transform.rotation = GameMath.Vector3toQuaternion(toVelocity);
            }
#if false
        }    
#endif
    }
}
