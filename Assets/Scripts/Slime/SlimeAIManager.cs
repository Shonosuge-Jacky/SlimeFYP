/// -------------------------------------------------------------------///
/// Script Documentation 
/// Responsible for Slime's Interact Action.
/// -------------------------------------------------------------------///
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

    public void GetInspect(Transform by)
    {
        behaviorTree.SendEvent<object>("InspectEvent", by);
    }

    public void GetPet()
    {
        behaviorTree.SendEvent("PetEvent");
    }
}
