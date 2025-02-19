using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "GAugment", menuName = "Scriptable Objects/Augments/GAugment", order = 7)]
public class GAugmentSo : AugmentSo
{
    [SerializeField] private AugmentLevelStat _augmentLevelStat;
    public override void ApplyAugment()
    {
        var gears = GearDataManager.Instance.GetAllGears();
        foreach (var gear in gears)
        {
            gear.AddAugmentStatByLevel(_augmentLevelStat);
        }
    }
}
