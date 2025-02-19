using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

public class RewardPlayerGear : RewardObject, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GearImageUI _gearImageUI;
    [SerializeField] private RewardGearSynergyInfo _synergyInfo;
    public void SetRewardRemoveGear(int index, PlayerGear playerGear)
    {
        _toggle.isOn = false;
        _index = index;
        _gearImageUI.SetGearImage(playerGear.GearSo.GearImage, playerGear.Upgrade, 0);
        _synergyInfo.SetRewardGearSynergyInfo(playerGear);

        _toggle.onValueChanged.AddListener((c) => OnRewardClick?.Invoke(_index));
        _synergyInfo.gameObject.SetActive(false);
    }
    public void SetRewardCheckGear(int index, GearSo gearSo)
    {
        _toggle.isOn = false;
        _index = index;
        _gearImageUI.SetGearImage(gearSo.GearImage, false, 0);
        _synergyInfo.SetRewardGearSynergyInfo(gearSo);

        _toggle.onValueChanged.AddListener((c) => OnRewardClick?.Invoke(_index));
        _synergyInfo.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _synergyInfo.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _synergyInfo.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _gearImageUI = GetComponentInChildren<GearImageUI>();
        _synergyInfo = GetComponentInChildren<RewardGearSynergyInfo>();
    }
#endif

}
