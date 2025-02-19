using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GearStatUI : BaseBehaviour
{

    [SerializeField] private TextMeshProUGUI _gearStatText;


    public void SetMemoryText(GearStatSo gearStat, bool upgrade)
    {
        _gearStatText.text = "[";
        for (int i = 0; i < gearStat.GearMemories[0].Memory.Length; i++)
        {
            float additiveMemory = gearStat.GetAdditiveMemory(i);
            if (additiveMemory  != 0)
            {
                if (additiveMemory > 0)
                {
                    _gearStatText.text += "<color=green>";
                }
                else
                {
                    _gearStatText.text += "<color=red>";
                }
            }
            _gearStatText.text += gearStat.GetTotalMemory(upgrade, i).ToString();
            _gearStatText.text += "<color=white>";
            if (i != gearStat.GearMemories[0].Memory.Length - 1)
            {
                _gearStatText.text += "/"; 
            }
        }

        _gearStatText.text += "]";
    }

    public void SetMemoryText(GearStatSo gearStat, bool upgrade, int level)
    {
        _gearStatText.text = "<color=grey>[";
        for (int i = 0; i < gearStat.GearMemories[0].Memory.Length; i++)
        {
            if (i == level)
            {
                float additiveMemory = gearStat.GetAdditiveMemory(i);
                if (additiveMemory != 0)
                {
                    if (additiveMemory > 0)
                    {
                        _gearStatText.text += "<color=green>";
                    }
                    else
                    {
                        _gearStatText.text += "<color=red>";
                    }
                }
                else
                {
                    _gearStatText.text += "<color=white>";
                }
            }
            _gearStatText.text += gearStat.GetTotalMemory(upgrade, i).ToString();
            _gearStatText.text += "<color=grey>";
            if (i != gearStat.GearMemories[0].Memory.Length - 1)
            {
                _gearStatText.text += "/";
            }
        }

        _gearStatText.text += "]";
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _gearStatText = FindGameObjectInChildren<TextMeshProUGUI>("StatText");
    }
#endif
}
