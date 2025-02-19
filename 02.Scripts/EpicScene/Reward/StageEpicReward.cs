using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEpicReward : BaseBehaviour
{
    [SerializeField] private EpicUpgradeSo[] _epicUpgrades;

    public EpicUpgradeSo[] GetAllEpicUpgrade()
    {
        return _epicUpgrades;
    }
}
