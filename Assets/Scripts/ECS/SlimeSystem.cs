using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct SlimeSystem : ISystem
{
    
    public void OnCreate(ref SystemState state) { 
        state.RequireForUpdate<SlimeComponent>();  //make sure system OnUpdate will only run if there is at least 1 Entity with RotateSpeed Component
        state.RequireForUpdate<GridData>();
        // state.Enabled = false;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state) { 
        // state.Enabled = false;
        // return;
        // UnityEngine.Debug.Log("SlimeSystem.Update");
        GridData _GridData = SystemAPI.GetSingleton<GridData>();
        // foreach (var (slime, localTransform) in
        //              SystemAPI.Query<RefRO<SlimeComponent>, RefRO<LocalTransform>>())
        // {
            // UnityEngine.Debug.Log("Create new SlimeJOb");
        Unity.Mathematics.Random random = Unity.Mathematics.Random.CreateFromIndex(state.GlobalSystemVersion);
        new SlimeAssignJob
        {
            Int2ToFloorState = _GridData.Int2ToFloorState,
            Random = random
        }.ScheduleParallel();
        new SlimeJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            Random = random
            // SlimePosition = new int2(new float2(localTransform.ValueRO.Position.x,localTransform.ValueRO.Position.z)),
        }.ScheduleParallel();
        // }
    }

    [BurstCompile]
    // [WithAll(typeof(SlimeComponent))]
    public partial struct SlimeAssignJob: IJobEntity {
        [ReadOnly] public NativeHashMap<int2, GridDatum> Int2ToFloorState;
        public Unity.Mathematics.Random Random;
        // public float deltaTime;

        public void Execute(ref SlimeComponent slime, ref LocalTransform transform)
        {
            // Debug.Log(slime.CurrSubState);
            // Debug.Log(slime.TurnSpeed);
            if (slime.CurrSubState != SlimeSubState.Waiting){
                return;
            }
            slime.isAvailable = false;
            GridDatum floorDatum = Int2ToFloorState[new int2(new float2(transform.Position.x, transform.Position.z))];
            switch(floorDatum.State){
                default:
                    break;
                case FloorState.Move:
                    // Debug.Log("SlimeAssignJob-Rotate");
                    slime.CurrSubState = SlimeSubState.Rotating;
                    // Debug.Log(floorDatum.Direction);
                    slime.TargetTransform = LocalTransform.FromPositionRotation(transform.Position + math.mul(floorDatum.Direction, new float3(0, 0, Random.NextFloat(2f,4f))), floorDatum.Direction);
                    // Debug.Log("1?-1?"+ transform.Rotation + " . " + slime.TargetTransform.Rotation + " . " +  (transform.Rotation.value.y + slime.TargetTransform.Rotation.value.y) + " . " +Quaternion.RotateTowards(transform.Rotation, slime.TargetTransform.Rotation, 1f) + " . " + Quaternion.Dot(transform.Rotation, slime.TargetTransform.Rotation) + " . " + Quaternion.Angle(transform.Rotation, slime.TargetTransform.Rotation));
                    // Debug.Log("!!!: " + transform.Rotation + " . " +  IsWithinRange(transform.Rotation.value.y, -1*slime.TargetTransform.Rotation.value.y));
                    slime.RotateDirection = IsWithinRange(transform.Rotation.value.y, -1*slime.TargetTransform.Rotation.value.y);
                    // Debug.Log("slimeTargetRotation" + slime.TargetTransform.Rotation);
                    slime.CurrState = SlimeState.Idle;
                    break;
                case FloorState.Music:
                    // Debug.Log("Music!");
                    slime.CurrSubState = SlimeSubState.Idle;
                    slime.CurrState = SlimeState.Music;
                    // transform.Position = new float3(transform.Position.x, 5, transform.Position.z);
                    break;
                case FloorState.Read:
                    slime.CurrSubState = SlimeSubState.Idle;
                    slime.CurrState = SlimeState.Read;
                    break;
                case FloorState.Gym:
                    slime.CurrSubState  = SlimeSubState.Idle;
                    slime.CurrState = SlimeState.Gym;
                    break;
                case FloorState.Idle:
                    // Debug.Log("SlimeAssignJob-Rotate");
                    slime.CurrSubState = SlimeSubState.Rotating;
                    slime.CurrState = SlimeState.Idle;
                    Quaternion randomDirection = Quaternion.LookRotation(new Vector3(Random.NextFloat(-1f, 1f), 0, Random.NextFloat(-1f,1f)));
                    slime.TargetTransform = LocalTransform.FromPositionRotation(transform.Position + math.mul(randomDirection, new float3(0, 0, Random.NextFloat(3f, 12f))), randomDirection);
                    // Debug.Log("1?-1?"+ transform.Rotation + " . " + slime.TargetTransform.Rotation + " . " +  (transform.Rotation.value.y + slime.TargetTransform.Rotation.value.y) + " . " +Quaternion.RotateTowards(transform.Rotation, slime.TargetTransform.Rotation, 1f) + " . " + Quaternion.Dot(transform.Rotation, slime.TargetTransform.Rotation) + " . " + Quaternion.Angle(transform.Rotation, slime.TargetTransform.Rotation));
                    // Debug.Log("!!!: " + transform.Rotation + " . " + );
                    slime.RotateDirection = IsWithinRange(transform.Rotation.value.y, -1*slime.TargetTransform.Rotation.value.y);
                    // Debug.Log(slime.TargetTransform.Rotation);
                    break;
            }
            slime.TargetTransform.Position = new float3(slime.TargetTransform.Position.x, 0, slime.TargetTransform.Position.z);
            // Debug.Log("-----");
        }

        public static int IsWithinRange(double x, double y)
        {
            if(x < 0){
                return y <= x && y < 1+x? 1 : -1;
            }else{
                return (y >x && y < 1) || (y > -1 && y < 0-x) ? 1 : -1;
            }
        }
    }

    [BurstCompile]
    // [WithAll(typeof(SlimeComponent))]
    public partial struct SlimeJob: IJobEntity {
        // [NativeDisableUnsafePtrRestriction]
        // [ReadOnly] public NativeHashMap<int2, GridDatum> Int2ToFloorState;
        public float deltaTime;
        public Unity.Mathematics.Random Random;

        public void Execute(ref SlimeComponent slime, ref LocalTransform transform)
        {
            if(slime.CurrSubState == SlimeSubState.Waiting || slime.CurrSubState == SlimeSubState.Idle){
                return;
            }
            // Debug.Log(slime.isAvailable);
                // Debug.Log(Quaternion.Angle(transform.Rotation, slime.TargetTransform.Rotation));

            switch(slime.CurrSubState){
                case SlimeSubState.Rotating:
                    // Debug.Log("Quaternion: " + Quaternion.Angle(transform.Rotation, slime.TargetTransform.Rotation));
                    // Debug.Log(math.dot(slime.TargetTransform.Rotation, Quaternion.Euler(transform.Right())));
                    // Debug.Log();
                    if (Quaternion.Angle(transform.Rotation, slime.TargetTransform.Rotation) > 10){
                        transform = transform.RotateY(slime.TurnSpeed * deltaTime);
                            // * (math.dot(slime.TargetTransform.Rotation, Quaternion.Euler(transform.Right())) >0 ? 1f: -1f));
                    }else{
                        slime.CurrSubState = SlimeSubState.Moving;
                    }
                    break;
                case SlimeSubState.Moving:
                    // Debug.Log("distance: " + math.distance(transform.Position, slime.TargetTransform.Position));
                    if (math.distance(transform.Position, slime.TargetTransform.Position) > 0.15){
                        transform.Position += math.normalize(slime.TargetTransform.Position - transform.Position) * slime.MoveSpeed * deltaTime;
                    }else{
                        slime.CurrSubState = SlimeSubState.Waiting;
                    }
                    break;
            }
            

            // GridDatum floorDatum = Int2ToFloorState[new int2(new float2(transform.Position.x, transform.Position.z))];
            // switch(floorDatum.State){
            //     default:
            //         break;
            //     case FloorState.Move:
            //         // Debug.Log("Rotate To "+ floorDatum.Direction.ToString());
            //         transform = transform.RotateY(floorDatum.Direction);
            //         // transform.TransformScale(2);
            //         break;
            //     case FloorState.Music:
            //         // Debug.Log("Music!");
            //         transform = transform.Translate(new float3(0, 5, 0));
            //         break;
            // }
        }
    }
}



