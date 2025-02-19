using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FAugment", menuName = "Scriptable Objects/Augments/FAugment", order = 6)]
public class FAugmentSo : AugmentSo
{
    [SerializeField] private InventorySo _inventorySo;
    [SerializeField] private int _decreaseSlotCount;
    [SerializeField] private int _increaseInitialGearCount;

    public override void ApplyAugment()
    {
        _inventorySo.ChangeSlotAndGearCount(-_decreaseSlotCount, _increaseInitialGearCount);
    }
}
