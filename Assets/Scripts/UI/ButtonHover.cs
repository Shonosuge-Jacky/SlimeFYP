using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems; // Make sure to include the DOTween namespace

public class ButtonHoverEffect : MonoBehaviour
{
    public Button button; // Reference to the button
    public float scaleMultiplier = 1.2f; // Scale multiplier for enlargement
    public float duration = 0.2f; // Duration of the scaling animation

    private Vector3 originalScale; // Store the original scale of the button

    private void Start()
    {
        button = GetComponent<Button>();
        // Store the original scale of the button
        originalScale = button.transform.localScale;

        // Add event listeners for pointer enter and exit
        button.onClick.AddListener(OnButtonClick);
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        
        EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry();
        entryPointerEnter.eventID = EventTriggerType.PointerEnter;
        entryPointerEnter.callback.AddListener((data) => { OnPointerEnter(); });
        trigger.triggers.Add(entryPointerEnter);

        EventTrigger.Entry entryPointerExit = new EventTrigger.Entry();
        entryPointerExit.eventID = EventTriggerType.PointerExit;
        entryPointerExit.callback.AddListener((data) => { OnPointerExit(); });
        trigger.triggers.Add(entryPointerExit);
    }

    private void OnPointerEnter()
    {
        // Enlarge the button
        button.transform.DOScale(originalScale * scaleMultiplier, duration).SetEase(Ease.OutBack);
    }

    private void OnPointerExit()
    {
        // Return to original size
        button.transform.DOScale(originalScale, duration).SetEase(Ease.OutBack);
    }

    private void OnButtonClick()
    {
        // Handle button click event here
        Debug.Log("Button clicked!");
    }
}