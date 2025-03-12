using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public Button OpenSceneSetting;
    public Button CloseSceneSetting;
    public Button StartButton;
    public Button ExitButton;

    public GameObject SceneSetting;

    void Start()
    {
        OpenSceneSetting.onClick.AddListener(()=>SceneSetting.SetActive(true));
        CloseSceneSetting.onClick.AddListener(()=>SceneSetting.SetActive(false));
        StartButton.onClick.AddListener(()=> SceneLoader.Instance.LoadScene("Test"));
        ExitButton.onClick.AddListener(()=> Application.Quit());
    }
}
