using System;
using Steamworks;
using UnityEngine;

[RequireComponent(typeof(GameMainEvents))]

public class GameManager : BaseBehaviour
{
    public static GameManager Instance;
    public GameMainEvents EventGameMain;

    [Header("Scriptable Objects")]
    [SerializeField] private PlayerStatSo _playerStatSo;
    [SerializeField] private PlayerAugmentSo _playerAugmentSo;
    [SerializeField] private StageAdditiveStatSo _stageAdditiveStatSo;
    [SerializeField] private InventorySo _inventorySo;
    [SerializeField] private SupplySo _supplySo;
    public static PlayerGameData PlayerGameData = new PlayerGameData();
    public static GameLoadData GameLoadData = new GameLoadData();



    protected override void Awake()
    {
        base.Awake();
        //Cursor.lockState = CursorLockMode.Confined;
        if (Instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        InitializeScriptableObjects();
        InitializePlayerData();
    }

    private void InitializePlayerData()
    {
        if (SaveLoadFile.LoadPlayerData() != null)
        {
            PlayerGameData = SaveLoadFile.LoadPlayerData();
        }

        if (SaveLoadFile.LoadGameData() != null)
        {
            GameLoadData = SaveLoadFile.LoadGameData();
        }
    }

    private void InitializeScriptableObjects()
    {
        _playerStatSo.InitializePlayerStatSo();
        _playerAugmentSo.InitializePlayerAugmentSo();
        _stageAdditiveStatSo.InitializeStageSo();
        _inventorySo.InitializeInventorySo();
        _supplySo.InitializeSupplySo();
    }
    private void Start()
    {
        EventGameMain.CallPlayerDataLoaded(PlayerGameData);
    }
    #region Event
    private void OnEnable()
    {
        GameManager.Instance.EventGameMain.OnDataCleared += Event_OnDataCleared;
    }

    private void OnDisable()
    {
        GameManager.Instance.EventGameMain.OnDataCleared -= Event_OnDataCleared;
    }

    private void Event_OnDataCleared()
    {
        InitializeScriptableObjects();
    }
    #endregion


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        EventGameMain = GetComponent<GameMainEvents>();
    }
#endif
}
