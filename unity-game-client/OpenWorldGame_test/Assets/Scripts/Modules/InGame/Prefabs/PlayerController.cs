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
    [SerializeField] float checkReachDistance = 0.5f;
    [SerializeField] float checkMoveMaxDistance = 7.0f; //if distance is larger than this, player don't move but do teleportation. 
    [SerializeField] float moveSpeedTime = 1.0f; //次のポイントへの移動に何秒かかるか 
    [SerializeField] float magnitudeToVelocityRatio = 4.0f;

    [Header("Camera")]
    
    [SerializeField] MainCameraController mainCameraController; //カメラの向きから、次に進む方向を決める。 

    Vector3 toPosition;
    Vector3 toVelocity;
    Vector3 toRotation;

    public void SetUsername(string username)
    {
        this.usernameTmp.text = username;
    }

    public Vector3 NextPositionFromVector2(Vector2 vec2)
    {
        Vector3 nextPos;
        if (isMyPlayer)
        {
            nextPos = this.gameObject.transform.position + magnitudeToVelocityRatio * (mainCameraController.rotationH * new Vector3(vec2.x, 0, vec2.y));

        }
        else
        {
            nextPos = this.gameObject.transform.position + magnitudeToVelocityRatio * new Vector3(vec2.x, 0, vec2.y);
        }
        return nextPos;
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
                this.transform.rotation = Quaternion.LookRotation(toVelocity);
            }
#if false
        }    
#endif
    }
}
