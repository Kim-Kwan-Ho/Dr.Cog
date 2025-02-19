using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory/Inventory")]
public class InventorySo : ScriptableObject
{
    [SerializeField] private int _startMaxSlot;
    [SerializeField] private int _startInitialGearCount;
    private int _curMaxSlot;
    public int CurMaxSlot { get { return _curMaxSlot; } }
    private int _curInitialGearCount;
    public int CurInitialGearCount { get { return _curInitialGearCount; } }

    public void InitializeInventorySo()
    {
        _curMaxSlot = _startMaxSlot;
        _curInitialGearCount = _startInitialGearCount;
    }

    public void ChangeSlotAndGearCount(int slot, int gear)
    {
        _curMaxSlot += slot;
        _curInitialGearCount += gear;
    }

    public void ChangeInventoryStat(int slot, int startGear)
    {
        _curMaxSlot = slot;
        _curInitialGearCount = startGear;
    }
}
