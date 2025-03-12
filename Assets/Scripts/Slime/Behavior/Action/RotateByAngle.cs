using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeAction")]
[TaskDescription("Rotate by a certain angle. Returns Success.")]
public class RotateByAngle : SlimeAction
{
    [SerializeField] float angle_min = -180;
    [SerializeField] float angle_max = 180;
    [SerializeField] bool isSearchingInteraction;
    float angle;
    private Quaternion target;
    private float rotateDirection;
    public override void OnStart()
    {
        angle = Random.Range(angle_min, angle_max);
        isFinished = false;
        target = transform.localRotation * Quaternion.Euler(0, angle, 0);
        rotateDirection = angle <= 0? -1f: 1f;
    }

    public override TaskStatus OnUpdate()
    {
        
        transform.rotation *= Quaternion.AngleAxis(rotateDirection * myProperty.turnSpeed * Time.deltaTime, Vector3.up);
        // Debug.Log(Quaternion.Angle(transform.rotation, target));
        isFinished = Quaternion.Angle(transform.rotation, target) <= 10;
        // if(isSearchingInteraction){}
        return isFinished && myProperty.groundCheck.isGround? TaskStatus.Success : TaskStatus.Running;
        
    }
    public override void OnEnd()
    {
    }
}
