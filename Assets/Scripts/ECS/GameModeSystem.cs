using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Unity.Mathematics;

public struct SlimeECSProperty{
    LocalTransform transform;
    SlimeComponent slimeComponent;
}

public partial struct GameModeSystem : ISystem
{
    public NativeQueue<SlimeECSProperty> SpawnSlimeEventQueue;
    public void OnCreate(ref SystemState state) 
    { 
        // EventCenter.Instance.AddEventListener(EventType.ChangeGameModeToExplore, ChangeGameModeToExplore);
        // EventCenter.Instance.AddEventListener(EventType.ChangeGameModeToInspect, ChangeGameModeToInspect);
        UnityEngine.Debug.Log("GameModeSystem OnCreate");
        EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Persistent)
            .WithAny<ChangeGameModeToExploreEventComponent>()
            .WithAny<ChangeGameModeToInspectEventComponent>();
        EntityQuery query = state.EntityManager.CreateEntityQuery(builder);
        state.RequireAnyForUpdate(query); 
        SpawnSlimeEventQueue = new NativeQueue<SlimeECSProperty>(Allocator.Persistent);
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        UnityEngine.Debug.Log("GameModeSystem Onupdate");
        int selectedRoomID = GameManager.Instance.SelectedRoom.myRoomID;
        // new 
        foreach ( var (temp, eventEntity) in SystemAPI.Query<ChangeGameModeToExploreEventComponent>().WithEntityAccess()){
            UnityEngine.Debug.Log("GameModeSystem Onupdate - ChangeGameModeToExplore");
            foreach ( var( slime, transform, entity ) in
             SystemAPI.Query<RefRO<SlimeComponent>, RefRO<LocalTransform>>().WithAll<SlimeComponent>().WithEntityAccess()){
                if(slime.ValueRO.RoomID != selectedRoomID){
                    continue;
                }
                Debug.Log("GameModeSystem Onupdate - Add Hidden and DisableRendering");
                // transform.ValueRW.Position = new float3(0,-100,0);
                // ecb.AddComponent<Disabled>(entity);
                ecb.DestroyEntity(entity);
                GameManager.CreateOOPGameObject(slime, transform);
            }
            // foreach ( var( slime, transform, entity ) in
            //  SystemAPI.Query<RefRO<SlimeComponent>, RefRW<LocalTransform>>().WithAll<SlimeComponent>().WithEntityAccess()){
            //     // UnityEngine.Debug.Log("GameModeSystem Onupdate - Add Hidden and DisableRendering");
            //     // transform.ValueRW.Position = new float3(0,-100,0);
            //     ecb.AddComponent<Disabled>(entity);
            //     GameManager.CreateOOPGameObject(slime);
            // }
            ecb.RemoveComponent<ChangeGameModeToExploreEventComponent>(eventEntity);
            EventCenter.Instance.BoardcastEvent(EventType.DoneChangeGameModeToExplore);
        }
        foreach ( var (temp, eventEntity) in SystemAPI.Query<ChangeGameModeToInspectEventComponent>().WithEntityAccess()){
            UnityEngine.Debug.Log("GameModeSystem Onupdate - ChangeGameModeToInspect");
            ecb.RemoveComponent<ChangeGameModeToInspectEventComponent>(eventEntity);
            SpawnerConfig spawnerConfig = SystemAPI.GetSingleton<SpawnerConfig>();
            foreach(GameObject slimeGameObject in GameObject.FindGameObjectsWithTag("SlimeProperty")){
                SlimeProperty slimeProperty = slimeGameObject.GetComponent<SlimeProperty>();
                if (slimeProperty == null)
                {
                    Debug.LogError($"GameObject {slimeGameObject.name} does not have a SlimeProperty component!");
                    continue; // Skip this GameObject if it doesn't have the required component
                }
                Entity spawnedEntity = ecb.Instantiate(spawnerConfig.SlimePrefab);
                ecb.AddComponent<SlimeComponent>(spawnedEntity);
                Debug.Log(slimeProperty);
                Debug.Log(spawnedEntity);
                ecb.SetComponent(spawnedEntity, new SlimeComponent 
                { 
                    // CurrSlimeState = SlimeState.Idle,
                    MoveSpeed = GameDataCenter._SlimeMoveSpeed,
                    TurnSpeed = math.radians(GameDataCenter._SlimeTurnSpeed_Slow),
                    JumpForce = 0,
                    // CurrEmoji = Emoji.Idle,
                    Timer = 0,
                    isAvailable = true,
                    // CurrState = slimeGameObject.GetComponent<SlimeProperty>().slimeState,   //Change
                    CurrState = slimeProperty.slimeState,
                    CurrValue = slimeProperty.slimeValue,
                    CurrSubState = SlimeSubState.Waiting,
                    TargetTransform = LocalTransform.Identity,
                    RotateDirection = 0
                });
                ecb.SetComponent(spawnedEntity, new LocalTransform{
                    Position = slimeGameObject.transform.position,
                    Rotation = quaternion.identity,
                    Scale = 1f
                });
                Object.Destroy(slimeGameObject);
            }
        }
        EventCenter.Instance.BoardcastEvent(EventType.DoneChangeGameModeToInspect);
        ecb.Playback(state.EntityManager);
        state.Dependency.Complete();
        
        
        // TempJob temp = new TempJob {
        //     ECB = ecb
        // };
        // temp.Schedule();
        // UnityEngine.Debug.Log("GameModeSystem JobDone");
        // state.EntityManager.RemoveComponent<ChangeGameModeToExploreEventComponent>(EventQuery);
        // return;
        // }

        // foreach ( var temp in SystemAPI.Query<ChangeGameModeToInspectEventComponent>()){
        //     foreach ( var( slime, entity ) in
        //      SystemAPI.Query<RefRO<SlimeComponent>>().WithAll<SlimeComponent>().WithAll<DisableRendering>().WithAll<Hidden>().WithEntityAccess()){
        //         state.EntityManager.RemoveComponent<Hidden>(entity);
        //         state.EntityManager.RemoveComponent<DisableRendering>(entity);
        //     }
        //     state.EntityManager.RemoveComponent<ChangeGameModeToInspectEventComponent>(EventQuery);
        // }

        

    }
    

    // [BurstCompile]
    // // [WithAll(typeof(SlimeComponent))]
    // public partial struct ChangeGameModeToExploreJob: IJobEntity {
    //     public EntityManager EntityManager;
    //     public void Execute(ref SlimeComponent slime, Entity entity)
    //     {
    //         // entityManager.RemoveComponent(entity, ActiveInECS);
    //         EntityManager.AddComponent<Hidden>(entity);
    //         EntityManager.AddComponent<DisableRendering>(entity);
    //         // GameManager.CreateOOPGameObject(slime);
    //     }
    // }

    // [BurstCompile]
    // // [WithAll(typeof(SlimeComponent))]
    // public partial struct ChangeGameModeToInspectJob: IJobEntity {
    //     public EntityManager EntityManager;
    //     public void Execute(ref SlimeComponent slime, Entity entity)
    //     {
    //         // entityManager.RemoveComponent(entity, ActiveInECS);
    //         EntityManager.RemoveComponent<Hidden>(entity);
    //         EntityManager.RemoveComponent<DisableRendering>(entity);
    //     }
    // }

    public partial struct TempJob : IJobEntity {
        public EntityCommandBuffer ECB;
        public void Execute(ChangeGameModeToExploreEventComponent c, Entity entity)
        {
            UnityEngine.Debug.Log("TempJobExecute");
        }
    }

    
}
public struct ChangeGameModeToExploreEventComponent : IComponentData {}
public struct ChangeGameModeToInspectEventComponent : IComponentData {}