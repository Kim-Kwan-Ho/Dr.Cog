using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RewardAddGear : RewardObject
{
    [SerializeField] private GearNameUI _gearNameUi;
    [SerializeField] private GearStatUI _gearStatUi;
    [SerializeField] private GearImageUI _gearImageUi;
    [SerializeField] private SynergyInfoUI[] _synergyInfoUis;
    [SerializeField] private Vector3 _descriptionOffSet;
    [SerializeField] private Vector3 _descriptionScale;

    public void SetRewardAddGear(int index, GearSo gearSo)
    {
        _index = index;
        _gearNameUi.SetGearName(gearSo.GearName, false);
        _gearStatUi.SetMemoryText(gearSo.GearStat, false);
        _gearImageUi.SetGearImage(gearSo.GearImage, false, 0);

        for (int i = 0; i < _synergyInfoUis.Length; i++)
        {
            _synergyInfoUis[i].SetSynergyInfo(gearSo.GearStat.SynergyList[i].SynergyUiSo, _descriptionOffSet, _descriptionScale);
        }
        _toggle.isOn = false;
        _toggle.onValueChanged.AddListener((c) => OnRewardClick?.Invoke(_index));

    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _gearNameUi = GetComponentInChildren<GearNameUI>();
        _gearStatUi = GetComponentInChildren<GearStatUI>();
        _gearImageUi = GetComponentInChildren<GearImageUI>();
        _synergyInfoUis = GetComponentsInChildren<SynergyInfoUI>();
    }
#endif
}
