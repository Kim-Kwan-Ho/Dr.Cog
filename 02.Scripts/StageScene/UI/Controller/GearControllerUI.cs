using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearControllerUI : BaseBehaviour
{
    [SerializeField] private SubGearStateToggle[] _toggles;




    public void SetControllerUI(Action<ESubGearStatState> action)
    {
        foreach (var subGearStateToggle in _toggles)
        {
            subGearStateToggle.SetToggleAction(action);
        }
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _toggles = GetComponentsInChildren<SubGearStateToggle>();
    }
#endif
}
