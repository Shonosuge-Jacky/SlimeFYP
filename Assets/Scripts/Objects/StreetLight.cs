using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLight : EnvironmentObject
{
    public GameObject spotLight;
    public GameObject lightCone;
    protected override void ChangeToDay()
    {
        spotLight.SetActive(false);
        lightCone.SetActive(false);
    }

    protected override void ChangeToNight()
    {
        spotLight.SetActive(true);
        lightCone.SetActive(true);
    }
}
