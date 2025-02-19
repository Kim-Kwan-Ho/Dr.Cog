using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerAugment", menuName = "Scriptable Objects/Player/PlayerAugment")]
public class PlayerAugmentSo : ScriptableObject
{
    private List<AugmentSo> _augmentList;
    public List<AugmentSo> AugmentList { get { return _augmentList; } }
    public void InitializePlayerAugmentSo()
    {
        _augmentList = new List<AugmentSo>();
    }

    public void AddPlayerAugment(AugmentSo augmentSo)
    {
        augmentSo.ApplyAugment();
        _augmentList.Add(augmentSo);
    }



}
