using System;
using UnityEngine;


[CreateAssetMenu(fileName = "YearningDebuff", menuName = "Scriptable Objects/Debuffs/YearningDebuff")]
public class YearningDebuffSo : DebuffSo
{
    private SupplySystem _supplySystem;
    private int _currentCount;
    [SerializeField] private int _maxCount;
    [SerializeField] private float _supplyTimeIncreaseAmount;
    [SerializeField] private int _supplyCountDecreaseAmount;

    public void InitializeYearningDebuffSo(SupplySystem supplySystem, Transform uiSpawnTrs)
    {
        _currentCount = 0;
        _supplySystem = supplySystem;
        _supplySystem.ActiveDebuff();
        _supplySystem.OnDebuffStackAdded -= RefreshDebuffStack;
        _supplySystem.OnDebuffStackAdded += RefreshDebuffStack;
        _isEnabled = true;
        _levelActions = new Action<bool>[_requireLevels.Length + 1];
        _levelActions[0] = null;
        _levelActions[1] = SetSupplyTimeDeubff;
        _levelActions[2] = SetSupplyCountDeubff;
        _hasStack = true;

    }
    private void RefreshDebuffStack(bool isDecreased)
    {
        if (isDecreased)
        {
            _currentCount--;
            if (_currentCount <= 0)
                _currentCount = 0;
        }
        else
        {
            _currentCount++;
            if (_currentCount >= _maxCount)
            {
                _currentCount = _maxCount;
            }
        }
        CheckLevel(_currentCount);
    }
    private void SetSupplyTimeDeubff(bool active)
    {
        if (active)
        {
            _supplySystem.OnGetSupplyIndecreaseTime -= GetSupplyDecreaseTime;
            _supplySystem.OnGetSupplyIndecreaseTime += GetSupplyDecreaseTime;
        }
        else
        {
            _supplySystem.OnGetSupplyIndecreaseTime -= GetSupplyDecreaseTime;
        }
    }
    private float GetSupplyDecreaseTime()
    {
        return _supplyTimeIncreaseAmount;
    }
    private void SetSupplyCountDeubff(bool active)
    {
        if (active)
        {
            _supplySystem.OnGetSupplyIndecreaseCount -= GetSupplyDecreaseAmount;
            _supplySystem.OnGetSupplyIndecreaseCount += GetSupplyDecreaseAmount;
        }
        else
        {
            _supplySystem.OnGetSupplyIndecreaseCount -= GetSupplyDecreaseAmount;
        }
    }
    private int GetSupplyDecreaseAmount()
    {
        return -_supplyCountDecreaseAmount;
    }

    public override int GetStackCount()
    {
        return _currentCount;
    }
}
