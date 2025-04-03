using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSetting : MonoBehaviour
{
    FloorSettingUIManager FloorSettingUIManager;
    public int RoomNumber;
    public TextMeshProUGUI roomNumber;
    public Button FloorObject1;
    public Button FloorObject2;
    public Button FloorObject3;
    public Button FloorObject4;
    public Button FloorObject5;
    public Button FloorObject6;
    public Button FloorObject7;
    public Button FloorObject8;
    public Button FloorObject9;

    public void Initialize(int id, FloorSettingUIManager floorSettingUIManager)
    {
        RoomNumber = id;
        roomNumber.text = $"Room {RoomNumber}";
        FloorSettingUIManager = floorSettingUIManager;
        SetTexts();
    }

    public void SetTexts()
    {
        SetText(FloorObject1, 0);
        SetText(FloorObject2, 1);
        SetText(FloorObject3, 2);
        SetText(FloorObject4, 3);
        SetText(FloorObject5, 4);
        SetText(FloorObject6, 5);
        SetText(FloorObject7, 6);
        SetText(FloorObject8, 7);
        SetText(FloorObject9, 8);
    }
    void Start()
    {
        FloorObject1.onClick.AddListener(()=> SetToChange(FloorObject1, RoomNumber, 0));
        FloorObject2.onClick.AddListener(()=> SetToChange(FloorObject2, RoomNumber, 1));
        FloorObject3.onClick.AddListener(()=> SetToChange(FloorObject3, RoomNumber, 2));
        FloorObject4.onClick.AddListener(()=> SetToChange(FloorObject4, RoomNumber, 3));
        FloorObject5.onClick.AddListener(()=> SetToChange(FloorObject5, RoomNumber, 4));
        FloorObject6.onClick.AddListener(()=> SetToChange(FloorObject6, RoomNumber, 5));
        FloorObject7.onClick.AddListener(()=> SetToChange(FloorObject7, RoomNumber, 6));
        FloorObject8.onClick.AddListener(()=> SetToChange(FloorObject8, RoomNumber, 7));
        FloorObject9.onClick.AddListener(()=> SetToChange(FloorObject9, RoomNumber, 8));
    }

    void SetToChange(Button btn, int RoomNumber, int id)
    {
        FloorSettingUIManager.SetToChange(RoomNumber, id);
    }

    void SetText(Button btn, int num)
    {
        btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 
            FloorSettingUIManager.wholeFloorSetting.wholeFloorSetting[RoomNumber].floorSetting.FloorObjects[num].FloorGameObjectType.ToString();
    }
}
