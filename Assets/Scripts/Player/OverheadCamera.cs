/// -------------------------------------------------------------------///
/// Script Documentation 
/// Camera control for player in inspect mode.  
/// -------------------------------------------------------------------///
using UnityEngine;

public class OverheadCamera : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float rotationSpeed = 100f;

    void Update()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime);

    }
}
