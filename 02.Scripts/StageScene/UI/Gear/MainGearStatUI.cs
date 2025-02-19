using System;
using TMPro;
using UnityEngine;

public class MainGearStatUI : BaseBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentPowerText;
    [SerializeField] private TextMeshProUGUI _currentEfficiencyText;
    [SerializeField] private TextMeshProUGUI _currentPlusRatioText;

    
    public void UpdateCurrentPowerText(int subGearCount, float power)
    {
        if (subGearCount > power)
        {
            _currentPowerText.text = "<color=red>" + subGearCount.ToString() + " / " + power.ToString();
        }
        else
        {
            _currentPowerText.text = subGearCount.ToString() + " / " + power.ToString();
        }
    }

    public void UpdateEfficiencyText(float efficiency)
    {
        _currentEfficiencyText.text = (efficiency * 100).ToString() + "%";
    }
    public void UpdatePlusRatioText(float amount)
    {
        amount = (float)Math.Round((double)amount * 100);
        //var value = (int)(amount - 100);
        _currentPlusRatioText.text = amount.ToString() + "%";
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _currentPowerText = FindGameObjectInChildren<TextMeshProUGUI>("CurrentPowerText");
        _currentEfficiencyText = FindGameObjectInChildren<TextMeshProUGUI>("CurrentEfficiencyText");
        _currentPlusRatioText = FindGameObjectInChildren<TextMeshProUGUI>("CurrentPlusRatioText");
    }
#endif
}
