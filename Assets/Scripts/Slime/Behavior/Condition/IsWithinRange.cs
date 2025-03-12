using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeCondition")]
[TaskDescription("Check if slime state same. Returns Success if yes. Returns Failure if no.")]
public class IsWithinRange : SlimeCondition
{
    [SerializeField] SharedTransform targetTransform;
    [SerializeField] float distance;
    public override TaskStatus OnUpdate()
    {
        // if(Vector3.Distance(transform.position, targetTransform.Value.position) < distance){
        //     return TaskStatus.Success;
        // }else{
        //     targetTransform = null;
        //     return TaskStatus.Failure;
        // }
        return Vector3.Distance(transform.position, targetTransform.Value.position) < distance ? TaskStatus.Success : TaskStatus.Failure;
    }
    public override void OnEnd()
    {
    }
}
