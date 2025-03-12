using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeAction")]
[TaskDescription("Move forwards. Returns Success.")]
public class MoveForwards : SlimeAction
{
    [SerializeField] float distance_min = 0;
    [SerializeField] float distance_max = 1;
    [SerializeField] bool isSearchingInteraction;
    float distance;
    private Vector3 original;
    
    public override void OnStart()
    {
        distance = Random.Range(distance_min, distance_max);
        // Debug.Log("Move forwards");
        original = transform.position;
    }
    public override TaskStatus OnUpdate()
    {
        myTransform.position += transform.forward * myProperty.moveSpeed * Time.deltaTime;
        if(!isFinished && myProperty.groundCheck.isGround && myRigidbody.velocity.y < 0.3){ 
            // Debug.Log("Jump");
            myRigidbody.velocity = new Vector3(
                myRigidbody.velocity.x, 
                myRigidbody.velocity.y+ myProperty.jumpForce + Random.Range(0,20), 
                myRigidbody.velocity.z );
        }
        isFinished = Vector3.Distance(original, transform.position) >= distance;
        return isFinished && myProperty.groundCheck.isGround? TaskStatus.Success : TaskStatus.Running;
    }
    public override void OnEnd()
    {
    }
}
