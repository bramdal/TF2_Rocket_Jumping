using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBehaviour : MonoBehaviour
{
    public float mouseSensitivity = 150f;
    float mouseInputX;
    float currentAngle = 0f;
    public float verticalAngle;
    float clampLimitBottom, clampLimitTop;
    float mouseInputY;
    Transform player;

    private void Start() {
        player = transform.parent;

        clampLimitBottom = -verticalAngle/2;
        clampLimitTop = verticalAngle/2;
    }

    private void Update(){
        Aim();
    }

    void Aim(){
        //yaw
        mouseInputY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        currentAngle += mouseInputY;
        currentAngle = Mathf.Clamp(currentAngle,clampLimitBottom, clampLimitTop);
        if(currentAngle >= clampLimitTop || currentAngle <= clampLimitBottom ){
            mouseInputY = 0f;
        }
        Vector3 lookDirection = transform.localEulerAngles;
        lookDirection.x = -currentAngle;
        lookDirection.y = lookDirection.z = 0f;
        transform.localEulerAngles = lookDirection;
        transform.Rotate(Vector3.left * mouseInputY);

        //pitch
        mouseInputX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        player.Rotate(Vector3.up * mouseInputX);
    }
}
