using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ISynergy", menuName = "Scriptable Objects/Synergy/Combined/ISynergy", order = 9)]
public class ISynergySo : MainGearSynergySo, ISynergyInventoryCombiner
{
    [SerializeField] private int[] _requireAmount;
    private int _currentAmount;
    private int _effectAmount;

    public override void InitializeSynergySo()
    {
        base.InitializeSynergySo();
        _mainGear = null;
        _currentAmount = 0;
        _effectAmount = 0;
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
        if (CheckCanAddGear())
        {
            AddSynergyGear();
        }
    }
    private bool CheckCanAddGear()
    {
        return _currentAmount >= _requireAmount[_level];
    }

    private void AddSynergyGear()
    {
        _effectAmount++;
        Inventory.AddSynergyGear(this);
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
            }
        }
        else
        {
            if (_level == 0)
            {
                RemoveSynergy();
                _isActive = false;
                _currentAmount = 0;
            }
        };
    }

    public override int GetStack()
    {
        return Math.Max(_requireAmount[_level] - _currentAmount, 1);
    }

    public override string GetSynergyEffect()
    {
        return "+" + _effectAmount.ToString();
    }

    public InventorySystem Inventory { get; set; }
    public void SetInventory(InventorySystem inventory)
    {
        Inventory = inventory;
    }
}