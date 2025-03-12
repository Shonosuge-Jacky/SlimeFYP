using UnityEngine;
using System;

public class ScreenShotCamera : MonoBehaviour
{
    public KeyCode key;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(key)){
            Debug.Log("ScreenShot!");
            ScreenCapture.CaptureScreenshot("ScreenShot/screenshot-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png");
        }
    }
}
