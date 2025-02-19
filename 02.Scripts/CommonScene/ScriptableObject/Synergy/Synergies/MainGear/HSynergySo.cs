using UnityEngine;

[CreateAssetMenu(fileName = "HSynergy", menuName = "Scriptable Objects/Synergy/MainGear/HSynergy", order = 8)]
public class HSynergySo : MainGearSynergySo
{
    [SerializeField] private int[] _stackIncreaseRatioAmount;
    [SerializeField] private int _requireAmount;
    private int _currentAmount;
    private int _currentStack;
    public override void InitializeSynergySo()
    {
        base.InitializeSynergySo();
        _mainGear = null;
        _currentAmount = 0;
        _currentStack = 0;
    }

    public override void ApplySynergy()
    {
        _mainGear.OnMemoryIncreased += OnMemoryIncreased;
        _mainGear.OnGetAdditiveStatRatio += GetSynergyAdditiveStatRatio;

    }

    public override void RemoveSynergy()
    {
        _mainGear.OnMemoryIncreased -= OnMemoryIncreased;
        _mainGear.OnGetAdditiveStatRatio -= GetSynergyAdditiveStatRatio;

    }

    private void OnMemoryIncreased()
    {
        if (!_isActive || _level == 0)
            return;

        _currentAmount++;
        if (CheckCanIncreasePower())
        {
            IncreaseStack();
        }
    }

    private bool CheckCanIncreasePower()
    {
        return _currentAmount >= _requireAmount;
    }

    private void IncreaseStack()
    {
        _currentStack++;
        _currentAmount = 0;
    }

    private float GetSynergyAdditiveStatRatio()
    {
        return (_currentStack * _stackIncreaseRatioAmount[_level] / 100f);
    }

    public override int GetStack()
    {
        return _requireAmount - _currentAmount;
    }

    public override string GetSynergyEffect()
    {
        return "X" + _currentStack.ToString();

    }
}