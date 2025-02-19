using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ESynergyUIMode
{
    InGame, OutGame, SynergyTable
}
public class SynergyUI : BaseBehaviour
{
    [SerializeField] private GameObject _synergyItem;
    [SerializeField] private List<SynergySo> _synergySoList;
    private Dictionary<ESynergyType, SynergyUIItem> _synergyInfoDic = new Dictionary<ESynergyType, SynergyUIItem>();


    [SerializeField] private List<SynergyUIItem> _activeSynergyList = new List<SynergyUIItem>();
    [SerializeField] private SynergyPage[] _synergyPages;
    [SerializeField] private int _synergyPageMaxCount;
    [SerializeField] private int _currentPage;
    [SerializeField] private Button _pageButton;

    [SerializeField] private SynergyDescription _synergyDescription;
    private ESynergyUIMode _mode;
    protected override void Initialize()
    {
        base.Initialize();
        GenerateSynergyTexts();
        _pageButton.onClick.AddListener(SetPageButton);
    }

    private void GenerateSynergyTexts()
    {
        _currentPage = 0;
        foreach (var synergy in _synergySoList)
        {
            SynergyUIItem synergyUIItem = Instantiate(_synergyItem, transform).GetComponent<SynergyUIItem>();
            synergyUIItem.Initialize(synergy);
            synergyUIItem.ShowDescription += ShowDescription;
            synergyUIItem.CloseDescription += CloseDescription;
            _synergyInfoDic[synergy.SynergyType] = synergyUIItem;
            synergyUIItem.gameObject.SetActive(false);
        }
    }
    public void UpdateSynergies(SynergyInfo synergyInfo, ESynergyUIMode mode)
    {
        if (!gameObject.activeInHierarchy)
            return;
        _mode = mode;
        string text = "";
        if (synergyInfo.Count == 0 && _mode != ESynergyUIMode.SynergyTable)
        {
            _activeSynergyList.Remove(_synergyInfoDic[synergyInfo.SynergyType]);
            _synergyInfoDic[synergyInfo.SynergyType].gameObject.SetActive(false);
        }
        else
        {
            _synergyInfoDic[synergyInfo.SynergyType].gameObject.SetActive(true);

            if (!_activeSynergyList.Contains(_synergyInfoDic[synergyInfo.SynergyType]))
            {
                _activeSynergyList.Add(_synergyInfoDic[synergyInfo.SynergyType]);
            }

            if (synergyInfo.Level == 0)
            {
                text += "<color=grey>";
                text += $"{synergyInfo.Count} / {synergyInfo.NextLevelCount}";
            }
            else
            {
                for (int i = 0; i < synergyInfo.SynergySo.SynergyLevel.RequireCount.Length; i++)
                {
                    text += "<color=grey>";

                    if (i == synergyInfo.Level - 1)
                    {
                        text += "<color=white>";
                    }
                    text += synergyInfo.SynergySo.SynergyLevel.RequireCount[i];
                    text += "<color=grey>";

                    if (i != synergyInfo.SynergySo.SynergyLevel.RequireCount.Length - 1)
                    {
                        text += " > ";
                    }
                }
            }
            _synergyInfoDic[synergyInfo.SynergyType].SetSynergyItem(synergyInfo.Level, synergyInfo.Count, text, synergyInfo.Value);

        }
        _synergyInfoDic[synergyInfo.SynergyType].UpdateSynergyInfo(synergyInfo.Level);

        RefreshSynergyOrder();
        //foreach (var rectTransform in GetComponentsInChildren<RectTransform>())
        //{
        //    LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        //}
        //Canvas.ForceUpdateCanvases();
    }

    private void RefreshSynergyOrder()
    {
        _activeSynergyList = _activeSynergyList.OrderByDescending(x => x.Value).ToList();

        if (_activeSynergyList.Count == 0)
        {
            _currentPage = 0;
            _synergyPages[0].gameObject.SetActive(false);
            _synergyPages[1].gameObject.SetActive(false);
            _pageButton.gameObject.SetActive(false);
        }
        else if (_activeSynergyList.Count < _synergyPageMaxCount)
        {
            _currentPage = 0;
            _synergyPages[0].gameObject.SetActive(true);
            _synergyPages[1].gameObject.SetActive(false);
            _pageButton.gameObject.SetActive(false);
        }
        else if (_activeSynergyList.Count > _synergyPageMaxCount)
        {
            _pageButton.gameObject.SetActive(true);
        }
        for (int i = 0; i < _synergyPages.Length; i++)
        {
            _synergyPages[i].RemoveAllChild();
        }
        for (int i = 0; i < _activeSynergyList.Count; i++)
        {
            _synergyPages[i / _synergyPageMaxCount].SetChild(_activeSynergyList[i].transform);
        }
    }

    private void SetPageButton()
    {
        SoundManager.Instance.PlaySfxSound(Sfx.SE_ButtonSelect);
        _synergyPages[_currentPage].gameObject.SetActive(false);
        if (_currentPage == 0)
        {
            _currentPage = 1;
        }
        else
        {
            _currentPage = 0;
        }
        _synergyPages[_currentPage].gameObject.SetActive(true);
    }

    private void ShowDescription(SynergySo synergySo, int level)
    {

        _synergyDescription.SetDescription(synergySo.SynergyUiSo);
        if (_mode == ESynergyUIMode.InGame)
        {
            ToolTipManager.Instance.ShowSynergyTooltip(synergySo.SynergyType);
            _synergyDescription.ShowSynergyEffect(synergySo);
        }
        _synergyDescription.UpdateHighlight(level);
        _synergyDescription.gameObject.SetActive(true);
    }
    private void CloseDescription()
    {
        if (_mode == ESynergyUIMode.InGame)
        {
            ToolTipManager.Instance.HideSynergyTooltip();
        }
        _synergyDescription.CloseSynergyEffect();
        _synergyDescription.gameObject.SetActive(false);
    }
    #region Legacy
    /*
      private void GenerateSynergyTexts()
       {
           for (int i = 0; i < Enum.GetValues(typeof(EGearSynergy)).Length; i++)
           {
               GameObject synergyGob = Instantiate(_synergyText, transform);
               _synergyTextLists.Add(synergyGob.GetComponent<TextMeshProUGUI>());
               _synergyUIItemLists.Add(synergyGob.transform.GetChild(0).GetComponent<SynergyUIItem>());
               _synergyUIItemLists[i].Initialize(0, i);
               _synergyTextLists[i].gameObject.SetActive(false);
           }
       }
       public void UpdateSynergies(EGearSynergy type, int level, int count, int nextLevelCount, bool isMax = false)
       {

           if (count == 0)
           {
               _synergyTextLists[(int)type].gameObject.SetActive(false);
           }
           else
           {
               _synergyTextLists[(int)type].gameObject.SetActive(true);
               if (isMax)
               {
                   _synergyTextLists[(int)type].text = Enum.GetName(typeof(EGearSynergy), type) + $" Level:{level}/ Max";
               }
               else
               {
                   _synergyTextLists[(int)type].text = Enum.GetName(typeof(EGearSynergy), type) + $" Level:{level}/({count}/{nextLevelCount})";
               }
           }
           _synergyUIItemLists[(int)type].UpdateSynergyInfo(level);
       }

     */
    #endregion

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _synergySoList = FindObjectsInAsset<SynergySo>();
        _synergyPages = GetComponentsInChildren<SynergyPage>();
        _pageButton = FindGameObjectInChildren<Button>("PageButton");
        _synergyDescription = GetComponentInChildren<SynergyDescription>();
    }
#endif
}

public struct SynergyInfo
{
    private readonly SynergySo _synergySo;
    public SynergySo SynergySo { get { return _synergySo; } }
    private readonly ESynergyType _synergyType;
    public ESynergyType SynergyType { get { return _synergyType; } }
    private readonly int _level;
    public int Level { get { return _level; } }
    private readonly int _count;
    public int Count { get { return _count; } }
    private readonly int _nextLevelCount;
    public int NextLevelCount { get { return _nextLevelCount; } }
    private readonly bool _isMax;
    public bool IsMax { get { return _isMax; } }

    private int _value;
    public int Value { get { return _value; } }

    public SynergyInfo(SynergySo synergySo, ESynergyType synergyType, int level, int count, int nextLevelCount, bool isMax)
    {
        _synergySo = synergySo;
        _synergyType = synergyType;
        _level = level;
        _count = count;
        _nextLevelCount = nextLevelCount;
        _isMax = isMax;
        _value = (_level * 1000) + (_count * 10);
    }
}
