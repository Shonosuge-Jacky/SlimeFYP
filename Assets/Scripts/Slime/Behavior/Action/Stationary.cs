using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeAction")]
[TaskDescription("After landing, make the root velocity = 0. Returns Success.")]
public class Stationary : SlimeAction
{
    [SerializeField] FloorState currFloorState;
    public override void OnStart()
    {

    }
    public override TaskStatus OnUpdate()
    {
        // if(myProperty.currGrid.floorState != currFloorState){
        //     return 
        // }
        if(myProperty.groundCheck.isGround){
            myRigidbody.velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            return TaskStatus.Success;
        }else{
            return TaskStatus.Running;
        }
        
    }
}
