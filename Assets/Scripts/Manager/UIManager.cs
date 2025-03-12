using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject blackBackground;
    bool isUIAnimation;
    [Header("Setting")]
    public GameObject settingObject;
    public TMP_Text gameSpeedText;
    

    [Header("Clock")]
    public TMP_Text clock;

    [Header("Slime Information")]
    public RectTransform infoPanel;
    public TMP_Text infoText;
    public GameObject slimeInfoParent;
    public TMP_Text slimeInfoText;
    [SerializeField] bool isPointing;
    [SerializeField] bool isShowingInformaiton;

    [Header("Loading")]
    public GameObject LoadingLeftPannel;
    public GameObject LoadingRightPannel;
    
    public void SetPointing(bool isPointing){
        this.isPointing = isPointing;
    }
    public void SetisShowingInformaiton(bool isShowing)
    {
        this.isShowingInformaiton = isShowing;
    }
    public bool GetPointing()
    {
        return this.isPointing;
    }

    private void Awake() {
        EventCenter.Instance.AddEventListener(EventType.ChangeGameModeToInspect, ()=>{Cursor.lockState = CursorLockMode.None;});
        EventCenter.Instance.AddEventListener(EventType.ChangeGameModeToExplore, ()=>{Cursor.lockState = CursorLockMode.None;});
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        UpdateInfoPanelLocation();
        UpdateSlimeInfoPanelLocation();
        if(!isUIAnimation){

        }
        CheckSettingPanel();
    }

    public void UpdateInfoPanelLocation(){
        if(isPointing){
            infoPanel.DOAnchorPosX(350, 0.5f);
        }else{
            infoPanel.DOAnchorPosX(650, 0.5f);
            slimeInfoParent.GetComponent<RectTransform>().DOAnchorPosX(-500, 0.5f);
        }
    }
    public void UpdateSlimeInfoPanelLocation()
    {
        if(isShowingInformaiton)
        {
            slimeInfoParent.GetComponent<RectTransform>().DOAnchorPosX(-50, 0.5f);
        }
    }
    // public void UpdateSlimeInfoPanelState(SlimeProperty slimeProperty){
    //     infoText.text = string.Format("State: {0} \n Music: {1} \n Read: {2} \n Strength: {3}", slimeProperty.slimeState.ToString(), slimeProperty.slimeValue.MusicValue.ToString(), slimeProperty.slimeValue.ReadValue.ToString(), slimeProperty.slimeValue.StrengthValue.ToString());
    // }

    public void UpdateSlimeInfoPanelState(){
        infoText.text = string.Format("Spacebar to see more");
    }

    public void UpdateSlimeInfoPanel(SlimeProperty slimeProperty)
    {
        slimeInfoText.text = string.Format("Slime State: {0} \n \nStatistic: \n Music {1} \n Read: {2} \n Strength: {3}", slimeProperty.slimeState.ToString(), slimeProperty.slimeValue.MusicValue.ToString(), slimeProperty.slimeValue.ReadValue.ToString(), slimeProperty.slimeValue.StrengthValue.ToString());
    }

    public void UpdateRoomInfoPanelState(){
        infoText.text = string.Format("Pointing At Room (E)");
    }

    public void UpdateGameSpeedUIText(Slider slider){
        gameSpeedText.text = "Speed: " + slider.value.ToString();
    }

    public void CheckSettingPanel(){
        if(!GameManager.Instance.isPausable){
            return;
        }
        if(Input.GetKeyDown(KeyCode.Escape) || GameManager.Instance.CloseSettingPannel){
            if(settingObject.activeSelf){
                blackBackground.GetComponent<Image>().DOFade(0, 0.5f);
                settingObject.GetComponent<RectTransform>().DOAnchorPosX(960, 0.5f);
                DOVirtual.DelayedCall(0.5f, ()=>{
                    settingObject.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    GameManager.Instance.isControlable = true;
                    blackBackground.SetActive(false);
                });
                isUIAnimation = true;
                DOVirtual.DelayedCall(0.5f, ()=> isUIAnimation = false);
                
                
            }else{
                settingObject.SetActive(true);
                settingObject.GetComponent<RectTransform>().position = new Vector2(960, 300);
                settingObject.GetComponent<RectTransform>().DOAnchorPosX(0, 0.5f).SetEase(Ease.OutBounce);
                
                Cursor.lockState = CursorLockMode.None;
                GameManager.Instance.isControlable = false;
                blackBackground.SetActive(true);
                blackBackground.GetComponent<Image>().DOFade(0.5f, 0.5f);

                isUIAnimation = true;
                DOVirtual.DelayedCall(0.5f, ()=> isUIAnimation = false);
            }
            GameManager.Instance.CloseSettingPannel = false;
        }
    }   
    public void EnterLoadingUI(){
        LoadingRightPannel.GetComponent<RectTransform>().DOAnchorPosX(240,0.3f);
        LoadingLeftPannel.GetComponent<RectTransform>().DOAnchorPosX(-240,0.3f);
    }
    public void ExitLoadingUI(){
        LoadingRightPannel.GetComponent<RectTransform>().DOAnchorPosX(750,0.8f);
        LoadingLeftPannel.GetComponent<RectTransform>().DOAnchorPosX(-750,0.8f);
    }
    
    public void OpenPersonalWebsite(){
        Application.OpenURL("http://www.shonosuge.com/");
    } 
    
}
