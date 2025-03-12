using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlimeEffect : MonoBehaviour
{
    public List<GameObject> overheadUIs;
    public Transform parent;
    bool isAnimation;
    private void Start() {
        parent.localScale = Vector3.zero;
        parent.localRotation = Quaternion.identity;
        isAnimation = false;
    }

    public void ShowOverheadUI_RotateScale(GameObject ui){
        if(!isAnimation){
            isAnimation = true;
            EnableOverheadUI(ui);
            Debug.Log("RotateScale");
            parent.DOScale(1f, 0.35f);
            parent.DOLocalRotate(new Vector3(0,360f,0f), 0.5f, RotateMode.FastBeyond360);
            DOVirtual.DelayedCall(0.35f, ()=>{
                parent.DOScale(0.85f, 0.2f);
                DOVirtual.DelayedCall(0.2f, ()=>{
                    parent.DOScale(1f, 0.2f);
                    DOVirtual.DelayedCall(0.4f, ()=>{
                        parent.DOLocalRotate(new Vector3(0,0,0f), 0.25f, RotateMode.FastBeyond360);
                        parent.DOScale(0f, 0.25f);
                        isAnimation = false;
                    });
                });
            });
        }
        
    }
    private void EnableOverheadUI(GameObject enable){
        foreach(GameObject overheadUI in overheadUIs){
            overheadUI.SetActive(false);
        }
        enable.SetActive(true);
    }
}
