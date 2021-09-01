using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;
    [SerializeField] float cameraRotationSpeed=90.0f;
    [SerializeField] Joystick cameraRotationJoystick;

    public Quaternion rotationV { get; private set; } //vertical 
    public Quaternion rotationH { get; private set; } //horizontal
    public Quaternion rotation { get { return rotationH * rotationV; } }
    private float m_distance; //distance
    private float rectPosY; //camera's position of Y 

    void Start()
    {
        Vector3 diff = this.gameObject.transform.position - cameraTarget.position;
        this.m_distance = diff.magnitude;
        Debug.Log("distance: "+this.m_distance);
        this.rotationH = Quaternion.Euler(0, this.gameObject.transform.eulerAngles.y, this.gameObject.transform.eulerAngles.z);
        this.rotationV = Quaternion.Euler(this.gameObject.transform.eulerAngles.x, 0, 0);
        this.rectPosY = (diff + (this.gameObject.transform.rotation * (Vector3.forward * m_distance))).y;
    }

    private void Update()
    {
        //rotate camera from joystick input
        if(cameraRotationJoystick.Horizontal!=0 || cameraRotationJoystick.Vertical != 0)
        {
            RotateByNormalizedArguments(cameraRotationJoystick.Horizontal * Time.deltaTime, -cameraRotationJoystick.Vertical * Time.deltaTime);
            Debug.Log("position: " + this.gameObject.transform.position);
            Debug.Log("rotation: " + this.gameObject.transform.rotation);
        }

    }

    private void LateUpdate()
    {
        //keep camera position
        this.gameObject.transform.rotation = rotation;
        this.gameObject.transform.position = Vector3.up * rectPosY + cameraTarget.position - ( this.gameObject.transform.rotation * (Vector3.forward * m_distance));
    }

    private void RotateByNormalizedArguments(float rotH, float rotV)
    {
        rotationH *= Quaternion.Euler(0, rotH * cameraRotationSpeed, 0);
        Quaternion new_rotationV = rotationV * Quaternion.Euler(rotV * cameraRotationSpeed, 0, 0);
        
        Debug.Log(rotationV.eulerAngles);
        if (new_rotationV.eulerAngles.x < 90 && new_rotationV.eulerAngles.y==0)
        {
            rotationV = new_rotationV;
        }
        
    }
}
