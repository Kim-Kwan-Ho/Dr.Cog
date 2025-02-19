using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardAugment : RewardObject
{
    [SerializeField] private Image _augmentImage;
    [SerializeField] private TextMeshProUGUI _augmentName;
    [SerializeField] private TextMeshProUGUI _augmentDescription;

    public void SetRewardAugment(int index, AugmentSo augmentSo)
    {
        _toggle.isOn = false;
        _index = index;
        _augmentImage.sprite = augmentSo.AugmentImage;
        _augmentName.text = augmentSo.Name.ToString();
        _augmentDescription.text = augmentSo.Description.ToString();
        _toggle.onValueChanged.AddListener((c) => OnRewardClick?.Invoke(_index));
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _augmentImage = FindGameObjectInChildren<Image>("AugmentImage");
        _augmentName = FindGameObjectInChildren<TextMeshProUGUI>("AugmentText");
        _augmentDescription = FindGameObjectInChildren<TextMeshProUGUI>("DescriptionText");
    }
#endif
}
