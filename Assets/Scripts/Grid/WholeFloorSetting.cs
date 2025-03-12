using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WholeFloorSetting", menuName = "ScriptableObjects/WholeFloorSetting")]
public class WholeFloorSetting : ScriptableObject
{
    public List<_FloorSetting> wholeFloorSetting;
}

[Serializable]
public class _FloorSetting{
    public int RoomID;
    public int x;
    public int y;
    public int amount;
    public FloorSetting floorSetting;
}