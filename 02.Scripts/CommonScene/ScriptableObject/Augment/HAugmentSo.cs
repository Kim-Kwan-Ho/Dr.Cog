using UnityEngine;



[CreateAssetMenu(fileName = "HAugment", menuName = "Scriptable Objects/Augments/HAugment", order = 8)]
public class HAugmentSo : AugmentSo
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
