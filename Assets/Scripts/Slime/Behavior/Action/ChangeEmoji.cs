using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("SlimeAction")]
[TaskDescription("Change to dedicated emoji. Returns Success.")]
public class ChangeEmoji : SlimeAction
{
    [SerializeField] protected Emoji faceMaterial;
    
    public override void OnStart()
    {
        // Debug.Log("ChangeFace" + faceMaterial.name);
        myRenderer.material = myProperty.EmojiToMaterial(faceMaterial);
        
    }
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
    public override void OnEnd()
    {
    }
}
