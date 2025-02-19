using UnityEngine;


[CreateAssetMenu(fileName = "ESynergy", menuName = "Scriptable Objects/Synergy/MainGear/ESynergy", order = 5)]
public class ESynergySo : MainGearSynergySo
{
    [SerializeField] private float[] _speedRatios;
  
    public override void ApplySynergy()
    {
        _mainGear.OnGetSpeedDecreaseRatio += GetSpeedDecreaseRatio;
        _mainGear.OnEfficiencyChanged?.Invoke();
    }

    public override void RemoveSynergy()
    {
        _mainGear.OnGetSpeedDecreaseRatio -= GetSpeedDecreaseRatio;
        _mainGear.OnEfficiencyChanged?.Invoke();
    }

    private float GetSpeedDecreaseRatio()
    {
        return _speedRatios[_level];
    }

    public override void SetLevel(int level)
    {
        base.SetLevel(level);
        _mainGear.OnEfficiencyChanged?.Invoke();
    }
}