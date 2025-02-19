using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CEpicUpgrade", menuName = "Scriptable Objects/EpicUpgrade/CEpicUpgrade", order = 3)]
public class CEpicUpgradeSo : EpicUpgradeSo
{
    [SerializeField] private InventorySo _InventoryStatSo;
    [SerializeField] private int _inventoryStartGearIncreaseAmount;

    public override void ApplyEpicUpgrade()
    {
        _InventoryStatSo.ChangeSlotAndGearCount(0, _inventoryStartGearIncreaseAmount);
    }
}
