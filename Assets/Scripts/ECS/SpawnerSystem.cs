using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public partial struct SpawnerSystem : ISystem
{
    
    public void OnCreate(ref SystemState state) 
    { 
        state.RequireForUpdate<SpawnerConfig>();
    }

    public void OnUpdate(ref SystemState state)
    {
        // Debug.Log("GameDataCenter._FloorGridStorage  :  ", Resources.Load<FloorGridsStorage>("ScriptableObject/FloorGridStorage"));
        state.Enabled = false;
        // Debug.Log("GameDataCenter.GetScriptableObjectTest()"+ GameDataCenter.GetScriptableObjectTest().gameObjectName == null? GameDataCenter.GetScriptableObjectTest().gameObjectName : "Null");
        // Debug.Log("GameDataCenter._Jukebox" + GameDataCenter._Jukebox.gameObjectName == null? GameDataCenter._Jukebox.gameObjectName : "Null");
        // Debug.Log("Resources.Load<FloorGameObject>" + Resources.Load<FloorGameObject>("ScriptableObjects/FloorGameObject/Jukebox").gameObjectName == null? Resources.Load<FloorGameObject>("ScriptableObjects/FloorGameObject/Jukebox").gameObjectName : "Null");
        SpawnerConfig spawnerConfig = SystemAPI.GetSingleton<SpawnerConfig>();
        foreach(_FloorSetting floorSetting in GameDataCenter._WholeFloorSetting.wholeFloorSetting){
            for(int i= 0 ; i < floorSetting.amount; i++){
                // Debug.Log("SpawnerSystem.OnUpdate()");
                
                Entity spawnedEntity = state.EntityManager.Instantiate(spawnerConfig.SlimePrefab);
                state.EntityManager.AddComponent<SlimeComponent>(spawnedEntity);
                SystemAPI.SetComponent(spawnedEntity, new SlimeComponent 
                { 
                    RoomID = floorSetting.RoomID,
                    // CurrSlimeState = SlimeState.Idle,
                    MoveSpeed = GameDataCenter._SlimeMoveSpeed,
                    TurnSpeed = math.radians(GameDataCenter._SlimeTurnSpeed_Slow),
                    JumpForce = 0,
                    // CurrEmoji = Emoji.Idle,
                    Timer = 0,
                    isAvailable = true,
                    CurrState = SlimeState.Idle,
                    CurrSubState = SlimeSubState.Waiting,
                    TargetTransform = LocalTransform.Identity,
                    CurrColor = SlimeColor.Green,
                    RotateDirection = 0
                });
                SystemAPI.SetComponent(spawnedEntity, new LocalTransform{
                    Position = new float3(UnityEngine.Random.Range(floorSetting.floorSetting.MinX + floorSetting.x * 150 + 15, floorSetting.floorSetting.MaxX + floorSetting.x * 150 - 15), 0, UnityEngine.Random.Range(floorSetting.floorSetting.MinY + floorSetting.y * 150, floorSetting.floorSetting.MaxY + floorSetting.y * 150)),
                    Rotation = quaternion.identity,
                    Scale = 1f
                });
                // state.EntityManager.AddComponent<Disabled>(spawnedEntity);
            }
        }
        
    }
}


public struct SlimeComponent : IComponentData {
    // public SlimeState CurrSlimeState;
    public int RoomID;
    public float MoveSpeed;
    public float TurnSpeed;
    public float JumpForce;
    public float Timer;
    public bool isAvailable;
    public SlimeState CurrState;
    public SlimeValue CurrValue;
    public SlimeSubState CurrSubState;
    public LocalTransform TargetTransform;
    public SlimeColor CurrColor;
    public float RotateDirection;
}

public enum SlimeSubState{
    Moving,
    Rotating,
    Waiting,
    Idle
}

public enum SlimeColor{
    Green,
    Orange,
    Blue,
    Purple
}