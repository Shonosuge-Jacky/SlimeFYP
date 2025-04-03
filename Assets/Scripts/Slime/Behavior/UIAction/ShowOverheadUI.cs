using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using DG.Tweening;

[TaskCategory("SlimeUIAction")]
[TaskDescription("Show UI Image. Returns Success.")]
public class ShowOverheadUI : SlimeAction
{
    public GameObject overheadUI;
    public bool isActive;
    public override void OnStart()
    {
        overheadUI.SetActive(isActive);
    }
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
