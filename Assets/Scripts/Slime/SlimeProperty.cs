using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public enum SlimeState{
    Idle,
    Chat,
    Music,
    Read,
    Gym
}
public struct SlimeValue{
    public int MusicValue;
    public int ReadValue;
    public int StrengthValue;
}
public enum Emoji{
    Idle,
    CloseEye,
    Excited,
    Mad,
    Confused,
    emm
}

[System.Serializable]
public struct FaceMaterial{
    public Material idle;
    public Material closeEye;
    public Material mad;
    public Material excited;
    public Material confused;
    public Material emmm;
}


public class SlimeProperty : MonoBehaviour
{
    [Header("Property")]
    public SlimeState slimeState;
    public SlimeValue slimeValue;
    public SlimeColor slimeColor;
    public float moveSpeed;
    public float turnSpeed;
    public float turnSpeed_slow;
    public float jumpForce;
    public GroundCheck groundCheck;
    public Emoji emoji;
    public bool foundInteractTarget;
    public FloorGrid currGrid;      //no use? (old system)
    public FloorState currGridState;
    public GridDatum currGridDatum;

    [Header("Variables")]
    public FaceMaterial faceMaterial;
    public GameObject root;
    public FieldOfView fieldOfView;
    public GridManager gridManager;
    public SlimeEffect effect;

    // public bool isColliding;
    // public SlimeColliderCheck slimeColliderCheck;
    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        EventCenter.Instance.AddEventListener(EventType.UpdateValueEvent, UpdateValue);
        
        UpdateFaceMaterial();
    }

    public Material EmojiToMaterial(Emoji emoji){
        switch (emoji){
            case Emoji.Idle:
                return faceMaterial.idle;
            case Emoji.CloseEye:
                return faceMaterial.closeEye;
            case Emoji.Excited:
                return faceMaterial.excited;
            case Emoji.Mad:
                return faceMaterial.mad;
            case Emoji.Confused:
                return faceMaterial.confused;
            case Emoji.emm:
                return faceMaterial.emmm; 
            default:
                return faceMaterial.idle;
        }
    }

    private void Update() {
        root.transform.position = new Vector3(transform.position.x, root.transform.position.y, transform.position.z);
        foundInteractTarget = fieldOfView.foundTarget;
        currGridDatum = gridManager.GetFloorGridDatum(new Vector3(transform.position.x, 1, transform.position.z));
        currGridState = currGridDatum.State;
    }

    public void Instantiate(float3 position, SlimeState state, SlimeValue value){
        transform.position = position;
        slimeState = state;
        slimeValue = value;
    }

    void UpdateValue(){
        switch(slimeState){
            default:
                break;
            case SlimeState.Music:
                slimeValue.MusicValue += 1;
                if(slimeValue.MusicValue >GameDataCenter._SlimeEvolveValue){
                    UpdateFaceMaterial();
                }
                break;
            case SlimeState.Read:
                slimeValue.ReadValue += 1;
                if(slimeValue.ReadValue >GameDataCenter._SlimeEvolveValue){
                    UpdateFaceMaterial();
                }
                break;
            case SlimeState.Gym:
                slimeValue.StrengthValue += 1;
                if(slimeValue.StrengthValue > GameDataCenter._SlimeEvolveValue){
                    UpdateFaceMaterial();
                }
                break;
        }
    }
    
    void UpdateFaceMaterial(){
        int val = Math.Max(slimeValue.MusicValue, Math.Max(slimeValue.ReadValue, slimeValue.StrengthValue));
        if(val > GameDataCenter._SlimeEvolveValue){
            if(val == slimeValue.MusicValue && slimeColor != SlimeColor.Purple){
                faceMaterial = GameDataCenter._SlimeFaceMaterialPurple;
                return;
            }else if(val == slimeValue.ReadValue && slimeColor != SlimeColor.Blue){
                faceMaterial = GameDataCenter._SlimeFaceMaterialBlue;
                return;
            }else if(val == slimeValue.StrengthValue && slimeColor != SlimeColor.Orange){
                faceMaterial = GameDataCenter._SlimeFaceMaterialOrange;
                return;
            }
        }
        if(slimeColor != SlimeColor.Green){
            faceMaterial = GameDataCenter._SlimeFaceMaterialGreen;
        }

    }
    
}
