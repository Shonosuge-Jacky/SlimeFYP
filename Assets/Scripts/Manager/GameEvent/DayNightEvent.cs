using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Game Event")]
public class DayNightEvent : ScriptableObject
{
    [SerializeField] private List<EnvironmentObject> environmentListeners = new List<EnvironmentObject>();
    [SerializeField] private List<FloorGrid> listeners = new List<FloorGrid>();
    
    //TriggerEvent for Day Night Trigger
    public void TriggerDayNightChangeEvent(DayNight dayNight)
    {
        for (int i = listeners.Count -1; i >= 0; i--)
        {
            listeners[i].OnDayNightChange(dayNight);
        }
        for (int i = environmentListeners.Count -1; i >= 0; i--)
        {
            environmentListeners[i].OnDayNightChange(dayNight);
        }
    }

    public void AddListener(FloorGrid listener)
    {
        if(!listeners.Contains(listener)){
            listeners.Add(listener);
        } 
    }
    public void AddListener(EnvironmentObject listener){
        if(!environmentListeners.Contains(listener)){
            environmentListeners.Add(listener);
        } 
    }

    public void RemoveListener(FloorGrid listener)
    {
        listeners.Remove(listener);
    }
    public void RemoveListener(EnvironmentObject listener){
        environmentListeners.Remove(listener);
    }

    public void ClearEventListeners(){
        listeners.Clear();
        environmentListeners.Clear();
    }
}