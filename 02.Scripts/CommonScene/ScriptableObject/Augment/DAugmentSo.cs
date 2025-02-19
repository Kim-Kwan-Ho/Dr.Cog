using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DAugment", menuName = "Scriptable Objects/Augments/DAugment", order = 4)]
public class DAugmentSo : AugmentSo
{
    [SerializeField] private StageAdditiveStatSo _additiveStatSo;
    [SerializeField] private StageAdditiveStat _additiveStat;
    public override void ApplyAugment()
    {
        _additiveStatSo.AddStageAdditiveStat(_additiveStat);
    }
}
