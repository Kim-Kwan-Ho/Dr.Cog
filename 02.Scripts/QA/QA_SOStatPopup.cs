using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QA_SOStatPopup : UIPopup
{
    #region PlayerStat
    [Header("PlayerStat")]
    [SerializeField] private PlayerStatSo _playerStatSo;
    [SerializeField] private TextMeshProUGUI _powerText;
    [SerializeField] private TextMeshProUGUI _overStatText;
    [SerializeField] private TextMeshProUGUI _statTimeText;
    [SerializeField] private TextMeshProUGUI _clockText;
    [SerializeField] private TextMeshProUGUI _counterClockText;

    [SerializeField] private TMP_InputField _powerField;
    [SerializeField] private TMP_InputField _overStatField;
    [SerializeField] private TMP_InputField _statTimeField;
    [SerializeField] private TMP_InputField _clockField;
    [SerializeField] private TMP_InputField _counterClockField;
    [SerializeField] private Button _playerStatApplyButton;
    #endregion


    #region SupplyStat
    [Header("SupplyStat")]
    [SerializeField] private SupplySo _supplySo;
    [SerializeField] private TextMeshProUGUI _supplyTimeText;
    [SerializeField] private TextMeshProUGUI _supplyCountText;

    [SerializeField] private TMP_InputField _supplyTimeField;
    [SerializeField] private TMP_InputField _supplyCountField;
    [SerializeField] private Button _supplyStatApplyButton;
    #endregion
    [Header("InventoryStat")]
    [SerializeField] private InventorySo _inventorySo;
    [SerializeField] private TextMeshProUGUI _inventoryText;
    [SerializeField] private TextMeshProUGUI _startGearText;
    [SerializeField] private TMP_InputField _inventoryField;
    [SerializeField] private TMP_InputField _startGearField;
    [SerializeField] private Button _inventoryStatApplyButton;

    #region Memory

    [SerializeField] private TextMeshProUGUI _memoryText;
    [SerializeField] private TMP_InputField _memoryField;
    [SerializeField] private Button _memoryStatApplyButton;


    #endregion
    [SerializeField] private Button _closeButton;

    protected override void Initialize()
    {
        base.Initialize();
        _playerStatApplyButton.onClick.AddListener(ApplyPlayerStat);
        _supplyStatApplyButton.onClick.AddListener(ApplySupplyStat);
        _inventoryStatApplyButton.onClick.AddListener(ApplyInventoryStat);
        _memoryStatApplyButton.onClick.AddListener(ApplyMemoryStat);
        _closeButton.onClick.AddListener(ClosePopupUI);
    }

    private void ApplyPlayerStat()
    {
        PlayerStat playerStat = new PlayerStat(int.Parse(_powerField.text),  float.Parse(_statTimeField.text), float.Parse(_overStatField.text), float.Parse(_clockField.text),
            float.Parse(_counterClockField.text));
        _playerStatSo.SwapStat(playerStat);
    }

    private void ApplySupplyStat()
    {
        _supplySo.ChangeSupplyStat(float.Parse(_supplyTimeField.text), int.Parse(_supplyCountField.text));
    }

    private void ApplyInventoryStat()
    {
        _inventorySo.ChangeInventoryStat(int.Parse(_inventoryField.text), int.Parse(_startGearField.text));
    }

    private void ApplyMemoryStat()
    {
        QAManager.Instance.ChangeMemory(int.Parse(_memoryField.text));
    }
    private void Update()
    {
        _powerText.text = _playerStatSo.CurrentPlayerStat.Power.ToString();
        _overStatText.text = _playerStatSo.CurrentPlayerStat.AttachedGearDecreaseAmount.ToString();
        _statTimeText.text = _playerStatSo.CurrentPlayerStat.StatIncreaseTime.ToString();
        _clockText.text = _playerStatSo.CurrentPlayerStat.ClockPlusStat.ToString();
        _counterClockText.text = _playerStatSo.CurrentPlayerStat.CounterPlusStat.ToString();

        _supplyTimeText.text = _supplySo.CurSupplyTime.ToString();
        _supplyCountText.text = _supplySo.CurSupplyCount.ToString();

        _inventoryText.text = _inventorySo.CurMaxSlot.ToString();
        _startGearText.text = _inventorySo.CurInitialGearCount.ToString();

        _memoryText.text = QAManager.Instance.Memory.ToString();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _powerText = FindGameObjectInChildren<TextMeshProUGUI>("PowerText");
        _overStatText = FindGameObjectInChildren<TextMeshProUGUI>("OverStatText");
        _statTimeText = FindGameObjectInChildren<TextMeshProUGUI>("StatTimeText");
        _clockText = FindGameObjectInChildren<TextMeshProUGUI>("ClockText");
        _counterClockText = FindGameObjectInChildren<TextMeshProUGUI>("CounterClockText");

        _powerField = FindGameObjectInChildren<TMP_InputField>("PowerField");
        _overStatField = FindGameObjectInChildren<TMP_InputField>("OverStatField");
        _statTimeField = FindGameObjectInChildren<TMP_InputField>("StatTimeField");
        _clockField = FindGameObjectInChildren<TMP_InputField>("ClockField");
        _counterClockField = FindGameObjectInChildren<TMP_InputField>("CounterClockField");
        _playerStatApplyButton = FindGameObjectInChildren<Button>("PlayerStatApplyButton");


        _supplyTimeText = FindGameObjectInChildren<TextMeshProUGUI>("SupplyTimeText");
        _supplyCountText = FindGameObjectInChildren<TextMeshProUGUI>("SupplyCountText");
        _supplyTimeField = FindGameObjectInChildren<TMP_InputField>("SupplyTimeField");
        _supplyCountField = FindGameObjectInChildren<TMP_InputField>("SupplyCountField");
        _supplyStatApplyButton = FindGameObjectInChildren<Button>("SupplyStatApplyButton");

        _inventoryText = FindGameObjectInChildren<TextMeshProUGUI>("InventoryText");
        _startGearText = FindGameObjectInChildren<TextMeshProUGUI>("StartGearText");
        _inventoryField = FindGameObjectInChildren<TMP_InputField>("InventoryField");
        _startGearField = FindGameObjectInChildren<TMP_InputField>("StartGearField");
        _inventoryStatApplyButton = FindGameObjectInChildren<Button>("InventoryStatApplyButton");

        _memoryText = FindGameObjectInChildren<TextMeshProUGUI>("MemoryText");
        _memoryField = FindGameObjectInChildren<TMP_InputField>("MemoryField");
        _memoryStatApplyButton = FindGameObjectInChildren<Button>("MemoryStatApplyButton");
        _closeButton = FindGameObjectInChildren<Button>("CloseButton");
    }
#endif

}
