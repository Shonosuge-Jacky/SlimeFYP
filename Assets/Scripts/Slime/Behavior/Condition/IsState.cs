using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeCondition")]
[TaskDescription("Check if slime state same. Returns Success if yes. Returns Failure if no.")]
public class IsState : SlimeCondition
{
    [SerializeField] SlimeState state;
    
    public override TaskStatus OnUpdate()
    {
        return myProperty.slimeState == state ? TaskStatus.Success : TaskStatus.Failure;
    }
    public override void OnEnd()
    {
    }
}
