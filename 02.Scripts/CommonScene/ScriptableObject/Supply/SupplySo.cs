using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Supply", menuName = "Scriptable Objects/Supply/Supply")]
public class SupplySo : ScriptableObject
{
    [SerializeField] private float _startSupplyTime;
    [SerializeField] private int _startSupplyCount;

    private float _curSupplyTime;
    public float CurSupplyTime { get { return _curSupplyTime; } }
    private int _curSupplyCount;
    public int CurSupplyCount { get { return _curSupplyCount; } }


    public void InitializeSupplySo()
    {
        _curSupplyTime = _startSupplyTime;
        _curSupplyCount = _startSupplyCount;
    }

    public void DecreaseSupplyTime(float amount)
    {
        _curSupplyTime -= amount;
    }

    public void IncreaseSupplyCount(int amount)
    {
        _curSupplyCount += amount;
    }

    public void DecreaseSupplyCount(int amount)
    {
        _curSupplyCount -= amount;
    }

    public void ChangeSupplyStat(float time, int count)
    {
        _curSupplyTime = time;
        _curSupplyCount = count;
    }
}
