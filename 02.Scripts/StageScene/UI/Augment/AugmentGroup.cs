using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentGroup : BaseBehaviour
{
    [SerializeField] private PlayerAugmentSo _playerAugmentSo;
    [SerializeField] private AugmentInfo[] _augmentInfos;


    protected override void Initialize()
    {
        base.Initialize();
        for (int i = 0; i < _playerAugmentSo.AugmentList.Count; i++)
        {
            _augmentInfos[i].gameObject.SetActive(true);
            _augmentInfos[i].SetAugmentInfo(_playerAugmentSo.AugmentList[i]);
        }
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _playerAugmentSo = FindObjectInAsset<PlayerAugmentSo>();
        _augmentInfos = GetComponentsInChildren<AugmentInfo>();
    }
#endif

}
