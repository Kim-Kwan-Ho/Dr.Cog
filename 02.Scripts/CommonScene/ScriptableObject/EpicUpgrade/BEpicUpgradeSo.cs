using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BEpicUpgrade", menuName = "Scriptable Objects/EpicUpgrade/BEpicUpgrade", order = 2)]
public class BEpicUpgradeSo : EpicUpgradeSo
{
    [SerializeField] private PlayerStatSo _playerStatSo;
    [SerializeField] private PlayerStat _playerDecreaseStat;

    public override void ApplyEpicUpgrade()
    {
        _playerStatSo.DecreaseStat(_playerDecreaseStat);
    }
}
