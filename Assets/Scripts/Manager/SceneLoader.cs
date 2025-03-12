using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public GameObject Menu;
    public GameObject LoadUI;

    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this);
    }
    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method is called when a scene has finished loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene '{scene.name}' loaded.");
        // Trigger your event here for when the scene finishes loading
    }

    // Call this method to start loading a scene
    public void LoadScene(string sceneName)
    {
        Debug.Log($"Starting to load scene '{sceneName}'...");
        // You can also trigger an event here for when the scene starts loading
        LoadUI.SetActive(true);
        LoadUI.transform.GetChild(0).GetComponent<Image>().DOFade(1, 0.5f);


        // Start loading the scene asynchronously
        AsyncOperation asyncLoad = null;
        
        DOVirtual.DelayedCall(0.5f, ()=> 
        {asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.completed += (operation) =>
        {
            Debug.Log($"Scene '{sceneName}' finished loading.");
            // Trigger your event here for when the scene finishes loading
            LoadUI.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0.5f);
            DOVirtual.DelayedCall(0.5f, ()=> LoadUI.SetActive(false));
            if(Menu)
                Destroy(Menu);
        };
        });

        // Optionally, you can track the progress of the loading
        
    }
}