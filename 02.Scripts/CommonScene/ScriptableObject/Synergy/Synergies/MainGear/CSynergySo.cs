using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CSynergy", menuName = "Scriptable Objects/Synergy/MainGear/CSynergy", order = 3)]
public class CSynergySo : MainGearSynergySo
{
    [SerializeField] private float[] _speedRatios;
    private readonly float _epsilon = 0.0001f;


    public override void ApplySynergy()
    {
        _mainGear.OnGetStatIncreaseSpeedRatio += GetSynergyStatIncreaseSpeedRatio;
        _mainGear.OnSpeedChanged?.Invoke();
    }

    public override void RemoveSynergy()
    {
        _mainGear.OnGetStatIncreaseSpeedRatio -= GetSynergyStatIncreaseSpeedRatio;
        _mainGear.OnSpeedChanged?.Invoke();
    }

    private float GetSynergyStatIncreaseSpeedRatio()
    {
        if (Math.Abs(_mainGear.GetEfficiency() - 1) < _epsilon)
        {
            return _speedRatios[_level];
        }
        else
        {
            return 1;
        }
    }

    public override void SetLevel(int level)
    {
        base.SetLevel(level);
        _mainGear.OnSpeedChanged?.Invoke();
    }
}
