using System;
using UnityEngine;
using UnityEngine.UI;

public enum DayNight{
    Day,
    Night,
}
public enum AMPM{
    AM,
    PM
}

[Serializable]
public struct TimeVariable{
    public DayNightEvent daynightEvent;
    public DayNight currDayNight;
    public int currHour_12;
    public float currMinutes;
    public AMPM currAMPM;
    public int currHour_24;
    public float currHourMin_24;

    public void ClearEventListeners(){
        daynightEvent.ClearEventListeners();
    }

    public void AddMinutes(float val){
        currMinutes += val;
        if(currMinutes > 60){
            currMinutes = 0;
            AddHour();
        }
    }
    void AddHour(){
        if(currHour_12 == 11){
            currAMPM = currAMPM == AMPM.AM? AMPM.PM : AMPM.AM;
        }
        if(currHour_12 == 5){
            currDayNight = currDayNight == DayNight.Day? DayNight.Night : DayNight.Day;
            Debug.Log("TriggerDayNightChangeEvent " + currDayNight);
            daynightEvent.TriggerDayNightChangeEvent(currDayNight);
        }
        currHour_12 = currHour_12 == 12? 1 : currHour_12 + 1;

        currHour_24 = currHour_24 == 23? 0 : currHour_24 + 1;
        
    }
}

public class TimeManager : MonoBehaviour
{
    public TimeVariable currTime;
    public Light gamelight;
    public float timeSpeed;
    public float nightLightIntensity = 0.43f;
    public float dayLightIntensity = 1f;
    
    float differLightIntensity;
    float xRotation;

    private void Awake() {
        differLightIntensity = dayLightIntensity - nightLightIntensity;
        currTime.ClearEventListeners();
    }
    void Update()
    {
        UpdateTimeSystem();
        
    }
    
    void UpdateTimeSystem(){
        currTime.AddMinutes(timeSpeed * Time.deltaTime);
        currTime.currHourMin_24 = currTime.currHour_24 + currTime.currMinutes / 60f;
        
        xRotation = currTime.currHourMin_24 / 24f * 360 - 90;
        gamelight.transform.localRotation = Quaternion.Euler(xRotation, 45f, 0f);


        if(currTime.currHour_24 < 5){
            gamelight.intensity = nightLightIntensity;
        }else if(5 <= currTime.currHour_24 && currTime.currHour_24 < 9){
            gamelight.intensity = (currTime.currHourMin_24 - 5) / 4f  * differLightIntensity + nightLightIntensity;
        }else if(9 <= currTime.currHour_24 && currTime.currHour_24 < 17){
            gamelight.intensity = dayLightIntensity;
        }else if(17 <= currTime.currHour_24 && currTime.currHour_24 < 21){
            gamelight.intensity = (21 - currTime.currHourMin_24) / 4f  * differLightIntensity + nightLightIntensity;
        }else{
            gamelight.intensity = nightLightIntensity;
        }

        GameManager.Instance.UIManager.clock.text = currTime.currHour_24.ToString().PadLeft(2, '0') + ":" + ((int)currTime.currMinutes).ToString().PadLeft(2, '0');
        
    }

    public void ChangeTimeSpeed(Slider timeSpeedSlider){
        timeSpeed = timeSpeedSlider.value;
    }
}
