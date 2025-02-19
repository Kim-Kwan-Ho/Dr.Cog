using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BAugment", menuName = "Scriptable Objects/Augments/BAugment", order = 2)]
public class BAugmentSo : AugmentSo
{
    [SerializeField] private PlayerStatSo _playerStatSo;
    [SerializeField] private PlayerStat _playerDecreaseStat;
    [SerializeField] private SupplySo _supplySo;
    [SerializeField] private int _supplyDecreaseCount;
    public override void ApplyAugment()
    {
        _playerStatSo.DecreaseStat(_playerDecreaseStat);
        _supplySo.DecreaseSupplyCount(_supplyDecreaseCount);
    }
}
