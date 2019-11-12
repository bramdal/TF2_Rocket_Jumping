using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //physical attributes
    public float mass = 1f;

    //horizontals
    public Vector3 movementDirection = Vector3.zero;
    public float movementSpeed;
    
    float inputX;
    float inputZ;

    //verticals
    public float gravity;
    public float jumpForce;
    float verticalVelocity = 0f;
    Vector3 slopeGravity;
    //crouch
    public float crouchDistance;
    Vector3 standPosition;
    Vector3 crouchPosition;

    //impact 
    Vector3 impactVector = Vector3.zero;
    bool isFlying = false;
    //states
    public bool crouched = false;
    public bool walking = false;
    public bool airtime = false;
    bool jumping = false;

    Transform head;
    CharacterController characterController;
    Camera cam;

    private void Start() {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        
        head = GameObject.Find("Head").transform;
        standPosition = crouchPosition = head.localPosition;
        crouchPosition.y -= crouchDistance;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update(){
        GetMovements(); 
    }

    void GetMovements(){
        if(Input.GetButton("Crouch"))
            crouched = true;            
        else
            crouched = false;
        if(crouched){
            crouched = true;
            head.localPosition = Vector3.MoveTowards(head.localPosition, crouchPosition, 0.1f);
        }
        else{
            crouched = false;
            head.localPosition = Vector3.MoveTowards(head.localPosition, standPosition, 0.1f);
        }

        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        movementDirection = inputZ * camForward + inputX * camRight;

        if(characterController.isGrounded){
            if(isFlying)
                impactVector = Vector3.zero;   
            isFlying = false;
            jumping = false;
            
            if(CheckSlope())
                verticalVelocity = -gravity * Time.deltaTime * 100f; // compensate for slopes; use slope check to extend functionality
            else
                verticalVelocity = -gravity * Time.deltaTime;
            if(Input.GetButtonDown("Jump")){
                verticalVelocity = jumpForce;
                jumping = true;
            }
            if(crouched)
                movementDirection *= movementSpeed /  (2f * mass);
            else
                movementDirection *= movementSpeed / mass;    
            movementDirection.y = verticalVelocity;
        }
        else if(jumping){
            movementDirection *= movementSpeed /  (mass);
            verticalVelocity -= gravity * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else if(isFlying && !jumping){
            if(verticalVelocity>0){
                movementDirection *= movementSpeed /  (2f * mass);
            }    
            else{
                movementDirection *= movementSpeed / (1.5f * mass);
            }        
            
            verticalVelocity -= gravity * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else{
            verticalVelocity -= gravity * Time.deltaTime;
            movementDirection *= movementSpeed /  (mass);
            movementDirection.y = verticalVelocity;
        }

        movementDirection += impactVector;
        characterController.Move(movementDirection * Time.deltaTime);
    }

    bool CheckSlope(){
        RaycastHit rayOut; 
        bool checkRayHit = Physics.Raycast(transform.position, Vector3.down, out rayOut, 2f); // hardcoded to player's height; add variable later
        if(checkRayHit && rayOut.normal != Vector3.up)
            return true;    
        return false;
    }
    public void AddImpact(float impactForce, Vector3 direction){
        impactVector = Vector3.zero;
        verticalVelocity = impactForce;
        isFlying = true;
        jumping = false;
        impactVector = direction;
        impactVector.Normalize();
        impactVector.y = 0f;
        if(crouched){
            impactVector *= impactForce * Mathf.Sin(Vector3.Angle(transform.up, direction)) / mass;
            verticalVelocity = impactForce / (0.5f * mass);
        }    
        else{
           impactVector *= impactForce * Mathf.Sin(Vector3.Angle(transform.up, direction)) / (0.5f * mass); 
           verticalVelocity = impactForce / mass;
        }    
    }

    
}
