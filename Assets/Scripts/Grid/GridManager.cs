
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using System;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance {get; private set;}

    [System.Serializable]
    public struct Debugs{
        public GameObject debugPrefab;
        public Material idle;
        public Material state;
        public Material arrow;
    }
    public Debugs debugs;
    Grid grid;
    public FloorGridsStorage floorGridsStorage;

    public Transform floorObjectParent;
    public bool showDebug;
    public DayNightEvent daynightEvent;

    [Header("Floor GameObject")]
    public FloorGameObject musicBoxObject;
    public FloorGameObject streeLightObject;
    public List<GameObject> groundDebugs;

    private void Awake() {
        grid = GetComponent<Grid>();
        floorGridsStorage = new FloorGridsStorage();
    }

    private void Start() {
        foreach(_FloorSetting floorSetting in GameDataCenter._WholeFloorSetting.wholeFloorSetting){
            Debug.Log(floorSetting.floorSetting);
            InitialGridIndicator(floorSetting);
        }
        // GameDataCenter._WholeFloorSetting.wholeFloorSetting[0]);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.K)){
            // ShowDebug(1,1,100,100);
            ShowDebug();
        }
    }
    
    void ShowDebug(){
        foreach(GameObject groundDebug in groundDebugs){
            groundDebug.SetActive(true);
        }
    }
    void ShowDebug(int minX, int minZ, int maxX, int maxZ){
        showDebug = !showDebug;
        for(int i = minX; i<maxX; i++){
            for (int j = minZ; j<maxZ; j++){       
                floorGridsStorage.ActiveDebug(new Vector3Int(i, 0, j), showDebug);
            }
        }
    }

    void InitialGridIndicator(_FloorSetting floorSetting){
        GameObject room = Instantiate(GameDataCenter._RoomPrefab, new Vector3(floorSetting.floorSetting.MinX + floorSetting.x * 150, 0f, floorSetting.floorSetting.MinY + floorSetting.y * 150), Quaternion.identity);
        room.transform.GetChild(0).GetComponent<RoomProperty>().myRoomID = floorSetting.RoomID;
        for(int i = floorSetting.floorSetting.MinX + floorSetting.x * 150 - 15; i<=floorSetting.floorSetting.MaxX + floorSetting.x * 150 + 15; i++){
            for (int j = floorSetting.floorSetting.MinY + floorSetting.y * 150 - 15; j<=floorSetting.floorSetting.MaxY + floorSetting.y * 150 + 15; j++){       
                GameObject debug = Instantiate(debugs.debugPrefab, grid.CellToWorld(new Vector3Int(i,0,j)) + new Vector3(0,0.1f,0), Quaternion.identity, transform);
                floorGridsStorage.AddFloorGrid(new Vector3Int(i,0,j), debug);
                groundDebugs.Add(debug);
                if (!showDebug) debug.SetActive(false);
            }
        }

        foreach(FloorObjectSetting floorObjectSetting in floorSetting.floorSetting.FloorObjects){
            Debug.Log($"Initiate {floorObjectSetting.FloorGameObjectType} in Room {floorSetting.RoomID}");
            AddObjectInGrid(floorObjectSetting.X + floorSetting.x * 150, floorObjectSetting.Y + floorSetting.y * 150, GameDataCenter.GetFloorGameObject(floorObjectSetting.FloorGameObjectType));
        }
        // AddObjectInGrid(20,20,musicBoxObject);
        // AddObjectInGrid(20,60,musicBoxObject);
        // AddObjectInGrid(80,40,musicBoxObject);
        // AddObjectInGrid(10, 50, streeLightObject);
        // AddObjectInGrid(70, 10, streeLightObject);
        // AddObjectInGrid(70, 70, streeLightObject);
        floorGridsStorage.DictionaryToList();
        SetFloorBorder(floorSetting.floorSetting.MinX + floorSetting.x * 150 - 15, floorSetting.floorSetting.MaxX + floorSetting.x * 150 + 15, floorSetting.floorSetting.MinY + floorSetting.y * 150 - 15, floorSetting.floorSetting.MaxY + floorSetting.y * 150 + 15);
        // SetFloorBorder(100);
        // InitiateGridDataAuthoring();
    }

    void InitialGridIndicator(int minX, int minZ, int maxX, int maxZ){
        for(int i = minX; i<maxX; i++){
            for (int j = minZ; j<maxZ; j++){       
                GameObject debug = Instantiate(debugs.debugPrefab, grid.CellToWorld(new Vector3Int(i,0,j)) + new Vector3(0,0.1f,0), Quaternion.identity, transform);
                floorGridsStorage.AddFloorGrid(new Vector3Int(i,0,j), debug);
                if (!showDebug) debug.SetActive(false);
            }
        }

        foreach(FloorObjectSetting floorObjectSetting in GameDataCenter._FloorSetting.FloorObjects){
            AddObjectInGrid(floorObjectSetting.X, floorObjectSetting.Y, GameDataCenter.GetFloorGameObject(floorObjectSetting.FloorGameObjectType));
        }
        // AddObjectInGrid(20,20,musicBoxObject);
        // AddObjectInGrid(20,60,musicBoxObject);
        // AddObjectInGrid(80,40,musicBoxObject);
        // AddObjectInGrid(10, 50, streeLightObject);
        // AddObjectInGrid(70, 10, streeLightObject);
        // AddObjectInGrid(70, 70, streeLightObject);
        floorGridsStorage.DictionaryToList();
        // SetFloorBorder(100);

        // InitiateGridDataAuthoring();
    }

    public void AddObjectInGrid(int x, int z, FloorGameObject floorGameObject){
        
        GameObject newEnvironemtObject = Instantiate(floorGameObject.gameObject, 
            grid.CellToWorld(new Vector3Int(x, 0, z)) + floorGameObject.positionOffset, 
            floorGameObject.rotationOffset, 
            floorObjectParent);
        daynightEvent.AddListener(newEnvironemtObject.GetComponent<EnvironmentObject>());

        for (int i = x - floorGameObject.leadArea ; i < x + floorGameObject.leadArea +1; i++){
            for (int j = z - floorGameObject.leadArea ; j < z + floorGameObject.leadArea +1; j++){
                if(i != x || j != z){
                    // Debug.Log("Move" + i + " " + j);
                    floorGridsStorage.SetFloorState(new Vector3Int(i, 0, j), FloorState.Move, FloorState.Idle, new Vector3(x,0,z) - new Vector3(i,0,j) );
                    floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, j), debugs.arrow, debugs.idle);
                    floorGridsStorage.AddFloorGridToDayNightEventListener(new Vector3Int(i, 0, j), daynightEvent);
                    // Debug.Log(floorGridsStorage)
                }
            }
        }
        
        for (int i = x - floorGameObject.stateArea; i < x + floorGameObject.stateArea+1; i++){
            for (int j = z - floorGameObject.stateArea; j < z + floorGameObject.stateArea+1; j++){
                // Debug.Log("Music" + i + " " + j);
                floorGridsStorage.SetFloorState(new Vector3Int(i, 0, j), floorGameObject.daytimeFloorState, floorGameObject.nighttimeFloorState);
                if(floorGameObject.daytimeFloorState == FloorState.Gym){ continue; }
                floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, j), debugs.state, debugs.idle);
                // floorGridsStorage.RemoveFloorGridToDayNightEventListener(new Vector3Int(i, 0, j), daynightEvent);
            }
        }
    }

    //Not working?
    void SetFloorBorder(int minX, int maxX, int minY, int maxY, int borderRange = 15){
        Debug.Log($"SetFloorBorder: {minX}, {maxX}, {minY}, {maxY}");
        Vector3 midPoint = new Vector3((minX + maxX)/2,0,(minY + maxY)/2);
        // for(int i = min - 1; i >= min - borderRange - 1; i--){
        //     for(int j = min - borderRange - 1; j <= max + borderRange; j++){
        //         floorGridsStorage.SetFloorState( new Vector3Int(i, 0, j), FloorState.Move, midPoint - new Vector3(i, 0, j));
        //         floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0,j), debugs.arrow);
        //         // Int2ToFloorState.TryAdd( new int2(i, j), new GridDatum(FloorState.Move, midPoint - new Vector3(i, 0, j)));
        //     }
        // }
        // for(int i = min; i <= max; i ++){
        //     for(int j = 1; j <= borderRange; j ++){
        //         floorGridsStorage.SetFloorState( new Vector3Int(i, 0, min - j), FloorState.Move, midPoint - new Vector3(i, 0, min - j));
        //         floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0,min - j), debugs.arrow);
        //         floorGridsStorage.SetFloorState( new Vector3Int(i, 0, max + j), FloorState.Move, midPoint - new Vector3(i, 0, max + j));
        //         floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0,max + j), debugs.arrow);
        //         // Int2ToFloorState.TryAdd( new int2(i, min - j), new GridDatum(FloorState.Move, midPoint - new Vector3(i, 0, 0 - j)));
        //         // Int2ToFloorState.TryAdd( new int2(i, max + j), new GridDatum(FloorState.Move, midPoint - new Vector3(i, 0, 100 + j)));
        //     }
        // }

        // for(int i = max + 1 ; i <= max + borderRange + 1; i++){
        //     for(int j = min - borderRange - 1; j <= max + borderRange; j++){
        //         floorGridsStorage.SetFloorState( new Vector3Int(i, 0, j), FloorState.Move, midPoint - new Vector3(i, 0, j));
        //         floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0,j), debugs.arrow);
        //         // Int2ToFloorState.TryAdd( new int2(i, j), new GridDatum(FloorState.Move, midPoint - new Vector3(i, 0, j)));
        //     }
        // }

        for (int i = minX; i <= maxX; i++){
            for ( int x = 0; x <= 15; x++){
                SetBorderData(midPoint, i, minY + x);
                SetBorderData(midPoint, i, maxY - x);
            }
        }
        for (int i = minY; i <= maxY; i++){
            for ( int x = 0; x <= 15; x++){
                SetBorderData(midPoint, minX + x, i);
                SetBorderData(midPoint, maxX - x, i);
            }
        }
        // for (int i = minX; i <= maxX; i++){
        //     for(int j = minY; j <= maxY; j++){
        //         for ( int x = 0; x <= 15; x++){
        //             floorGridsStorage.SetFloorState(new Vector3Int(i, 0, minX+x), FloorState.Move, midPoint - new Vector3(i, 0, minX+x) );
        //             floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, minX+x), debugs.arrow);
        //             floorGridsStorage.SetFloorState(new Vector3Int(i, 0, maxX-x), FloorState.Move, midPoint - new Vector3(i, 0, maxX-x) );
        //             floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, maxX-x), debugs.arrow);
        //             floorGridsStorage.SetFloorState(new Vector3Int(minY+x, 0, j), FloorState.Move, midPoint - new Vector3(minY+x, 0, j) );
        //             floorGridsStorage.SetFloorDebug(new Vector3Int(minY+x, 0, j), debugs.arrow);
        //             floorGridsStorage.SetFloorState(new Vector3Int(maxY-x, 0, j), FloorState.Move, midPoint - new Vector3(maxY-x, 0, j) );
        //             floorGridsStorage.SetFloorDebug(new Vector3Int(maxY-x, 0, j), debugs.arrow);
        //         }
        //     }
            // Debug.Log(i);
            
            
            
            // floorGridsStorage.SetFloorState(new Vector3Int(i, 0, min+2), FloorState.Move, new Vector3((min+max)/2,0,(min+max)/2) - new Vector3(i, 0, min+2) );
            // floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, min+2), debugs.arrow);
            // floorGridsStorage.SetFloorState(new Vector3Int(i, 0, max-2), FloorState.Move, new Vector3((min+max)/2,0,(min+max)/2) - new Vector3(i, 0, max-2) );
            // floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, max-2), debugs.arrow);
            // floorGridsStorage.SetFloorState(new Vector3Int(min+2, 0, i), FloorState.Move, new Vector3((min+max)/2,0,(min+max)/2) - new Vector3(min+2, 0, i) );
            // floorGridsStorage.SetFloorDebug(new Vector3Int(min+2, 0, i), debugs.arrow);
            // floorGridsStorage.SetFloorState(new Vector3Int(max-2, 0, i), FloorState.Move, new Vector3((min+max)/2,0,(min+max)/2) - new Vector3(max-2, 0, i) );
            // floorGridsStorage.SetFloorDebug(new Vector3Int(max-2, 0, i), debugs.arrow);

            // floorGridsStorage.SetFloorState(new Vector3Int(i, 0, min+3), FloorState.Move, new Vector3((min+max)/2,0,(min+max)/2) - new Vector3(i, 0, min+3) );
            // floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, min+3), debugs.arrow);
            // floorGridsStorage.SetFloorState(new Vector3Int(i, 0, max-3), FloorState.Move, new Vector3((min+max)/2,0,(min+max)/2) - new Vector3(i, 0, max-3) );
            // floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, max-3), debugs.arrow);
            // floorGridsStorage.SetFloorState(new Vector3Int(min+3, 0, i), FloorState.Move, new Vector3((min+max)/2,0,(min+max)/2) - new Vector3(min+3, 0, i) );
            // floorGridsStorage.SetFloorDebug(new Vector3Int(min+3, 0, i), debugs.arrow);
            // floorGridsStorage.SetFloorState(new Vector3Int(max-3, 0, i), FloorState.Move, new Vector3((min+max)/2,0,(min+max)/2) - new Vector3(max-3, 0, i) );
            // floorGridsStorage.SetFloorDebug(new Vector3Int(max-3, 0, i), debugs.arrow);
        // }
    }
    void SetBorderData(Vector3 mid, int x, int y){
        floorGridsStorage.SetFloorState(new Vector3Int(x, 0, y), FloorState.Move, mid - new Vector3(x, 0, y) );
        floorGridsStorage.SetFloorDebug(new Vector3Int(x, 0, y), debugs.arrow);
    }
    void SetFloorBorder(int max){
        for (int i = 1; i < max; i++){
            // Debug.Log(i);
            floorGridsStorage.SetFloorState(new Vector3Int(i, 0, 1), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(i,0,1) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, 1), debugs.arrow);
            floorGridsStorage.SetFloorState(new Vector3Int(i, 0, max-1), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(i,0,max-1) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, max-1), debugs.arrow);
            floorGridsStorage.SetFloorState(new Vector3Int(1, 0, i), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(1,0,i) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(1, 0, i), debugs.arrow);
            floorGridsStorage.SetFloorState(new Vector3Int(max-1, 0, i), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(max-1,0,i) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(max-1, 0, i), debugs.arrow);
            
            floorGridsStorage.SetFloorState(new Vector3Int(i, 0, 2), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(i,0,2) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, 2), debugs.arrow);
            floorGridsStorage.SetFloorState(new Vector3Int(i, 0, max-2), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(i,0,max-2) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, max-2), debugs.arrow);
            floorGridsStorage.SetFloorState(new Vector3Int(2, 0, i), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(2,0,i) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(2, 0, i), debugs.arrow);
            floorGridsStorage.SetFloorState(new Vector3Int(max-2, 0, i), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(max-2,0,i) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(max-2, 0, i), debugs.arrow);

            floorGridsStorage.SetFloorState(new Vector3Int(i, 0, 3), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(i,0,3) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, 3), debugs.arrow);
            floorGridsStorage.SetFloorState(new Vector3Int(i, 0, max-3), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(i,0,max-3) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(i, 0, max-3), debugs.arrow);
            floorGridsStorage.SetFloorState(new Vector3Int(3, 0, i), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(3,0,i) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(3, 0, i), debugs.arrow);
            floorGridsStorage.SetFloorState(new Vector3Int(max-3, 0, i), FloorState.Move, new Vector3(max/2,0,max/2) - new Vector3(max-3,0,i) );
            floorGridsStorage.SetFloorDebug(new Vector3Int(max-3, 0, i), debugs.arrow);
        }


    }

    public FloorGrid GetFloorGrid(Vector3 position){
        return floorGridsStorage.GetFloorGrid(grid.WorldToCell(position));
    }

    public FloorState GetFloorState(Vector3 position){
        if(GridDataSystem.GetNativeHashMap().ContainsKey(new int2((int)position.x, (int)position.z))){
            return GridDataSystem.GetNativeHashMap()[new int2((int)position.x, (int)position.z)].State;
        }else{
            return FloorState.Idle;
        }
    }
    public GridDatum GetFloorGridDatum(Vector3 position){
        if(GridDataSystem.GetNativeHashMap().ContainsKey(new int2((int)position.x, (int)position.z))){
            return GridDataSystem.GetNativeHashMap()[new int2((int)position.x, (int)position.z)];
        }else{
            return new GridDatum(FloorState.Idle);
        }
    }


    // private void InitiateGridDataAuthoring(){
    //     GridDataSystem gridDataSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GridDataSystem>();
    //     Debug.Log("---" + gridDataSystem);
    //     // gridDataSystem.RecordGridData(floorGridsStorage.GetDayNightFloorState);
    // }

    

}