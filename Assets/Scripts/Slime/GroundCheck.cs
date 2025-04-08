/// -------------------------------------------------------------------///
/// Script Documentation 
/// Check Slime's ground touching
/// -------------------------------------------------------------------///
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGround;

    private void OnCollisionEnter(Collision other) {
        isGround = true;
    }
    private void OnCollisionExit(Collision other) {
        isGround = false;
    }

    // private void OnTriggerEnter(Collider other) {
    //     isGround = true;

    // }
    // private void OnTriggerExit(Collider other) {
    //     isGround = false;

    // }
}
