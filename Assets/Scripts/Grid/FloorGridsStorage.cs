using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


[Serializable]
public class FloorGrid{
    public Vector3Int cellPosition;
    public FloorState floorState;
    public Quaternion direction;
    public GameObject debug;
    public Material dayTimeDebugMaterial;
    public Material nightTimeDebugMaterial;
    public DayNightFloorState dayNightFloorState;
    public FloorGrid(Vector3Int cellPosition, FloorState floorState, Quaternion direction, GameObject debug){
        this.cellPosition = cellPosition;
        this.floorState = floorState;
        this.direction = direction;
        this.debug = debug;
        dayNightFloorState.dayTimeFloorState = FloorState.Idle;
        dayNightFloorState.nightTimeFloorState = FloorState.Idle;
    }

    public void SetFloorState(FloorState floorState){
        this.floorState = floorState;
    }
    public void SetFloorState(FloorState dayTimeFloorState, FloorState nightTimeFloorState){
        floorState = dayTimeFloorState;
        this.dayNightFloorState.dayTimeFloorState = dayTimeFloorState;
        this.dayNightFloorState.nightTimeFloorState = nightTimeFloorState;
    }
    public void SetDirection(Quaternion direction){
        this.direction = direction;
    }

    public void OnDayNightChange(DayNight daynight){
        // Debug.Log(cellPosition.ToString() + "Day Night Change, now " + daynight.ToString());
        floorState = daynight == DayNight.Day? dayNightFloorState.dayTimeFloorState : dayNightFloorState.nightTimeFloorState;
        debug.GetComponent<Renderer>().material = daynight == DayNight.Day ? dayTimeDebugMaterial : nightTimeDebugMaterial;
    }
}

[CreateAssetMenu(fileName = "storage", menuName = "ScriptableObjects/FloorStatesStorage")]
public class FloorGridsStorage : ScriptableObject
{
    Dictionary<Vector3Int, FloorGrid> floorStates = new Dictionary<Vector3Int, FloorGrid>();
    [SerializeField] List<FloorGrid> floorStates_List = new List<FloorGrid>();


    public void AddFloorGrid(Vector3Int cellPosition, GameObject debug){
        FloorGrid newFloorGrid = new FloorGrid(cellPosition, FloorState.Idle, Quaternion.identity, debug);
        floorStates.Add(cellPosition, newFloorGrid);
    }
    public void AddFloorGridToDayNightEventListener(Vector3Int cellPosition, DayNightEvent daynightEvent){
        daynightEvent.AddListener(GetFloorGrid(cellPosition));
    }
    public void RemoveFloorGridToDayNightEventListener(Vector3Int cellPosition, DayNightEvent daynightEvent){
        daynightEvent.RemoveListener(GetFloorGrid(cellPosition));
    }
    public FloorGrid GetFloorGrid(Vector3Int cellPosition){
        return floorStates[cellPosition];
    }

    public FloorState GetFloorState(Vector3Int cellPosition){
        return floorStates[cellPosition].floorState;
    }
    public DayNightFloorState GetDayNightFloorState(Vector3Int cellPosition){
        return floorStates[cellPosition].dayNightFloorState;
    }
    public Quaternion GetFloorDirection(Vector3Int cellPosition){
        return floorStates[cellPosition].direction;
    }
    
    public void SetFloorState(Vector3Int cellPosition, FloorState state){
        floorStates[cellPosition].floorState = state;
        floorStates[cellPosition].direction = Quaternion.identity;
    }
    public void SetFloorState(Vector3Int cellPosition, FloorState state, Vector3 direction){
        floorStates[cellPosition].floorState = state;
        // Debug.Log(cellPosition + " " + floorStates[cellPosition].floorState);
        floorStates[cellPosition].dayNightFloorState.dayTimeFloorState = state;
        floorStates[cellPosition].dayNightFloorState.nightTimeFloorState = state;
        floorStates[cellPosition].direction = Quaternion.LookRotation(direction);
    }
    public void SetFloorState(Vector3Int cellPosition, FloorState dayTimeState, FloorState nightTimeState){
        floorStates[cellPosition].floorState = dayTimeState;
        floorStates[cellPosition].dayNightFloorState.dayTimeFloorState = dayTimeState;
        floorStates[cellPosition].dayNightFloorState.nightTimeFloorState = nightTimeState;
        floorStates[cellPosition].direction = Quaternion.identity;
    }
    public void SetFloorState(Vector3Int cellPosition, FloorState dayTimeState, FloorState nightTimeState, Vector3 direction){
        floorStates[cellPosition].floorState = dayTimeState;
        floorStates[cellPosition].dayNightFloorState.dayTimeFloorState = dayTimeState;
        floorStates[cellPosition].dayNightFloorState.nightTimeFloorState = nightTimeState;
        floorStates[cellPosition].direction = Quaternion.LookRotation(direction);
    }

    public void SetFloorDebug(Vector3Int cellPosition, Material material){
        floorStates[cellPosition].debug.GetComponent<Renderer>().material = material;
        // if(floorStates[cellPosition].floorState == FloorState.Move){
            floorStates[cellPosition].debug.transform.rotation 
                = GetFloorDirection(cellPosition) * Quaternion.Euler(0, 180f, 0);
        // }s
        
    }
    public void SetFloorDebug(Vector3Int cellPosition, Material dayTimeMaterial, Material nightTimeMaterial){
        floorStates[cellPosition].debug.GetComponent<Renderer>().material = dayTimeMaterial;
        floorStates[cellPosition].dayTimeDebugMaterial = dayTimeMaterial;
        floorStates[cellPosition].nightTimeDebugMaterial = nightTimeMaterial;
        // if(floorStates[cellPosition].floorState == FloorState.Move){
            floorStates[cellPosition].debug.transform.rotation 
                = GetFloorDirection(cellPosition) * Quaternion.Euler(0, 180f, 0);
        // }s
        
    }

    public void ActiveDebug(Vector3Int cellPosition, bool isActive){
        floorStates[cellPosition].debug.SetActive(isActive);
    }

    public void DictionaryToList(){
        foreach(KeyValuePair<Vector3Int, FloorGrid> entry in floorStates){
            floorStates_List.Add(entry.Value);
        }
    }


    public Dictionary<float2,DayNightFloorState>  GetProcessedDayNightFloorStates(){
        Dictionary<float2,DayNightFloorState> m_dict = new Dictionary<float2,DayNightFloorState>();
        for(int i = 0; i < 100; i++){
            for(int j = 0; j < 100; j++){
                m_dict[new float2(i, j)] = floorStates[new Vector3Int(i, j, 0)].dayNightFloorState;
            }
        }
        return m_dict;
    }
}





public enum FloorState{
    Move,
    Idle,
    Music,
    Read,
    Gym
}

public struct DayNightFloorState{
    public FloorState dayTimeFloorState;
    public FloorState nightTimeFloorState;
}