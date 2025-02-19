using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AAugment_", menuName = "Scriptable Objects/Augments/AAugment", order = 1)]
public class AAugmentSo : AugmentSo
{

    [SerializeField] private AugmentSynergyStat _augmentSynergyStat;



    public override void ApplyAugment()
    {
        var statSoList = GearDataManager.Instance.GetSynergyGears(_augmentSynergyStat.Synergy);
        foreach (var gearStatSo in statSoList)
        {
            gearStatSo.AddAugmentStat(_augmentSynergyStat);
        }
    }
}

[Serializable]
public struct AugmentSynergyStat
{
    [SerializeField] private SynergySo _synergy;
    public SynergySo Synergy { get { return _synergy; } }
    [SerializeField] private float _sumAdditiveAmount;
    public float SumAdditiveAmount { get { return _sumAdditiveAmount; } }


}
