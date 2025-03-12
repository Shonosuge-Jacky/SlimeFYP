using BehaviorDesigner.Runtime;
using UnityEngine;

public class SlimeAIManager : MonoBehaviour
{
    BehaviorTree behaviorTree;
    private void Awake() {
        behaviorTree = GetComponent<BehaviorTree>();    
    }

    public void GetCalled(Transform by){
        // Debug.Log("GetCalled");
        behaviorTree.SendEvent<object>("CallEvent", by);
    }
}
