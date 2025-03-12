using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct UIElements{
    public GameObject[] InspectUIElements;
    public GameObject[] ExploreUIElements;

}

public enum UIState{
    Inspect,
    Explore
}

public class UIStateMachine : MonoBehaviour
{
    public UIElements elements = new UIElements();
    IState currentState;
    UnityEvent enterStateEvent = new UnityEvent();

    void Awake(){
        EventCenter.Instance.AddEventListener(EventType.DoneChangeGameModeToInspect, ()=>SwitchState(UIState.Inspect));
        EventCenter.Instance.AddEventListener(EventType.DoneChangeGameModeToExplore, ()=>SwitchState(UIState.Explore));
    }
    private void Start() {
        if(GameManager.Instance.CurrGameMode == GameMode.Inspect){
            currentState = new InspectUIState();
        }else{
            currentState = new ExploreUIState();
        }
    }

    public void SwitchState(UIState state){
        GameManager.Instance.isControlable = true;
        GameManager.Instance.isPausable = true;
        // enterStateEvent.AddListener(()=>ReceivedEvent(state));
        currentState.OnExit();
        //Enter Loading State
        if (state == UIState.Inspect){
            currentState = new InspectUIState();
        }else{
            currentState = new ExploreUIState();
        }


        if(UIManager.Instance.settingObject.activeSelf == true){
            GameManager.Instance.CloseSettingPannel = true;
            Cursor.lockState = CursorLockMode.Locked;
        }   
        
        
        currentState.OnEnter();
        UIManager.Instance.ExitLoadingUI();
    }
    void ReceivedEvent(IState state){
        enterStateEvent.RemoveAllListeners();
        currentState = state;
        currentState.OnEnter();
    }
}

public interface IState{
    void OnEnter();
    void OnExit();
}
