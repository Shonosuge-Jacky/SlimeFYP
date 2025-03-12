using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeAction")]
[TaskDescription("Keep Rotating until reach floor direction. Returns Success.")]
// [TaskIcon("")]
public class RotateUntilFloorDirection : SlimeAction
{
    private Quaternion target;
    private float rotateDirection = 1;
    public override void OnStart()
    {
        isFinished = false;
        target = myProperty.currGridDatum.Direction;
    }

    public override TaskStatus OnUpdate()
    {
        transform.rotation *= Quaternion.AngleAxis(rotateDirection * myProperty.turnSpeed * Time.deltaTime, Vector3.up);
        isFinished = Quaternion.Angle(transform.rotation, target) <= 5;
        return isFinished && myProperty.groundCheck.isGround? TaskStatus.Success : TaskStatus.Running;
    }
}