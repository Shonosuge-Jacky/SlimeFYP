using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeAction")]
[TaskDescription("Keep Move forwards until the FloorState is not Move. Returns Success.")]
public class MoveForwardUntileFloorNotMove : SlimeAction
{
    public override TaskStatus OnUpdate()
    {
        if(!myProperty.groundCheck.isGround){
            myTransform.position += transform.forward * myProperty.moveSpeed * Time.deltaTime;
        }
        
        if(!isFinished && myProperty.groundCheck.isGround && myRigidbody.velocity.y < 0.3){ 
            myRigidbody.velocity = new Vector3(
                myRigidbody.velocity.x, 
                myRigidbody.velocity.y+ myProperty.jumpForce + Random.Range(0,20), 
                myRigidbody.velocity.z );
        }
        isFinished = myProperty.currGridDatum.State != FloorState.Move;
        return isFinished && myProperty.groundCheck.isGround? TaskStatus.Success : TaskStatus.Running;
    }

}
