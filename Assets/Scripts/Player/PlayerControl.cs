using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;
using UnityEngine;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    
    [Header("Property")]
    public float speed = 12f;
    // public float jumpForce = 5;
    // public float upwardGravity = 6;
    // public float midAirGravity = 10;
    // public float fallGravity = 16;
    public bool isGrounded;
    public Transform inspectPosition;
    public Transform explorePosition;
    Vector3 change;
    PlayerAreaOfInterest areaOfInterest;
    
    // Start is called before the first frame update

    private void Awake() {
        areaOfInterest = transform.GetChild(0).GetComponent<PlayerAreaOfInterest>();
        EventCenter.Instance.AddEventListener(EventType.ChangeGameModeToInspect, ()=>SetPosition(GameManager.Instance.SelectedRoom.myInspectSpawnPoint, 40));
        EventCenter.Instance.AddEventListener(EventType.ChangeGameModeToExplore, ()=>SetPosition(GameManager.Instance.SelectedRoom.myExploreSpawnPoint, 12));
    }
    private void Start() {
        if(GameManager.Instance.CurrGameMode == GameMode.Inspect){
            SetPosition(inspectPosition, 40);
        }else{
            SetPosition(explorePosition, 12);
        }
    }

    void Update()
    {
        if(GameManager.Instance.isControlable){
            CheckMove();
            if(GameManager.Instance.CurrGameMode == GameMode.Explore){
                // isGrounded = Physics.Raycast(transform.position, -Vector3.up, 6.1f);
                if(Input.GetKeyDown(KeyCode.L)){
                    CallSlime();
                }
            }else{
                // Debug.Log("CheckUpDown");
                CheckUpDown();
            }
            
        }
    }
    private void LateUpdate() {
        // CheckJump();
    }

    void CheckMove(){
        change = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
        transform.position += change * speed * Time.deltaTime;
    }
    void CheckUpDown(){
        change = transform.up * Input.GetAxisRaw("Jump");
        transform.position += change * speed * Time.deltaTime;
    }

    void SetPosition(Transform position, float newspeed){
        transform.position = position.position;
        speed = newspeed;
    }

    // void UpdateMovePosition(){
    //     change = transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
    //     controller.Move(change * speed * Time.deltaTime);
    // }

    // void CheckJump(){
    //     if(isGrounded && Input.GetKeyDown(KeyCode.Space)){
    //         GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);
    //     }
    // }

    

    void CallSlime(){
        foreach(GameObject slime in areaOfInterest.GetInterestedSlimes()){
            slime.transform.parent.parent.GetComponent<SlimeAIManager>().GetCalled(transform);
        }
    }
}