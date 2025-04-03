/// -------------------------------------------------------------------///
/// Script Documentation 
/// Store all the event about change in day or night for all object in the game world.
/// -------------------------------------------------------------------///
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Game Event")]
public class DayNightEvent : ScriptableObject
{
    [SerializeField] private List<EnvironmentObject> environmentListeners = new List<EnvironmentObject>();
    [SerializeField] private List<FloorGrid> listeners = new List<FloorGrid>();
    private List<GridDatum> datalisteners = new List<GridDatum>();
    
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
        // for(int i = datalisteners.Count - 1; i >= 0; i--)
        // {
        //     datalisteners[i].OnDayNightChange(dayNight);
        // }
    }

    /// <summary>
    /// Add Listener to environmentListeners
    /// </summary>
    /// <param name="listener"></param>
    public void AddListener(FloorGrid listener)
    {
        if(!listeners.Contains(listener)){
            listeners.Add(listener);
        } 
    }


    /// <summary>
    /// Add Listener to environmentListeners
    /// </summary>
    /// <param name="listener"></param>
    public void AddListener(EnvironmentObject listener){
        if(!environmentListeners.Contains(listener)){
            environmentListeners.Add(listener);
        } 
    }

    public void AddListener(GridDatum listener)
    {
        if(!datalisteners.Contains(listener)){
            datalisteners.Add(listener);
        } 
    }

    /// <summary>
    /// Remove Listener
    /// </summary>
    /// <param name="listener"></param>
    public void RemoveListener(FloorGrid listener)
    {
        listeners.Remove(listener);
    }
    /// <summary>
    /// Remove Listener
    /// </summary>
    /// <param name="listener"></param>
    public void RemoveListener(EnvironmentObject listener){
        environmentListeners.Remove(listener);
    }

    public void RemoveListener(GridDatum listener)
    {
        datalisteners.Remove(listener);
    }

    /// <summary>
    /// Remove all the listener
    /// </summary>
    public void ClearEventListeners(){
        listeners.Clear();
        environmentListeners.Clear();
        datalisteners.Clear();
    }
}