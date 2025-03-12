using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeAction")]
[TaskDescription("Set SlimeProperty.SlimeState. Returns Success.")]
[TaskIcon("{SkinColor}SequenceIcon.png")]
public class SetSlimeState : SlimeAction
{
    [SerializeField] SlimeState slimeState;
    public override void OnStart()
    {
        myProperty.slimeState = slimeState;
        // if(slimeState == SlimeState.Music){
        //     // Debug.Log("Change State to" + slimeState);
        // }
        
    }
    public override TaskStatus OnUpdate()
    {
       return TaskStatus.Success;
    }
}