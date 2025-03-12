using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "floorGameObject", menuName = "ScriptableObjects/FloorGameObject")]
public class FloorGameObject : ScriptableObject
{
    public GameObject gameObject;
    public string gameObjectName;
    public FloorState floorState;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;
    public int stateArea;
    public int leadArea;
    public FloorState daytimeFloorState;
    public FloorState nighttimeFloorState;
    // 
}

public enum FloorGameObjectType{
    Jukebox,
    StreetLight,
    BookShelf,
    Dumbbell
}
