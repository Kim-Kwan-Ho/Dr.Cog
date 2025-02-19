using Steamworks;
using UnityEngine;

public class QAManager : BaseBehaviour
{

    public static QAManager Instance;


    protected override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }


    private void Update()
    {
        KeyInputHandler();
    }

    private void KeyInputHandler()
    {
        if (Input.GetKeyDown(_memoryIncreaseKey))
        {
            IncreaseMemory();
        }
        if (Input.GetKeyDown(_gearSelectOpenKey))
        {
            OpenGearSelectPopup();
        }
        if (Input.GetKeyDown(_gearRemoveOpenKey))
        {
            OpenGearRemovePopup();
        }
        if (Input.GetKeyDown(_gearUpgradeOpenKey))
        {
            OpenGearUpgradePopup();
        }

        if (Input.GetKeyDown(_augmentAddOpenKey))
        {
            OpenAugmentOpenPopup();
        }

        if (Input.GetKeyDown(_soChangeOpenKey))
        {
            OpenSoChangeOpenPopup();
        }

        if (Input.GetKeyDown(_playerDataOpenKey))
        {
            OpenPlayerDataPopup();
        }

        if (Input.GetKeyDown(_nextStageOpenKey))
        {
            OpenNextStagePopup();
        }
    }

    #region MemoryIncrease
    [SerializeField] private KeyCode _memoryIncreaseKey = KeyCode.Q;

    private float _memory = 50;
    public float Memory { get { return _memory; } }
    private void IncreaseMemory()
    {
        FindAnyObjectByType<StageMemory>().AddMemory(_memory);
    }
    public void ChangeMemory(float amount)
    {
        _memory = amount;
    }
    #endregion
    #region GearAdd
    [SerializeField] private KeyCode _gearSelectOpenKey = KeyCode.W;
    private void OpenGearSelectPopup()
    {
        UIManager.Instance.OpenPopupUI<QA_GearSelectPopup>().SetRewardGears(GearDataManager.Instance.GetNonRepetitionGears(30));
    }
    #endregion
    #region GearRemove
    [SerializeField] private KeyCode _gearRemoveOpenKey = KeyCode.E;
    private void OpenGearRemovePopup()
    {
        UIManager.Instance.OpenPopupUI<QA_GearRemovePopup>().SetRewardGears(GearDataManager.Instance.GetPlayerGearData());
    }
    #endregion

    #region GearRemove
    [SerializeField] private KeyCode _gearUpgradeOpenKey = KeyCode.R;
    private void OpenGearUpgradePopup()
    {
        UIManager.Instance.OpenPopupUI<QA_GearUpgradePopup>().SetRewardGears(GearDataManager.Instance.GetPlayerUpgradeGears());
    }
    #endregion

    #region Augment
    [SerializeField] private KeyCode _augmentAddOpenKey = KeyCode.T;
    private void OpenAugmentOpenPopup()
    {
        UIManager.Instance.OpenPopupUI<QA_AugmentAddPopup>();
    }
    #endregion

    #region SOChnage

    [SerializeField] private KeyCode _soChangeOpenKey = KeyCode.Y;

    private void OpenSoChangeOpenPopup()
    {
        UIManager.Instance.OpenPopupUI<QA_SOStatPopup>();
    }
    #endregion

    #region PlayerData

    [SerializeField] private KeyCode _playerDataOpenKey = KeyCode.U;

    private void OpenPlayerDataPopup()
    {
        UIManager.Instance.OpenPopupUI<QA_PlayerDataPopup>();
    }
    #endregion


    #region NextStage

    [SerializeField] private KeyCode _nextStageOpenKey = KeyCode.I;

    private void OpenNextStagePopup()
    {
        UIManager.Instance.OpenPopupUI<QA_StageSucceedPopup>();
    }

    #endregion

    #region Data Reset

    public void HardResetData()
    {
        GameManager.PlayerGameData.HardResetData();
    }
#endregion
}
