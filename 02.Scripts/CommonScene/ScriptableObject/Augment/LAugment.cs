using UnityEngine;


[CreateAssetMenu(fileName = "LAugment", menuName = "Scriptable Objects/Augments/LAugment", order = 12)]

public class LAugment : AugmentSo
{
    [SerializeField] private SupplySo _supplySo;
    [SerializeField] private int _supplyTimeDecreaseAmount;
    [SerializeField] private PlayerStatSo _playerStatSo;
    [SerializeField] private PlayerStat _playerDecreaseStat;
    public override void ApplyAugment()
    {
        _supplySo.DecreaseSupplyTime(_supplyTimeDecreaseAmount);
        _playerStatSo.DecreaseStat(_playerDecreaseStat);
    }
}
