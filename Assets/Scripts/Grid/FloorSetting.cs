using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorSetting", menuName = "ScriptableObjects/FloorSetting")]
public class FloorSetting: ScriptableObject
{
    public int MinX;
    public int MinY;
    public int MaxX;
    public int MaxY;

    public List<FloorObjectSetting> FloorObjects;
}

[Serializable]
public class FloorObjectSetting{
    public int X;
    public int Y;
    public FloorGameObjectType FloorGameObjectType;
}
