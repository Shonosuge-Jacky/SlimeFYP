using UnityEngine;

public class Jukebox : EnvironmentObject
{
    Animator myAnimator;
    private void Awake() {
        myAnimator = GetComponent<Animator>();
    }
    protected override void ChangeToDay()
    {
        Debug.Log("Change to day");
        myAnimator.SetBool("isPlaying", true);
    }

    protected override void ChangeToNight()
    {
        myAnimator.SetBool("isPlaying", false);
    }
}
