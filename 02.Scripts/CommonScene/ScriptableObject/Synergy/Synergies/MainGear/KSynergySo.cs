using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KSynergy", menuName = "Scriptable Objects/Synergy/MainGear/KSynergy", order = 11)]
public class KSynergySo : MainGearSynergySo
{
    [SerializeField] private float[] _increaseByLevels;

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
        var count = _mainGear.GetOverGearCount();
        if (count > 0)
        {
            return (count * _increaseByLevels[_level]) / 100;
        }
        else
        {
            return 0;
        }

    }
    public override string GetSynergyEffect()
    {
        var count = _mainGear.GetOverGearCount();
        float value = 0;
        if (count > 0)
        {
            value = (count * _increaseByLevels[_level]);
        }
        else
        {
            value = 0;
        }
        return value.ToString() + "%";
    }

}
