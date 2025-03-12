using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeAction")]
[TaskDescription("Jump Vertically . Returns Success.")]
public class Jump : SlimeAction
{
    private bool isJumped;
    public override void OnStart()
    {
        isJumped = false;
    }
    public override TaskStatus OnUpdate()
    {
        if(myProperty.groundCheck.isGround && !isJumped){
            myRigidbody.velocity = new Vector3(
                myRigidbody.velocity.x, 
                myRigidbody.velocity.y+ myProperty.jumpForce, 
                myRigidbody.velocity.z ); 
        }
        if(!myProperty.groundCheck.isGround){
            isJumped = true;
        }

        return isJumped && myProperty.groundCheck.isGround? TaskStatus.Success: TaskStatus.Running;
    }
    public override void OnEnd()
    {
    }
}
