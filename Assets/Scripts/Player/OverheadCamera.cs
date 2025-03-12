using UnityEngine;

public class OverheadCamera : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float rotationSpeed = 100f;

    void Update()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime);
        // // Mouse movement for camera translation
        // float h = Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
        // float v = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;

        // transform.Translate(h,0,v);

        // // Keyboard input for camera rotation
        // if (Input.GetKey(KeyCode.Q))
        // {
        //     transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        // }
        // if (Input.GetKey(KeyCode.E))
        // {
        //     transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        // }
    }
}
