using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeCondition")]
[TaskDescription("Check if foundInteractTarget (myPorperty). Returns Success if yes. Returns Failure if no.")]
public class IsFoundTarget : SlimeCondition
{

    public override TaskStatus OnUpdate()
    {
        return myProperty.foundInteractTarget ? TaskStatus.Success : TaskStatus.Failure;
    }
    public override void OnEnd()
    {
    }
}
