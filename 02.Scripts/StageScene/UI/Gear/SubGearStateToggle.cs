using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubGearStateToggle : BaseBehaviour
{
    [SerializeField] private ESubGearStatState _state;
    [SerializeField] private Toggle _toggle;
    private Action<ESubGearStatState> _toggleAction;
    protected override void Initialize()
    {
        base.Initialize();
        _toggle.onValueChanged.AddListener(c => CheckAction());
    }

    public void SetToggleAction(Action<ESubGearStatState> aciton)
    {
        _toggleAction = aciton;
    }

    private void CheckAction()
    {
        SoundManager.Instance.PlaySfxSound(Sfx.SE_ButtonSelect);
        if (_toggle.isOn)
            _toggleAction?.Invoke(_state);
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _toggle = GetComponent<Toggle>();

    }
#endif
}
