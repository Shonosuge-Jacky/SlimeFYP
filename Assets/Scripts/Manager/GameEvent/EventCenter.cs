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

public class EventCenter
{
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

    public void RemoveEventListener(EventType type, UnityAction action)
    {
        if( EventDictionary.ContainsKey(type))
        {
            ((EventInfo)EventDictionary[type]).actions -= action;
        }
    }

    public void BoardcastEvent<T>(EventType type, T parameter)
    {
        if (EventDictionary.ContainsKey(type))
        {
            if(((EventInfo<T>)EventDictionary[type]).actions != null)
                ((EventInfo<T>)EventDictionary[type]).actions.Invoke(parameter);
        }
    }
    public void BoardcastEvent(EventType type)
    {
        if (EventDictionary.ContainsKey(type))
        {
            if(((EventInfo)EventDictionary[type]).actions != null)
                ((EventInfo)EventDictionary[type]).actions.Invoke();
        }
    }

    public void ClearEventListeners()
    {
        EventDictionary.Clear();
    }

    public void ClearEventListener(EventType type)
    {
        if( EventDictionary.ContainsKey(type))
        {
            EventDictionary.Remove(type);
        }
    }

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