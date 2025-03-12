using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;
using UnityEngine;

[TaskCategory("SlimeAction")]
[TaskDescription("Follow target. Returns Success.")]
public class Follow : SlimeAction
{
    [SerializeField] SharedTransform target ;
    [SerializeField] float distance_min;
    [SerializeField] float distance_max; 
    float distance;
    
    public override void OnStart()
    {
        distance = Random.Range(distance_min, distance_max);
    }
    public override TaskStatus OnUpdate()
    {
        myTransform.position = Vector3.MoveTowards(transform.position, target.Value.position,myProperty.moveSpeed * Time.deltaTime) ;
        if(!isFinished && myProperty.groundCheck.isGround && myRigidbody.velocity.y <= 0){ 
            // myRigidbody.velocity = Vector3.zero;
            myRigidbody.velocity = new Vector3(
                myRigidbody.velocity.x, 
                myRigidbody.velocity.y + myProperty.jumpForce + Random.Range(-1,1), 
                myRigidbody.velocity.z );
        }
        // Debug.Log(Vector3.Distance(target.Value.position, transform.position) - Mathf.Abs(target.Value.position.y - transform.position.y));
        isFinished = Vector3.Distance(target.Value.position, transform.position) - Mathf.Abs(target.Value.position.y - transform.position.y) <= distance;
        return isFinished && myProperty.groundCheck.isGround? TaskStatus.Success : TaskStatus.Running;
    }
    public override void OnEnd()
    {
    }
}
