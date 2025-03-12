using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [Header("InvisibleArguments")]
    public Rigidbody myRigidbody;
    public Transform myTransform;
    public GroundCheck groundCheck;
    public Renderer myRenderer;
    
    [Header("Control")]
    public float moveSpeed;
    public float turnSpeed;
    public float jumpForce;



    private void LateUpdate() {
        CheckJump();
        CheckMove();
        // UpdateFace();
    }

    void CheckJump(){
        if(Input.GetButton("Jump") && groundCheck.isGround){
            myRigidbody.velocity = new Vector3(
                myRigidbody.velocity.x, 
                myRigidbody.velocity.y+ jumpForce, 
                myRigidbody.velocity.z );

        }
    }
    void CheckMove(){
        if(Input.GetAxisRaw("Vertical") >= 0.1){
            // myRigidbody.velocity = new Vector3(
            // myRigidbody.velocity.x, 
            // myRigidbody.velocity.y  , 
            // myRigidbody.velocity.z + Input.GetAxisRaw("Vertical") * moveSpeed);
            myTransform.position += transform.forward *moveSpeed * Time.deltaTime;
        }

        if(0.1 <= Input.GetAxisRaw("Horizontal") || Input.GetAxisRaw("Horizontal") <= 0.1){
            myTransform.localRotation *= Quaternion.AngleAxis(Input.GetAxisRaw("Horizontal") * turnSpeed * Time.deltaTime, Vector3.up);
        }
    }

    // void UpdateFace(){
    //     if(Input.GetKeyDown(KeyCode.Alpha1)){
    //         Debug.Log("True1");
    //         myRenderer.material = face.idle;
    //     }
    //     if(Input.GetKeyDown(KeyCode.Alpha2)){
    //         Debug.Log("True2");
    //         myRenderer.material = face.closeEye;
    //     }
    //     if(Input.GetKeyDown(KeyCode.Alpha3)){
    //         Debug.Log("True3");
    //         myRenderer.material = face.mad;
    //     }
    //     if(Input.GetKeyDown(KeyCode.Alpha4)){
    //         myRenderer.material = face.excited;
    //     }
    //     if(Input.GetKeyDown(KeyCode.Alpha5)){
    //         myRenderer.material = face.emmm;
    //     }
    //     if(Input.GetKeyDown(KeyCode.Alpha6)){
    //         myRenderer.material = face.confused;
    //     }
    // }

}
