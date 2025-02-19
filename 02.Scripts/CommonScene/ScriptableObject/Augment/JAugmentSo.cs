using UnityEngine;



[CreateAssetMenu(fileName = "JAugment", menuName = "Scriptable Objects/Augments/JAugment", order = 10)]
public class JAugmentSo : AugmentSo
{
    [SerializeField] private SupplySo _supplySo;
    [SerializeField] private int _supplyCountIncreaseAmount;
    [SerializeField] private InventorySo _inventorySo;
    [SerializeField] private int _inventoryStartGearDecreaseAmount;
    public override void ApplyAugment()
    {
        _supplySo.IncreaseSupplyCount(_supplyCountIncreaseAmount);
        _inventorySo.ChangeSlotAndGearCount(0, -_inventoryStartGearDecreaseAmount);
    }
}