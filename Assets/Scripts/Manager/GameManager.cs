/// -------------------------------------------------------------------///
/// Script Documentation 
/// A Game Manager responsible for:
/// 1. changing the gamemode (inspect and explore).
/// 2. controlling if the game is paused and controllable.
/// 3. instantiating OOP object in the gameworld when it is changed to explore mode.
/// -------------------------------------------------------------------///

using System.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UIManager UIManager { get; private set; }
    public TimeManager TimeManager { get; private set; }
    public bool isControlable;
    public bool isPausable;
    public GameMode CurrGameMode;
    public bool CloseSettingPannel;

    static GameObject m_OOPSlimeParent;

    private Coroutine countdownCoroutine;

    public RoomProperty SelectedRoom;

    bool firstLoaded = false;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        UIManager = GetComponentInChildren<UIManager>();
        TimeManager = GetComponentInChildren<TimeManager>();
        // m_OverheadCamera = FindObjectOfType<OverheadCamera>().gameObject;
        m_OOPSlimeParent = GameObject.FindGameObjectWithTag("SlimeParent");

    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        CurrGameMode = GameDataCenter._InitialGameMode;
    }
    
    private void Update(){
        if(!firstLoaded)
        {
            SceneLoader.Instance.LoadUI.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f);
            DOVirtual.DelayedCall(0.5f, ()=> SceneLoader.Instance.LoadUI.SetActive(false));
            firstLoaded = true;
        }
        if (CurrGameMode == GameMode.Explore)
        {
            if (countdownCoroutine == null)
            {
                countdownCoroutine = StartCoroutine(TriggerEventEveryTwoSeconds());
            }
        }
        else
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null; // Reset the reference
            }
        }
        if(isControlable && Input.GetKeyUp(KeyCode.E)){
            ChangeGameMode();
        }
    }

    /// <summary>
    /// Function responsible for changing the game mode (inspect and explore)
    /// </summary>
    public void ChangeGameMode(){
        isControlable = false;
        isPausable = false;
        UIManager.Instance.EnterLoadingUI();
        DOVirtual.DelayedCall(0.5f, ()=>{
            CurrGameMode = CurrGameMode == GameMode.Inspect? GameMode.Explore : GameMode.Inspect;
            switch(CurrGameMode)
            {
                case GameMode.Inspect:
                    EventCenter.Instance.BoardcastEvent(EventType.ChangeGameModeToInspect);
                    StartCoroutine(EventCenter.Instance.SendEventToECS(EventType.ChangeGameModeToInspect));
                    // m_PlayerGameObject.SetActive(true);
                    // m_OverheadCamera.SetActive(false);
                    break;
                case GameMode.Explore:
                    EventCenter.Instance.BoardcastEvent(EventType.ChangeGameModeToExplore);
                    StartCoroutine(EventCenter.Instance.SendEventToECS(EventType.ChangeGameModeToExplore));
                    break;
            }
        });
        
    }

    /// <summary>
    /// Function responsible for creating OOP gameobject when gamemode is changed to explore mode.
    /// </summary>
    /// <param name="slime">ECS component SlimeComponent</param>
    /// <param name="slimeTransform">ECS component slime's LocalTransform</param>
    public static void CreateOOPGameObject(RefRO<SlimeComponent> slime, RefRO<LocalTransform> slimeTransform){
        Debug.Log(slime.ValueRO.CurrSubState);
        if(!m_OOPSlimeParent){
            Debug.LogError("No GameObject with Tag SlimeParent");
        }
        Instantiate(GameDataCenter._SlimePrefabOOP, m_OOPSlimeParent.transform).
            GetComponent<SlimeProperty>().Instantiate(slimeTransform.ValueRO.Position, slime.ValueRO.CurrState, slime.ValueRO.CurrValue);
        
    }

    /// <summary>
    /// Trigger a UpdateValue Event every two seconds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TriggerEventEveryTwoSeconds()
    {
        while (CurrGameMode == GameMode.Explore)
        {
            // Trigger the event
            EventCenter.Instance.BoardcastEvent(EventType.UpdateValueEvent);

            // Wait for 2 seconds before triggering it again
            yield return new WaitForSeconds(2f);
        }
    }

}

public enum GameMode{
    Inspect,
    Explore,
    Phone
}