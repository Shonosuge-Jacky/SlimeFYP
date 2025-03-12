using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class SlimeCondition : Conditional
{
    protected GameObject _slime;
    protected Transform myTransform;
    protected Rigidbody myRigidbody;
    protected SlimeProperty myProperty;
    protected Renderer myRenderer;

    public override void OnAwake() {
        _slime = gameObject;
        myTransform = GetComponent<Transform>();
        myRigidbody = transform.GetChild(0).GetChild(0).GetComponent<Rigidbody>();
        // myRigidbody = GetComponent<Rigidbody>();
        myProperty = GetComponent<SlimeProperty>();
        myRenderer = transform.GetChild(1).GetComponent<Renderer>();
    }
}
