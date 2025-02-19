using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardGearInfo : BaseBehaviour
{
    [SerializeField] private GameObject _nullInfo;
    [SerializeField] private GearInfo _gearInfo;

    protected override void Initialize()
    {
        base.Initialize();
        SetNullInfo();
    }

    public void SetGearInfos(GearSo gearSo, bool isUpgrade, int level)
    {
        _nullInfo.gameObject.SetActive(false);
        _gearInfo.gameObject.SetActive(true);
        _gearInfo.SetGearInfo(gearSo, isUpgrade, level);
    }

    public void SetNullInfo()
    {
        _nullInfo.gameObject.SetActive(true);
        _gearInfo.gameObject.SetActive(false);
    }



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _nullInfo = FindGameObjectInChildren("NullInfo");
        _gearInfo = FindGameObjectInChildren<GearInfo>("GearInfo");
    }
#endif
}
