using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GSynergyUI : BaseBehaviour
{
    [SerializeField] private TextMeshPro _gSynergyText;
    public void SetText(float amount)
    {
        _gSynergyText.text = amount.ToString("F1");
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _gSynergyText = GetComponent<TextMeshPro>();
    }
#endif
}
