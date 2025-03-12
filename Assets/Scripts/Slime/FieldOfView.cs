using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] List<GameObject> slimes;
    [SerializeField] List<GameObject> slimes_inView;
    public Collider self;
    public bool foundTarget;

    private void OnTriggerEnter(Collider other) {
        // Debug.Log("OnTriggerEnter" + other.gameObject.name);
        if(other.CompareTag("Slime") && other != self){
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
    

    private void Update() {
        foreach(GameObject slime in slimes){
            if(IsInRange(slime.transform)){
                if(!slimes_inView.Contains(slime)){
                    // Debug.Log("InRange" + slime);
                    slimes_inView.Add(slime);
                }
            }else{
                if(slimes_inView.Contains(slime)){
                    slimes_inView.Remove(slime);
                }
            }
        }
        foundTarget = slimes_inView.Count > 0;
    }

    public bool IsInRange(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        float dot = Vector3.Dot(direction.normalized, transform.forward);
        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg; 
        // Debug.Log(offsetAngle + " " + (offsetAngle < 45 * 0.5f).ToString()  + (direction.magnitude < 15).ToString());
        return offsetAngle < 30 * 0.5f && direction.magnitude < 10;
    }

}
