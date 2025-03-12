using Unity.Entities;
using Unity.Burst;
using UnityEngine;
using Unity.Mathematics;
using Unity.Rendering;

[BurstCompile]
public partial struct MyTimedSystem : ISystem
{
    private double _lastUpdateTime; // Keeps track of the last time the system executed

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SlimeComponent>();
        _lastUpdateTime = 0.0; // Initialize the last update time to 0
    }

    public void OnUpdate(ref SystemState state)
    {
        // Get the current elapsed time
        double currentTime = SystemAPI.Time.ElapsedTime;

        // Check if 2 seconds have passed since the last update
        if (currentTime - _lastUpdateTime >= 2.0)
        {
            _lastUpdateTime = currentTime; // Update the last time the system executed
            new SlimePropertyJob{}.ScheduleParallel();

        }
    }

    [BurstCompile]
    public partial struct SlimePropertyJob: IJobEntity {
        public void Execute(ref SlimeComponent slime, ref URPMaterialPropertyBaseColor baseColor){
            // Debug.Log("SlimePropertyJob Execute()");
            // slime.CurrValue.StrengthValue += 1;
            switch(slime.CurrState){
                default:
                    break;
                case SlimeState.Music:
                    slime.CurrValue.MusicValue += 1;
                    break;
                case SlimeState.Read:
                    slime.CurrValue.ReadValue += 1;
                    break;
                case SlimeState.Gym:
                    slime.CurrValue.StrengthValue += 1;
                    break;
            }
            baseColor.Value = new float4(0,0,0,0);
        }

        // void UpdateFaceMaterial(SlimeValue slimeValue, SlimeColor slimeColor, RenderMesh material ){
        //     int val = math.max(slimeValue.MusicValue, math.max(slimeValue.ReadValue, slimeValue.StrengthValue));
        //     if(val > 50){
        //         if(val == slimeValue.MusicValue && slimeColor != SlimeColor.Purple){
        //             material.material = GameDataCenter._SlimeFaceMaterialPurple.idle;
        //             return;
        //         }else if(val == slimeValue.ReadValue && slimeColor != SlimeColor.Blue){
        //             material.material = GameDataCenter._SlimeFaceMaterialBlue.idle;
        //             return;
        //         }else if(val == slimeValue.StrengthValue && slimeColor != SlimeColor.Orange){
        //             material.material = GameDataCenter._SlimeFaceMaterialOrange.idle;
        //             return;
        //         }
        //     }
        //     if(slimeColor != SlimeColor.Green){
        //         material.material = GameDataCenter._SlimeFaceMaterialGreen.idle;
        //     }
        // }
    }

    public partial struct SlimeMaterialJob: IJobEntity {
        [System.Obsolete]
        public void Execute(ref SlimeComponent slime, RenderMesh material){
            int val = math.max(slime.CurrValue.MusicValue, math.max(slime.CurrValue.ReadValue, slime.CurrValue.StrengthValue));
            if(val > 50){
                if(val == slime.CurrValue.MusicValue && slime.CurrColor != SlimeColor.Purple){
                    slime.CurrColor = SlimeColor.Purple;
                    material.material = GameDataCenter._SlimeFaceMaterialPurple.idle;
                    return;
                }else if(val == slime.CurrValue.ReadValue && slime.CurrColor != SlimeColor.Blue){
                    slime.CurrColor = SlimeColor.Blue;
                    material.material = GameDataCenter._SlimeFaceMaterialBlue.idle;
                    return;
                }else if(val == slime.CurrValue.StrengthValue && slime.CurrColor != SlimeColor.Orange){
                    slime.CurrColor = SlimeColor.Orange;
                    material.material = GameDataCenter._SlimeFaceMaterialOrange.idle;
                    return;
                }
            }
            if(slime.CurrColor != SlimeColor.Green){
                slime.CurrColor = SlimeColor.Green;
                material.material = GameDataCenter._SlimeFaceMaterialGreen.idle;
            }
        }
    }

    
}