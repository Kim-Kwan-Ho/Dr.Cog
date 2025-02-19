using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearInfo : BaseBehaviour
{
    [SerializeField] private GearImageUI _gearImageUi;
    [SerializeField] private GearStatUI _gearStatUi;
    [SerializeField] private GearNameUI _gearNameUi;


    public void SetGearInfo(GearSo gearSo, bool isUpgrade, int level)
    {
        _gearNameUi.SetGearName(gearSo.GearName, isUpgrade);
        _gearImageUi.SetGearImage(gearSo.GearImage, isUpgrade, 0);
        _gearStatUi.SetMemoryText(gearSo.GearStat, isUpgrade);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _gearNameUi = GetComponentInChildren<GearNameUI>();
        _gearImageUi = GetComponentInChildren<GearImageUI>();
        _gearStatUi = GetComponentInChildren<GearStatUI>();
    }
#endif
}
