using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CAugment", menuName = "Scriptable Objects/Augments/CAugment", order = 3)]
public class CAugmentSo : AugmentSo
{
    [SerializeField] private StageAdditiveStatSo _additiveStatSo;
    [SerializeField] private StageAdditiveStat _additiveStat;
    public override void ApplyAugment()
    {
        _additiveStatSo.AddStageAdditiveStat(_additiveStat);
    }
}
