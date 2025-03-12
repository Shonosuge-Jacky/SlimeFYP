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

    public void Initialize(int id, FloorSettingUIManager floorSettingUIManager)
    {
        RoomNumber = id;
        roomNumber.text = $"RoomNumber {RoomNumber}";
        FloorSettingUIManager = floorSettingUIManager;
        SetTexts();
    }

    public void SetTexts()
    {
        SetText(FloorObject1, 1);
        SetText(FloorObject2, 2);
        SetText(FloorObject3, 3);
        SetText(FloorObject4, 4);
        SetText(FloorObject5, 5);
        SetText(FloorObject6, 6);
    }
    void Start()
    {
        FloorObject1.onClick.AddListener(()=> SetToChange(FloorObject1, RoomNumber, 1));
        FloorObject2.onClick.AddListener(()=> SetToChange(FloorObject2, RoomNumber, 2));
        FloorObject3.onClick.AddListener(()=> SetToChange(FloorObject3, RoomNumber, 3));
        FloorObject4.onClick.AddListener(()=> SetToChange(FloorObject4, RoomNumber, 4));
        FloorObject5.onClick.AddListener(()=> SetToChange(FloorObject5, RoomNumber, 5));
        FloorObject6.onClick.AddListener(()=> SetToChange(FloorObject6, RoomNumber, 6));
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
