using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnvironmentObject : MonoBehaviour
{
    public void OnDayNightChange(DayNight daynight){
        Debug.Log(daynight);
        if(daynight == DayNight.Day){
            ChangeToDay();
        }else{
            ChangeToNight();
        }
    }

    abstract protected void ChangeToDay();
    abstract protected void ChangeToNight();


}
