using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearImageUI : BaseBehaviour
{
    [SerializeField] private Image _gearImage;
    [SerializeField] private Image _gearIconImage;
    [SerializeField] private Image _gearUpgradeImage;
    [SerializeField] private Image _gearLevelImage;
    public void SetGearImage(GearImageSo gearImageSo, bool upgrade, int level)
    {
        _gearImage.sprite = gearImageSo.GearImage;
        _gearImage.color = gearImageSo.BodyColor;
        _gearIconImage.sprite = gearImageSo.FeelingIconImage;
        if (upgrade)
        {
            _gearUpgradeImage.sprite = gearImageSo.GearUpgradeImage;
            _gearUpgradeImage.color = gearImageSo.BodyColor;
        }
        else
        {
            _gearUpgradeImage.gameObject.SetActive(false);
        }

        _gearLevelImage.sprite = gearImageSo.GearMergeSo.GearMergeImage;
        _gearLevelImage.color = gearImageSo.GearMergeSo.BodyColor[level];
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _gearImage = FindGameObjectInChildren<Image>("GearImage");
        _gearIconImage = FindGameObjectInChildren<Image>("GearIconImage");
        _gearUpgradeImage = FindGameObjectInChildren<Image>("GearUpgradeImage");
        _gearLevelImage = FindGameObjectInChildren<Image>("GearLevelImage");
    }
#endif
}
