using System;
using UnityEngine;

[CreateAssetMenu(fileName = "JSynergy", menuName = "Scriptable Objects/Synergy/MainGear/JSynergy", order = 10)]
public class JSynergySo : MainGearSynergySo
{
    [SerializeField] private GearMemoryIncreaseByLevel[] _increaseByLevels;

    public override void InitializeSynergySo()
    {
        base.InitializeSynergySo();
        _mainGear = null;
    }

    public override void ApplySynergy()
    {
        _mainGear.OnGetAdditiveStatRatio += GetSynergyAdditiveStatRatio;

    }

    public override void RemoveSynergy()
    {
        _mainGear.OnGetAdditiveStatRatio -= GetSynergyAdditiveStatRatio;

    }

    private float GetSynergyAdditiveStatRatio()
    {
        var counts = _mainGear.GetGearsCountByLevel();
        return (counts.three * _increaseByLevels[_level].LevelThree + counts.four * _increaseByLevels[_level].LevelFour) / 100f;
    }

    public override string GetSynergyEffect()
    {
        var counts = _mainGear.GetGearsCountByLevel();
        return (counts.three * _increaseByLevels[_level].LevelThree + counts.four * _increaseByLevels[_level].LevelFour).ToString() + "%";
    }
}

[Serializable]
public struct GearMemoryIncreaseByLevel
{
    [SerializeField] private int _levelThree;
    public int LevelThree { get { return _levelThree; } }
    [SerializeField] private int _levelFour;
    public int LevelFour { get { return _levelFour; } }
}