using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GearNameUI : BaseBehaviour
{
    [SerializeField] private TextMeshProUGUI _gearNameText;

    public void SetGearName(string name, bool isUpgrade)
    {
        _gearNameText.text = name.ToString();
        if (isUpgrade)
        {
            _gearNameText.color = Color.green;
            _gearNameText.text += "+";
        }
        else
        {
            _gearNameText.color = Color.white;
        }
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _gearNameText = FindGameObjectInChildren<TextMeshProUGUI>("GearNameText");
    }
#endif
}
