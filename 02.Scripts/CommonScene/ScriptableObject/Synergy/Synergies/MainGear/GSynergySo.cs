using System;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "GSynergy", menuName = "Scriptable Objects/Synergy/MainGear/GSynergy", order = 7)]
public class GSynergySo : MainGearSynergySo
{
    [SerializeField] private float _decreaseAmount;
    private float _timer = 1f;
    [SerializeField] private float[] _startAmount;
    private float _currentAmount;
    private float _beforeAmount;

    [SerializeField] private GameObject _textGob;
    private TextMeshPro _infotext;

    public override void InitializeSynergySo()
    {
        base.InitializeSynergySo();
        _currentAmount = 0;
        _beforeAmount = 0;
        _timer = 1;
        _infotext = null;
    }
    public override void ApplySynergy()
    {
        _mainGear.OnGetAdditiveMemory += GetSynergyAdditiveMemory;
        _mainGear.OnMemoryIncreased += ResetTimer;
        if (_infotext == null)
        {
            _infotext = Instantiate(_textGob, _mainGear.transform.position, Quaternion.identity).GetComponent<TextMeshPro>();
        }
    }

    public override void RemoveSynergy()
    {
        _mainGear.OnGetAdditiveMemory -= GetSynergyAdditiveMemory;
        _mainGear.OnMemoryIncreased -= ResetTimer;

        if (_infotext != null)
        {
            Destroy(_infotext.gameObject);
        }
    }

    public override void UpdateSynergy()
    {
        if (!_isActive || _level == 0)
            return;

        DecreaseTimer();
        if (_infotext != null)
        {
            _infotext.text = _currentAmount.ToString("F1");
        }
    }

    private void DecreaseTimer()
    {
        _timer -= (Time.deltaTime * StageSceneManager.Instance.TimeRatio);
        if (_timer <= 0)
        {
            DecreaseAmount();
            _timer = 1;
        }
    }

    private void ResetTimer()
    {
        _timer = 1;
    }
    private void DecreaseAmount()
    {
        _currentAmount -= _decreaseAmount;
        if (_currentAmount < 0)
        {
            _currentAmount = 0;
        }
    }

    private float GetSynergyAdditiveMemory(float amount)
    {
        _beforeAmount = _currentAmount;
        _currentAmount = _startAmount[_level];
        if (_beforeAmount <= 0)
        {
            return 0;
        }
        else
        {
            return amount * (_beforeAmount / 100f);
        }
    }
}