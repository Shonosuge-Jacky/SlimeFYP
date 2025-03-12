using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    public float rotationSpeed = 30f; // Speed of rotation in degrees per second
    private float currentYRotation = 0f; // Current Y rotation

    void Update()
    {
        // Calculate the rotation for this frame
        currentYRotation += rotationSpeed * Time.deltaTime;

        // If the rotation exceeds 360 degrees, reset it to 0
        if (currentYRotation >= 360f)
        {
            currentYRotation = 0f;
        }

        // Apply the rotation to the light
        transform.rotation = Quaternion.Euler(40f, currentYRotation, -22f);
    }
}
