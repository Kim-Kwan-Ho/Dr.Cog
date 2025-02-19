using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Achievement", menuName = "Scriptable Objects/Achievement/Achievement")]
public class AchievementSo : ScriptableObject
{
    [SerializeField] private int _gearAddCount;
    public int GearAddCount { get { return _gearAddCount; } }
    [SerializeField] private int _gearRemoveCount;
    public int GearRemoveCount { get { return _gearRemoveCount; } }
    [SerializeField] private int _gearUpgradeCount;
    public int GearUpgradeCount { get { return _gearUpgradeCount; } }
    [SerializeField] private int _augmentAddCount;
    public int AugmentAddCount { get { return _augmentAddCount; } }

    [SerializeField] private int _minusGearSupplyCount;
    public int MinusGearSupplyCount { get { return _minusGearSupplyCount; } }
}
