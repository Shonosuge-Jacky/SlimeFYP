using System;
using UnityEngine;

// public struct 
public struct GameDataCenter
{
    [Header("GameSetting")]
    public static GameMode              _InitialGameMode        = GameMode.Inspect;

    [Header("UI")]
    public static GameObject            _Menu_RoomSetting       = Resources.Load<GameObject>("UI/RoomSetting");
    [Header("EnviornmentObject")]
    public static FloorGridsStorage     _FloorGridStorage       = Resources.Load<FloorGridsStorage>("ScriptableObjects/FloorGridStorage");
    public static FloorSetting          _FloorSetting           = Resources.Load<FloorSetting>("ScriptableObjects/FloorSetting");
    public static FloorGameObject       _Jukebox                = Resources.Load<FloorGameObject>("ScriptableObjects/MusicBox");
    public static FloorGameObject       _StreetLight            = Resources.Load<FloorGameObject>("ScriptableObjects/StreetLight");
    public static FloorGameObject       _BookShelf              = Resources.Load<FloorGameObject>("ScriptableObjects/BookShelf");
    public static FloorGameObject       _Dumbbell               = Resources.Load<FloorGameObject>("ScriptableObjects/Dumbbell");

    [Header("RoomSetting")]
    public static WholeFloorSetting     _WholeFloorSetting      = Resources.Load<WholeFloorSetting>("ScriptableObjects/WholeFloorSetting");
    public static FloorState            _FloorInitialState      = FloorState.Idle;
    public static Color                 _FloorSelectColor       = new Color(1, 0.93f, 0.78f);
    public static GameObject            _RoomPrefab             = Resources.Load<GameObject>("Prefabs/EnvironmentObject/Platform");
    public static Material              _RoomMaterialIdle       = Resources.Load<Material>("Material/RoomIdle");
    public static Material              _RoomMaterialSelected   = Resources.Load<Material>("Material/RoomSelected");

    [Header("ECSSetting")]
    public static int                   _SlimeAmount            = 100;

    [Header("OOPPrefab")]
    public static GameObject            _SlimePrefabOOP         = Resources.Load<GameObject>("Prefabs/SlimePrefabOOP");
    public static GameObject            _PlayerPrefab           = Resources.Load<GameObject>("Prefabs/PlayerPrefab");
    
    [Header("ECSPrefab")]
    public static GameObject            _SlimePrefabECS         = Resources.Load<GameObject>("Prefabs/SlimePrefabECS");
    public static GameObject            _EmptyPrefabECS         = Resources.Load<GameObject>("Prefabs/EmptyPrefabECS");

    [Header("SlimeProperty")]
    public static int                   _SlimeEvolveValue       = 15;
    public static float                 _SlimeMoveSpeed         = 1f;
    public static float                 _SlimeTurnSpeed         = 90f;
    public static float                 _SlimeTurnSpeed_Slow    = 30f;
    public static float                 _SlimeJumpForce         = 40f;
    public static SlimeState            _SlimeInitialState      = SlimeState.Idle;
    public static Emoji                 _SlimeInitialEmoji      = Emoji.Idle;
    public static FaceMaterial          _SlimeFaceMaterialGreen = new FaceMaterial
    {
        idle        = Resources.Load<Material>("Texture/Green/idle"),
        closeEye    = Resources.Load<Material>("Texture/Green/closeEye"),
        mad         = Resources.Load<Material>("Texture/Green/mad"),
        excited     = Resources.Load<Material>("Texture/Green/excited"),
        confused    = Resources.Load<Material>("Texture/Green/confused"),
        emmm        = Resources.Load<Material>("Texture/Green/emmm"),
    };

    public static FaceMaterial          _SlimeFaceMaterialOrange = new FaceMaterial
    {
        idle        = Resources.Load<Material>("Texture/Orange/idle"),
        closeEye    = Resources.Load<Material>("Texture/Orange/closeEye"),
        mad         = Resources.Load<Material>("Texture/Orange/mad"),
        excited     = Resources.Load<Material>("Texture/Orange/excited"),
        confused    = Resources.Load<Material>("Texture/Orange/confused"),
        emmm        = Resources.Load<Material>("Texture/Orange/emmm"),
    };

    public static FaceMaterial          _SlimeFaceMaterialPurple = new FaceMaterial
    {
        idle        = Resources.Load<Material>("Texture/Purple/idle"),
        closeEye    = Resources.Load<Material>("Texture/Purple/closeEye"),
        mad         = Resources.Load<Material>("Texture/Purple/mad"),
        excited     = Resources.Load<Material>("Texture/Purple/excited"),
        confused    = Resources.Load<Material>("Texture/Purple/confused"),
        emmm        = Resources.Load<Material>("Texture/Purple/emmm"),
    };

    public static FaceMaterial          _SlimeFaceMaterialBlue = new FaceMaterial
    {
        idle        = Resources.Load<Material>("Texture/Blue/idle"),
        closeEye    = Resources.Load<Material>("Texture/Blue/closeEye"),
        mad         = Resources.Load<Material>("Texture/Blue/mad"),
        excited     = Resources.Load<Material>("Texture/Blue/excited"),
        confused    = Resources.Load<Material>("Texture/Blue/confused"),
        emmm        = Resources.Load<Material>("Texture/Blue/emmm"),
    };

    // [Header("PlayerProperty")]



    public static FloorGameObject GetFloorGameObject(FloorGameObjectType type){
        switch(type){
            case FloorGameObjectType.Jukebox:
            return _Jukebox;
            
            case FloorGameObjectType.StreetLight:
            return _StreetLight;

            case FloorGameObjectType.BookShelf:
            return _BookShelf;

            case FloorGameObjectType.Dumbbell:
            return _Dumbbell;
        }
        return null;
    }
}
