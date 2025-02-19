using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ASynergy", menuName = "Scriptable Objects/Synergy/MainGear/ASynergy", order = 1)]
public class ASynergySo : MainGearSynergySo
{
    [SerializeField] private int[] _requireAmount;
    private int _currentAmount;
    private int _effectAmount;
    public override void InitializeSynergySo()
    {
        base.InitializeSynergySo();
        _currentAmount = 0;
        _effectAmount = 0;
        _mainGear = null;
    }

    public override void ApplySynergy()
    {
        _mainGear.OnMemoryIncreased += OnMemoryIncreased;
    }

    public override void RemoveSynergy()
    {
        _mainGear.OnMemoryIncreased -= OnMemoryIncreased;
    }

    private void OnMemoryIncreased()
    {
        if (!_isActive || _level == 0)
            return;

        _currentAmount++;
        if (CheckCanIncreasePower())
        {
            IncreasePower();
        }
    }

    private bool CheckCanIncreasePower()
    {
        return _currentAmount >= _requireAmount[_level];
    }

    private void IncreasePower()
    {
        _effectAmount++;
        _mainGear.IncreasePower();
        _currentAmount = 0;
    }

    public override void SetLevel(int count)
    {
        _level = CheckSynergyLevel(count);
        if (!_isActive)
        {
            if (_level > 0)
            {
                _isActive = true;
                ApplySynergy();
                //_currentAmount = _requireAmount[_level];
            }
        }
        else
        {
            if (_level == 0)
            {
                RemoveSynergy();
                _isActive = false;
                //_currentAmount = _requireAmount[_level];
            }
        }
    }

    public override int GetStack()
    {
        return Math.Max(_requireAmount[_level] - _currentAmount, 1);
    }

    public override string GetSynergyEffect()
    {
        return "+" + _effectAmount.ToString();
    }
}
