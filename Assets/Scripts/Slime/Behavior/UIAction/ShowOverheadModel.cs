using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using DG.Tweening;

[TaskCategory("SlimeUIAction")]
[TaskDescription("Show UI Image. Returns Success.")]
public class ShowOverheadModel : SlimeAction
{
    public GameObject overheadModel;
    public override void OnStart()
    {
        myEffect.ShowOverheadUI_RotateScale(overheadModel);
    }
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
