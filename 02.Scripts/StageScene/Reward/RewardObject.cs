using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class RewardObject : BaseBehaviour
{
    [SerializeField] protected Toggle _toggle;
    public Action<int> OnRewardClick;
    protected int _index;


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _toggle = GetComponent<Toggle>();
    }
#endif
}
