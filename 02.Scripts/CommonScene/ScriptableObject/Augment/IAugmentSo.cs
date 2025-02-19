using UnityEngine;



[CreateAssetMenu(fileName = "IAugment", menuName = "Scriptable Objects/Augments/IAugment", order = 9)]
public class IAugmentSo : AugmentSo
{
    [SerializeField] private SupplySo _supplySo;
    [SerializeField] private int _supplyTimeDecreaseAmount;
    [SerializeField] private PlayerStatSo _playerStatSo;
    [SerializeField] private PlayerStat _playerIncreaseStat;
    public override void ApplyAugment()
    {
        _supplySo.DecreaseSupplyTime(_supplyTimeDecreaseAmount);
        _playerStatSo.IncreaseStat(_playerIncreaseStat);
    }
}