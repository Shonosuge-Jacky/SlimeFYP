/// -------------------------------------------------------------------///
/// Script Documentation 
/// Store all the general events for all object, including the event call for ECS and OOP system to communicate.
/// Allow Calling event with parameters.
/// -------------------------------------------------------------------///
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine.Events;

public interface IEventInfo
{

}

public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo( UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

public enum EventType
{
    ChangeGameModeToInspect,
    ChangeGameModeToExplore,
    DoneChangeGameModeToInspect,
    DoneChangeGameModeToExplore,
    UpdateValueEvent
}

/// <summary>
/// Event Center
/// </summary>
public class EventCenter
{
    /// <summary>
    /// Singleton for Event Center
    /// </summary>
    private static EventCenter _Instance;
    
    public static EventCenter Instance{
        get
        {
            if(_Instance == null)
            {
                _Instance = new EventCenter();
            }    
            return _Instance;
        }
        
    }

    /// <summary>
    /// Communication Pipeline for ECS system specifically.
    /// Whenever OOP system want to send event to ECS system, it will be sent through a speicific pipeline.
    /// This pipeline will instantiate a specfic entity with Event Componenet in the ECS system.
    /// </summary>
    private Entity ECSCommunicationPipeline{
        get
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entity = entityManager.CreateEntityQuery(typeof(SpawnerConfig)).GetSingleton<SpawnerConfig>();
            Entity ECSCommunicationPipeline = entityManager.Instantiate(entity.EmptyPrefab);
            return ECSCommunicationPipeline;
        }
    }


    private Dictionary<EventType, IEventInfo> EventDictionary = new Dictionary<EventType, IEventInfo>();

    /// <summary>
    /// Add Listener, with parameter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="action"></param>
    public void AddEventListener<T>(EventType type, UnityAction<T> action)
    {
        if( EventDictionary.ContainsKey(type))
        {
            ((EventInfo<T>)EventDictionary[type]).actions += action;
        }else
        {
            EventDictionary.Add(type, new EventInfo<T>( action ));
        }
    }

    /// <summary>
    /// Add Listener, without parameter
    /// </summary>
    /// <param name="type"></param>
    /// <param name="action"></param>
    public void AddEventListener(EventType type, UnityAction action)
    {
        if( EventDictionary.ContainsKey(type))
        {
            ((EventInfo)EventDictionary[type]).actions += action;
        }else
        {
            EventDictionary.Add(type, new EventInfo( action ));
        }
    }

    /// <summary>
    /// Remove Event Listener
    /// </summary>
    /// <param name="type"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(EventType type, UnityAction action)
    {
        if( EventDictionary.ContainsKey(type))
        {
            ((EventInfo)EventDictionary[type]).actions -= action;
        }
    }

    /// <summary>
    /// Boardcast Event, with parameter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <param name="parameter"></param>
    public void BoardcastEvent<T>(EventType type, T parameter)
    {
        if (EventDictionary.ContainsKey(type))
        {
            if(((EventInfo<T>)EventDictionary[type]).actions != null)
                ((EventInfo<T>)EventDictionary[type]).actions.Invoke(parameter);
        }
    }

    /// <summary>
    /// Boardcast Event
    /// </summary>
    /// <param name="type"></param>
    public void BoardcastEvent(EventType type)
    {
        if (EventDictionary.ContainsKey(type))
        {
            if(((EventInfo)EventDictionary[type]).actions != null)
                ((EventInfo)EventDictionary[type]).actions.Invoke();
        }
    }

    /// <summary>
    /// Clear all stored event and their listener.
    /// </summary>
    public void ClearEventListeners()
    {
        EventDictionary.Clear();
    }

    /// <summary>
    /// Clear all event of a specific event
    /// </summary>
    /// <param name="type"></param>
    public void ClearEventListener(EventType type)
    {
        if( EventDictionary.ContainsKey(type))
        {
            EventDictionary.Remove(type);
        }
    }

    /// <summary>
    /// Function for sending event to ECS system.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public IEnumerator SendEventToECS(EventType type){
        EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
        if(type == EventType.ChangeGameModeToExplore){
            em.AddComponent<ChangeGameModeToExploreEventComponent>(ECSCommunicationPipeline);
        }else{
            em.AddComponent<ChangeGameModeToInspectEventComponent>(ECSCommunicationPipeline);
        }
        
        yield return null;
    }
}