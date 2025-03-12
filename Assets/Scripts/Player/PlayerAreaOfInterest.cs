using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAreaOfInterest : MonoBehaviour
{
    [SerializeField] List<GameObject> slimes;

    private void OnTriggerEnter(Collider other) {
        // Debug.Log("OnTriggerEnter" + other.gameObject.name);
        if(other.CompareTag("Slime")){
            slimes.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("OnTriggerExit" + other.gameObject.name);
        if (slimes.Contains(other.gameObject)){
            slimes.Remove(other.gameObject);
        }
    }

    public List<GameObject> GetInterestedSlimes(){
        return slimes;
    }
}
