using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardGearSynergyInfo : BaseBehaviour
{
    [SerializeField] private GearNameUI _gearNameUi;
    [SerializeField] private SynergyInfoUI[] _synergyInfoUis;

    public void SetRewardGearSynergyInfo(PlayerGear playerGear)
    {
        _gearNameUi.SetGearName(playerGear.GearSo.GearName, playerGear.Upgrade);
        for (int i = 0; i < _synergyInfoUis.Length; i++)
        {
            _synergyInfoUis[i].SetSynergyInfo(playerGear.GearSo.GearStat.SynergyList[i].SynergyUiSo);
        }
    }
    public void SetRewardGearSynergyInfo(GearSo gearSo)
    {
        _gearNameUi.SetGearName(gearSo.GearName, false);
        for (int i = 0; i < _synergyInfoUis.Length; i++)
        {
            _synergyInfoUis[i].SetSynergyInfo(gearSo.GearStat.SynergyList[i].SynergyUiSo);
        }
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _gearNameUi = GetComponentInChildren<GearNameUI>();
        _synergyInfoUis = GetComponentsInChildren<SynergyInfoUI>();
    }
#endif


}
