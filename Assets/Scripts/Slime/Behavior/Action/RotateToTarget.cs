using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeAction")]
[TaskDescription("Rotate to the direction of target. Returns Success.")]
public class RotateToTarget : SlimeAction
{
    [SerializeField] protected SharedTransform targetTransform;
    [SerializeField] bool isSlowRotate;
    private Quaternion target;
    private float rotateDirection;
    public override void OnStart()
    {
        isFinished = false;
        Vector3 dir = targetTransform.Value.position - transform.position;
        dir.Normalize();
        target = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 1, 0);
        // Debug.Log(target.eulerAngles);
        // Debug.Log("DotResult" + DotResult);
        // Debug.Log(Vector3.Dot(dir, transform.right));
        rotateDirection = Vector3.Dot(dir, transform.right) >0 ? 1f: -1f;
        // Debug.Log("Rotate to Target: " + dir.x);
        isFinished = Mathf.Abs(transform.eulerAngles.y - target.eulerAngles.y) <= 3;
        // Debug.Log(Mathf.Abs(transform.eulerAngles.y - target.eulerAngles.y) + "  " + isFinished);
        // Debug.Log(Quaternion.Angle(transform.rotation, target).ToString() + dir + "  " +  target + rotateDirection);
    }

    public override TaskStatus OnUpdate()
    {
        if(!isFinished){
            transform.rotation *= Quaternion.AngleAxis(rotateDirection * (isSlowRotate? myProperty.turnSpeed_slow : myProperty.turnSpeed) * Time.deltaTime, Vector3.up);
        // Debug.Log("Rotate To Target" + Mathf.Abs(transform.eulerAngles.y - target.eulerAngles.y)  + " " +(Mathf.Abs(transform.eulerAngles.y - target.eulerAngles.y) <= 8));
        // Debug.Log("==" + Mathf.Abs(transform.rotation.y - target.y));
            isFinished = Mathf.Abs(transform.eulerAngles.y - target.eulerAngles.y) <= 12;
        }
        
        return isFinished && myProperty.groundCheck.isGround? TaskStatus.Success : TaskStatus.Running;
    }
    
}
