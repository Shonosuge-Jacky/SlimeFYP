using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorSettingUIManager : MonoBehaviour
{
    public WholeFloorSetting wholeFloorSetting;
    public int RoomNumber;
    public int ObjectNumber;
    public GameObject SubPanel;
    public Button JukeBox;
    public Button Dumbbell;
    public Button BookShelf;
    public GameObject Menu;
    List<RoomSetting> roomSettings = new List<RoomSetting>();

    void Start()
    {
        for(int i = 0; i < wholeFloorSetting.wholeFloorSetting.Count; i++)
        {
            GameObject ui = Instantiate(GameDataCenter._Menu_RoomSetting, this.transform);
            ui.GetComponent<RoomSetting>().Initialize(i, this);
            roomSettings.Add(ui.GetComponent<RoomSetting>());
        }
        JukeBox.onClick.AddListener(()=> {SetObject(FloorGameObjectType.Jukebox);});
        Dumbbell.onClick.AddListener(()=> {SetObject(FloorGameObjectType.Dumbbell);});
        BookShelf.onClick.AddListener(()=> {SetObject(FloorGameObjectType.BookShelf);});
    }
    public void SetToChange(int _RoomNumber, int _ObjectNumber)
    {
        RoomNumber = _RoomNumber;
        ObjectNumber = _ObjectNumber;
        SubPanel.SetActive(true);
        Debug.Log(RoomNumber + " " + ObjectNumber + " " + wholeFloorSetting.wholeFloorSetting[RoomNumber].floorSetting.FloorObjects[ObjectNumber].FloorGameObjectType);
    }
    void SetObject(FloorGameObjectType type)
    {
        if(RoomNumber == 100 && ObjectNumber == 100)
        {
            return;
        }
        SubPanel.SetActive(false);
        wholeFloorSetting.wholeFloorSetting[RoomNumber].floorSetting.FloorObjects[ObjectNumber].FloorGameObjectType = type;
        foreach(RoomSetting roomSetting in roomSettings)
        {
            roomSetting.SetTexts();
        }
    }
}
