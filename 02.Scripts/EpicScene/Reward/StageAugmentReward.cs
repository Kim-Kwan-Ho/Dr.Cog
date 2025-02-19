using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAugmentReward : BaseBehaviour
{
    [SerializeField] private PlayerAugmentSo _playerAugmentSo;
    [SerializeField] private AugmentSo[] _augments;



    public AugmentSo[] GetRandomAugment(int count)
    {
        List<AugmentSo> augmentList = new List<AugmentSo>();

        while (augmentList.Count < count)
        {
            int r = Random.Range(0, _augments.Length);

            if (!_playerAugmentSo.AugmentList.Contains(_augments[r]) && (!augmentList.Contains(_augments[r])))
            {
                augmentList.Add(_augments[r]);
            }
        }
        return augmentList.ToArray();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _augments = FindObjectsInAsset<AugmentSo>().ToArray();
    }
#endif
}
