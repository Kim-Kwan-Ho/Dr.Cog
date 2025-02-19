using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FSynergy", menuName = "Scriptable Objects/Synergy/MainGear/FSynergy", order = 6)]
public class FSynergySo : MainGearSynergySo
{
    [SerializeField] private float[] _additiveStats;
    private readonly float _epsilon = 0.0001f;
 
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
        if (Math.Abs(_mainGear.GetEfficiency() - 1) < _epsilon)
        {
            return (_additiveStats[_level] / 100f);
        }
        else
        {
            return 0;
        }
    }
}