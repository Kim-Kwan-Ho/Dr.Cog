using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "GearStat_", menuName = "Scriptable Objects/Gears/GearStat")]
public class GearStatSo : ScriptableObject
{

    [Header("Synergy")]
    [SerializeField] private List<SynergySo> _synergyList;
    public List<SynergySo> SynergyList { get { return _synergyList; } }
    [SerializeField] private List<SubGearSynergySo> _subGearSynergyList;
    public List<SubGearSynergySo> SubGearSynergyList { get { return _subGearSynergyList; } }

    [Header("Stats")]
    [SerializeField] private GearMemory[] _gearMemories;
    public GearMemory[] GearMemories { get { return _gearMemories; } }

    [SerializeField] private int _maxLevel;
    public int MaxLevel { get { return _maxLevel; } }

    private List<AugmentSynergyStat> _additiveMemoryList;
    public List<AugmentSynergyStat> AdditiveMemoryList { get { return AdditiveMemoryList; } }

    private List<AugmentLevelStat> _additiveMemoryByLevel;
    public List<AugmentLevelStat> AdditiveMemoryByLevel { get { return _additiveMemoryByLevel; } }

    public void InitializeGearSo()
    {
        _additiveMemoryList = new List<AugmentSynergyStat>(_synergyList.Count);
        _additiveMemoryByLevel = new List<AugmentLevelStat>();
    }
    public float GetTotalMemory(bool upgrade, int level)
    {
        float amount = _gearMemories[upgrade ? 1 : 0].Memory[level];

        foreach (var additiveMemory in _additiveMemoryList)
        {
            amount += additiveMemory.SumAdditiveAmount;
        }
        foreach (var additiveMemory in _additiveMemoryByLevel)
        {
            amount += additiveMemory.SumAdditiveAmount[level];
        }
        return amount;
    }

    public void AddAugmentStat(AugmentSynergyStat augmentStat)
    {
        _additiveMemoryList.Add(augmentStat);
    }

    public void AddAugmentStatByLevel(AugmentLevelStat augmentStat)
    {
        _additiveMemoryByLevel.Add(augmentStat);
    }

    public float GetMemory(bool upgrade, int level)
    {
        return _gearMemories[upgrade ? 1 : 0].Memory[level];
    }
    public float GetAdditiveMemory(int level)
    {
        float amount = 0;
        foreach (var additiveMemory in _additiveMemoryList)
        {
            amount += additiveMemory.SumAdditiveAmount;
        }
        foreach (var additiveMemory in _additiveMemoryByLevel)
        {
            amount += additiveMemory.SumAdditiveAmount[level];
        }
        return amount;
    }
}

[Serializable]
public struct GearMemory
{
    public float[] Memory;
}

[Serializable]

public struct AugmentLevelStat
{
    [SerializeField] private float[] _sumAdditiveAmount;
    public float[] SumAdditiveAmount { get { return _sumAdditiveAmount; } }
}
