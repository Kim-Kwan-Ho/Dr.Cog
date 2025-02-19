using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class RewardEpicUpgrade : RewardObject
{
    [SerializeField] private TextMeshProUGUI _epicRewardNameText;
    //[SerializeField] private TextMeshProUGUI _epicRewardStatText;
    [SerializeField] private TextMeshProUGUI _epicRewardDescriptionText;
    [SerializeField] private Image _epicRewardImage;


    public void SetRewardEpicUpgrade(int index,EpicUpgradeSo epicUpgradeSo)
    {
        //_gearNameText.text = gearSo.GearName;
        _toggle.isOn = false;
        _index = index;
        _epicRewardNameText.text = epicUpgradeSo.Name;
        _epicRewardDescriptionText.text = epicUpgradeSo.Description;

        _epicRewardImage.sprite = epicUpgradeSo.UpgradeSoIcon;
        _epicRewardImage.color = epicUpgradeSo.IconColor;
        _toggle.onValueChanged.AddListener((c) => OnRewardClick?.Invoke(_index));

    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _epicRewardNameText = FindGameObjectInChildren<TextMeshProUGUI>("GearNameText");
        _epicRewardDescriptionText = FindGameObjectInChildren<TextMeshProUGUI>("EpicRewardDescriptionText");
        _epicRewardImage = FindGameObjectInChildren<Image>("GearImage");
    }
#endif
}
