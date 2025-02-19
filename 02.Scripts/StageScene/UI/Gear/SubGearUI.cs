using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ESubGearStatState
{
    None,
    Level,
    Direction,
    Stat
}


public class SubGearUI : BaseBehaviour
{
    [SerializeField] private SubGearIconSo _subGearIconSo;
    [SerializeField] private Image _feelingIconImage;
    [SerializeField] private Image _directionImage;
    [SerializeField] private Color _redColor;
    [SerializeField] private Color _blueColor;
    [SerializeField] private TextMeshProUGUI _statText;
    [SerializeField] private TextMeshProUGUI _levelText;


    public void SetStateUI(ESubGearStatState state)
    {
        DisableAllStat();

        switch (state)
        {
            case ESubGearStatState.Level:
                _levelText.gameObject.SetActive(true);
                break;
            case ESubGearStatState.Direction:
                _directionImage.gameObject.SetActive(true);
                break;
            case ESubGearStatState.Stat:
                _statText.gameObject.SetActive(true);
                break;
            default:
                _feelingIconImage.gameObject.SetActive(true);
                break;
        }
    }

    public void UpdateIconUI(Sprite iconImage)
    {
        _feelingIconImage.sprite = iconImage;
    }

    public void UpdateUI(GearContainer gearContainer, ESubGearStatState state)
    {
        _statText.text = Utils.Utilities.GetGearSignString(gearContainer.GearSo.GearStat.GetTotalMemory(gearContainer.Upgrade, gearContainer.Level)) + gearContainer.GearSo.GearStat.GetTotalMemory(gearContainer.Upgrade, gearContainer.Level).ToString();
        _levelText.text = (gearContainer.Level + 1).ToString();
        _feelingIconImage.sprite = gearContainer.GearSo.GearImage.FeelingIconImage;
        SetStateUI(state);

    }
    public void UpdateUI(GearContainer gearContainer)
    {
        _statText.text = Utils.Utilities.GetGearSignString(gearContainer.GearSo.GearStat.GetTotalMemory(gearContainer.Upgrade, gearContainer.Level)) + gearContainer.GearSo.GearStat.GetTotalMemory(gearContainer.Upgrade, gearContainer.Level).ToString();
        _levelText.text = (gearContainer.Level + 1).ToString();
    }

    public void SetIconDirection(bool isClockwise)
    {
        _directionImage.sprite = isClockwise ? _subGearIconSo.ClockWiseSprite : _subGearIconSo.CounterClockWiseSprite;
        _directionImage.color = isClockwise ? _blueColor : _redColor;
    }
    private void DisableAllStat()
    {
        _directionImage.gameObject.SetActive(false);
        _statText.gameObject.SetActive(false);
        _levelText.gameObject.SetActive(false);
        _feelingIconImage.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _statText = FindGameObjectInChildren<TextMeshProUGUI>("StatText");
        _feelingIconImage = FindGameObjectInChildren<Image>("FeelingImage");
        _directionImage = FindGameObjectInChildren<Image>("DirectionImage");
        _levelText = FindGameObjectInChildren<TextMeshProUGUI>("LevelText");
        _subGearIconSo = FindObjectInAsset<SubGearIconSo>();
    }
#endif
}
