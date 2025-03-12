using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class SlimeAction : Action
{
    protected GameObject _slime;
    protected Transform myTransform;
    protected Rigidbody myRigidbody;
    protected SlimeProperty myProperty;
    protected SlimeEffect myEffect;
    protected Renderer myRenderer;
    protected bool isFinished;
    
    public override void OnAwake() {
        _slime = gameObject;
        myTransform = GetComponent<Transform>();
        myRigidbody = transform.GetChild(0).GetChild(0).GetComponent<Rigidbody>();
        myProperty = GetComponent<SlimeProperty>();
        myRenderer = transform.GetChild(1).GetComponent<Renderer>();
        myEffect = GetComponent<SlimeEffect>();
    }
}
