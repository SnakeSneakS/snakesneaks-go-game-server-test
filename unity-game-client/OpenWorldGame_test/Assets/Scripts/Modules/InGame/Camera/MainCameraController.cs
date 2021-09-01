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
    float m_distance; //distance


    void Start()
    {
        Vector3 diff = this.gameObject.transform.position - cameraTarget.position;
        this.m_distance = diff.magnitude;
        this.rotationH = Quaternion.identity;
        this.rotationV = Quaternion.Euler(this.gameObject.transform.rotation.x, 0, this.gameObject.transform.rotation.z);
    }

    private void Update()
    {
        //rotate camera from joystick input
        if(cameraRotationJoystick.Horizontal!=0 || cameraRotationJoystick.Vertical != 0)
        {
            Rotate(cameraRotationJoystick.Horizontal * Time.deltaTime, -cameraRotationJoystick.Vertical * Time.deltaTime);
        }

    }

    private void LateUpdate()
    {
        //keep camera position
        this.gameObject.transform.position = cameraTarget.position - transform.rotation * Vector3.forward * m_distance;
        this.gameObject.transform.rotation = rotationH * rotationV;
    }

    private void Rotate(float rotH, float rotV)
    {
        rotationH *= Quaternion.Euler(0, rotH * cameraRotationSpeed, 0);
        rotationV *= Quaternion.Euler(rotV * cameraRotationSpeed, 0, 0);
    }
}
