using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] bool isMyPlayer = false;
    [SerializeField] TextMeshProUGUI usernameTmp;
    [SerializeField] GameObject usernameUI;

    [Header("Move State")]
    bool isMoving = false;
    [Tooltip("If next distination is near than this value, judge as \"Reached\"")]
    [SerializeField] float checkReachDistance = 0.5f;
    [Tooltip("If next distination is far than this value, not move but teleport")]
    [SerializeField] float checkMoveMaxDistance = 7.0f; //if distance is larger than this, player don't move but do teleportation.
    [Tooltip("the time required to move to next destination.")]
    [SerializeField] float moveSpeedTime = 1.0f; //次のポイントへの移動に何秒かかるか
    [Tooltip("the ratio how fast player turn to next destination")][Range(0,1)]
    [SerializeField] float turnSpeedRatio = 0.5f; //どれくらい早く移動方向を向くか
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

        isMoving = true;
        toVelocity = DiffPos / moveSpeedTime;

        Debug.Log($"Move to: ({toPosition.x},{toPosition.y},{toPosition.z})");
    }

    private void CheckAndReach()
    {
        Vector3 DiffPos = this.transform.position - toPosition;
        if ( (DiffPos.magnitude < checkReachDistance) || (DiffPos.magnitude > checkMoveMaxDistance) )
        {
            isMoving = false;
            this.gameObject.transform.position = toPosition;
            this.gameObject.transform.rotation = Quaternion.Euler(toRotation);
        }
    }

    private void OnEnable()
    {
        this.mainCameraController = Camera.main.GetComponent<MainCameraController>();
    }

    private void Update()
    {
        
        if (isMoving)
        {
            //MoveControll
             CheckAndReach();
            this.transform.position += Time.deltaTime * toVelocity;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(toVelocity), turnSpeedRatio);
        }

        //NameControll
        if(this.usernameUI!=null) this.usernameUI.transform.rotation = Quaternion.LookRotation((this.usernameUI.transform.position - mainCameraController.gameObject.transform.position));
    }
}
