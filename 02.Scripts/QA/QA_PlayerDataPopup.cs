using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QA_PlayerDataPopup : UIPopup
{
    [Header("Search")]
    [SerializeField] private TMP_InputField _stageSearchField;
    [SerializeField] private TMP_InputField _depthSearchField;
    [SerializeField] private TMP_InputField _heightSearchField;
    [SerializeField] private Button _searchButton;
    [SerializeField] private Toggle _searchVisitToggle;
    [SerializeField] private TextMeshProUGUI _searchResultText;

    [Header("Insert")]
    [SerializeField] private TMP_InputField _stageInsertField;
    [SerializeField] private TMP_InputField _depthInsertField;
    [SerializeField] private TMP_InputField _heightInsertField;
    [SerializeField] private Button _insertButton;
    [SerializeField] private Toggle _insertVisitToggle;
    [SerializeField] private TMP_InputField _countInsertField;
    [SerializeField] private Button _closeButton;

    protected override void Initialize()
    {
        base.Initialize();
        _searchButton.onClick.AddListener(SearchPlayerData);
        _closeButton.onClick.AddListener(ClosePopupUI);
        _insertButton.onClick.AddListener(InsertPlayerData);
    }

    private void SearchPlayerData()
    {
        var positionInfo = new MapPositionInfo(int.Parse(_stageSearchField.text), int.Parse(_depthSearchField.text),
            int.Parse(_heightSearchField.text));
        if (_searchVisitToggle.isOn)
        {
            _searchResultText.text = GameManager.PlayerGameData.GetVisitedCount(positionInfo).ToString();
        }
        else
        {
            _searchResultText.text = GameManager.PlayerGameData.GetClearedCount(positionInfo).ToString();
        }
    }

    private void InsertPlayerData()
    {
        var positionInfo = new MapPositionInfo(int.Parse(_stageInsertField.text), int.Parse(_depthInsertField.text),
            int.Parse(_heightInsertField.text));

        if (_insertVisitToggle.isOn)
        {
            GameManager.PlayerGameData.ChangeVisitedCount(positionInfo, int.Parse(_countInsertField.text));
        }
        else
        {
            GameManager.PlayerGameData.ChangeClearedCount(positionInfo, int.Parse(_countInsertField.text));
        }
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _stageSearchField = FindGameObjectInChildren<TMP_InputField>("StageSearchField");
        _depthSearchField = FindGameObjectInChildren<TMP_InputField>("DepthSearchField");
        _heightSearchField = FindGameObjectInChildren<TMP_InputField>("HeightSearchField");
        _searchVisitToggle = FindGameObjectInChildren<Toggle>("SearchVisitToggle");
        _searchButton = FindGameObjectInChildren<Button>("SearchButton");
        _searchResultText = FindGameObjectInChildren<TextMeshProUGUI>("SearchResultText");


        _stageInsertField = FindGameObjectInChildren<TMP_InputField>("StageInsertField");
        _depthInsertField = FindGameObjectInChildren<TMP_InputField>("DepthInsertField");
        _heightInsertField = FindGameObjectInChildren<TMP_InputField>("HeightInsertField");
        _insertVisitToggle = FindGameObjectInChildren<Toggle>("InsertVisitToggle");
        _insertButton = FindGameObjectInChildren<Button>("InsertButton");
        _countInsertField = FindGameObjectInChildren<TMP_InputField>("CountInsertField");
        _closeButton = FindGameObjectInChildren<Button>("CloseButton");
    }
#endif



}
