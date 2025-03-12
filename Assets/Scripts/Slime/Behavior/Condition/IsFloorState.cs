using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeCondition")]
[TaskDescription("Check if floor state same. Returns Success if yes. Returns Failure if no.")]
public class IsFloorState : SlimeCondition
{
    [SerializeField] FloorState state;
    public override TaskStatus OnUpdate()
    {
        return myProperty.currGridDatum.State == state ? TaskStatus.Success : TaskStatus.Failure;
    }
    public override void OnEnd()
    {
    }
}
