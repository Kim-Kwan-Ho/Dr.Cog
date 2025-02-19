
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SynergyUIItem : BaseBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Level")]
    [SerializeField] private GameObject _synergyLevelGroup;
    [SerializeField] private TextMeshProUGUI _synergyCountText;


    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _synergyNameText;
    [SerializeField] private TextMeshProUGUI _synergyText;

    [Header("Images")]
    [SerializeField] private Image _synergyImage;
    [SerializeField] private Image _synergyBackgroundImage;
    [SerializeField] private Color _backgroundImageBasicColor;
    [SerializeField] private Color[] _backgroundImageColors;


    [Header("Description")]
    private ESynergyType _synergyType;
    private int _level;
    private int _value;
    public int Value{get {return _value;}}

    private SynergySo _synergySo;
    public Action<SynergySo, int> ShowDescription;
    public Action CloseDescription;

    public ESynergyType GetSynergyType()
    {
        return _synergyType;

    }

    public void Initialize(SynergySo synergySo)
    {
        _synergySo = synergySo;
        _synergyType = synergySo.SynergyType;
        _synergyNameText.text = synergySo.SynergyUiSo  .SynergyName;
        _synergyImage.sprite = synergySo.SynergyUiSo.SynergyIcon;
        _synergyBackgroundImage.sprite = synergySo.SynergyUiSo.SynergyBackgroundImage;
        _backgroundImageBasicColor = synergySo.SynergyUiSo.SynergyBasicColor;
        _backgroundImageColors = synergySo.SynergyUiSo.SynergyLvlColor;
    }

    public void UpdateSynergyInfo(int synergyLvl)
    {
        if (_synergyImage != null)
        {
            if (synergyLvl > 0)
            {
                _synergyBackgroundImage.color = _backgroundImageColors[synergyLvl - 1];
                _synergyImage.color = Color.black;
            }
            else
            {
                _synergyImage.color = _backgroundImageBasicColor;
                _synergyBackgroundImage.color = _backgroundImageBasicColor;
            }
        }
    }
    public void SetSynergyItem(int level,int count ,string info, int value)
    {
        _value = value;
        _level = level;
        if (level == 0)
        {
            _synergyLevelGroup.gameObject.SetActive(false);
        }
        else
        {
            _synergyLevelGroup.gameObject.SetActive(true);
            _synergyCountText.text = count.ToString();
        }
        _synergyText.text = info;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowDescription?.Invoke(_synergySo, _level);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CloseDescription?.Invoke();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _synergyLevelGroup = FindGameObjectInChildren("SynergyLevelGroup");
        _synergyCountText = FindGameObjectInChildren<TextMeshProUGUI>("SynergyCountText");
        _synergyNameText = FindGameObjectInChildren<TextMeshProUGUI>("SynergyNameText");
        _synergyImage = FindGameObjectInChildren<Image>("SynergyIcon");
        _synergyBackgroundImage = FindGameObjectInChildren<Image>("SynergyBackground");
        _synergyText = FindGameObjectInChildren<TextMeshProUGUI>("SynergyText");
    }
#endif
}
