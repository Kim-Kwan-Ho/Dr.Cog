using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(fileName = "EpicStage_", menuName = "Scriptable Objects/Stages/EpicStage")]
public class EpicStageInfoSo : StageInfoSo
{
    [SerializeField] private DebuffSo _debuff;
    public DebuffSo Debuff { get { return _debuff; } }
}
