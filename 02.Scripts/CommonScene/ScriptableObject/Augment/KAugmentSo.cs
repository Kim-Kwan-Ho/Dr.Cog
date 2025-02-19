using UnityEngine;

[CreateAssetMenu(fileName = "KAugment", menuName = "Scriptable Objects/Augments/KAugment", order = 11)]
public class KAugmentSo : AugmentSo
{
    [SerializeField] private PlayerStatSo _playerStatSo;
    [SerializeField] private PlayerStat _increasePlayerStat;
    public override void ApplyAugment()
    {
        _playerStatSo.IncreaseStat(_increasePlayerStat);
    }
}