using UnityEngine;
using DG.Tweening;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera secondaryCamera; // Assign in Inspector
    private Camera mainCamera;
    private bool usingMainCamera = true;
    [Header("Property")]
    public float standingY = 0;
    public float crouchingY = -2f;
    public float minFieldOfView = 40;
    public float maxFieldOfView = 80;
    public float sensitivity = 100f;
    float xRotation = 0f;
    public float newFieldOfView = 50;
    public float moveSpeed = 5f;
    [Header("Variable")]
    public Transform playerTransform;

    [Header("Raycast")]
    [SerializeField] RaycastHit HitInfo;
    public LayerMask slimeMask;
    public LayerMask roomMask;
    public float spectateDistance;
    public float inspectDistance;
    GameObject selectedRoom;
    bool pointed;

    SlimeAIManager currPointingSlime;
    public float inpsectDistance = 5;
    public float moveCameraSpeed = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
        // EventCenter.Instance.AddEventListener(EventType.ChangeGameModeToInspect, ()=>{gameObject.transform.eulerAngles = new Vector3(180,0,0);});
        // EventCenter.Instance.AddEventListener(EventType.ChangeGameModeToExplore, ()=>{gameObject.transform.eulerAngles = new Vector3(0,0,0);});
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isControlable){
            UpdateCursorRotation();
            if(GameManager.Instance.CurrGameMode == GameMode.Explore)
            {
                CheckCrouch();
                CheckInteractRaycast();
                CheckInformationRaycast();
            }
            if(GameManager.Instance.CurrGameMode == GameMode.Inspect)
            {
                CheckInteractRaycastToRoom();
            }
            CheckZoom();
            CheckSwitch();
        }
        
        
    }

    void UpdateCursorRotation(){
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }

    void CheckCrouch(){
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            transform.DOLocalMoveY(crouchingY, 0.7f);
        }else if(Input.GetKeyUp(KeyCode.LeftShift)){
            transform.DOLocalMoveY(standingY, 0.7f);
        }
    }
    void CheckZoom(){
        
        float fieldOfViewChange = newFieldOfView + Input.mouseScrollDelta.y * -5;
        
        if(minFieldOfView <= fieldOfViewChange && maxFieldOfView >= fieldOfViewChange){
            // Debug.Log(fieldOfViewChange);
            newFieldOfView = fieldOfViewChange;
        }
        GetComponent<Camera>().DOFieldOfView(newFieldOfView, 0.3f);
    }

    void CheckInteractRaycast(){
        // Debug.Log("CheckInteractRaycast");
        Debug.DrawRay(transform.position, transform.forward * 100.0f, Color.yellow);
        if(Physics.Raycast(transform.position,transform.forward, out HitInfo, spectateDistance, slimeMask)){
            // GameManager.Instance.UIManager.UpdateSlimeInfoPanelState(HitInfo.transform.parent.parent.GetComponent<SlimeProperty>());
            GameManager.Instance.UIManager.UpdateSlimeInfoPanelState();
            GameManager.Instance.UIManager.SetPointing(true);
        }else{
            GameManager.Instance.UIManager.SetPointing(false);
        }
            
    }

    void CheckInformationRaycast()
    {
        if(GameManager.Instance.UIManager.GetPointing())
        {
            currPointingSlime = HitInfo.transform.parent.parent.GetComponent<SlimeAIManager>();
            GameManager.Instance.UIManager.UpdateSlimeInfoPanel(HitInfo.transform.parent.parent.GetComponent<SlimeProperty>());
            if(Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.UIManager.SetisShowingInformaiton(true);
                // currPointingSlime.GetInspect(transform);
                // StartInspect();
            }
        }else
        {
            GameManager.Instance.UIManager.SetisShowingInformaiton(false);
            currPointingSlime = null;
        }
    }

    void CheckInteractRaycastToRoom(){
        if(Physics.Raycast(transform.position,transform.forward, out HitInfo, inspectDistance, roomMask)){
            GameManager.Instance.UIManager.UpdateRoomInfoPanelState();
            GameManager.Instance.UIManager.SetPointing(true);
            GameManager.Instance.SelectedRoom = HitInfo.transform.gameObject.GetComponent<RoomProperty>();
            selectedRoom = HitInfo.transform.gameObject;
            selectedRoom.GetComponent<MeshRenderer>().material = GameDataCenter._RoomMaterialSelected;
            pointed = true;
        }else{
            if(!pointed){
                return;
            }
            GameManager.Instance.UIManager.SetPointing(false);
            GameManager.Instance.SelectedRoom = null;
            selectedRoom.GetComponent<MeshRenderer>().material = GameDataCenter._RoomMaterialIdle;
            selectedRoom = null;
            pointed = false;
        }
    }

    void CheckSwitch()
    {
        if (Input.GetKey(KeyCode.Tab)) // While Tab is held down
        {
            mainCamera.enabled = false;
            secondaryCamera.enabled = true;
        }
        else // When Tab is released
        {
            mainCamera.enabled = true;
            secondaryCamera.enabled = false;
        }
    }


    void StartInspect()
    {
        
        GameManager.Instance.isControlable = false;
        GameManager.Instance.isPausable = false;
        // Get the position of the Slime
        Vector3 slimePosition = currPointingSlime.transform.position;

        // Calculate the direction from the camera to the slime
        Vector3 directionToSlime = (slimePosition - transform.position).normalized;

        // Calculate the target position
        Vector3 targetPosition = slimePosition - directionToSlime * inpsectDistance;
        targetPosition.y = -3;

        Debug.Log("Start Inspect" + targetPosition);

        StartCoroutine(MoveCamera(targetPosition));
    }

    private System.Collections.IEnumerator MoveCamera(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // Move the camera towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveCameraSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Ensure the camera reaches the exact target position
        transform.position = targetPosition;
    }

}
