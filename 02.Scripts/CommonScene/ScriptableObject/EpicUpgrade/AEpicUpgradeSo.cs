using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AEpicUpgrade", menuName = "Scriptable Objects/EpicUpgrade/AEpicUpgrade", order = 1)]
public class AEpicUpgradeSo : EpicUpgradeSo
{
    [SerializeField] private PlayerStatSo _playerStatSo;
    [SerializeField] private PlayerStat _playerIncreaseStat;

    public override void ApplyEpicUpgrade()
    {
        _playerStatSo.IncreaseStat(_playerIncreaseStat);
    }
}
