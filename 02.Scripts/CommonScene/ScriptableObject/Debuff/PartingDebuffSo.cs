using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PartingDebuff", menuName = "Scriptable Objects/Debuffs/PartingDebuff")]
public class PartingDebuffSo : DebuffSo
{
    private GearController _gearController;
    private TilemapHandler _tilemapHandler;
    private MainGear _mainGear;


    [Header("Time")]
    private bool _isTimeDecreasing;
    private float _currentTime;
    [SerializeField] private float _targetTime;


    [Header("Speed")]
    [SerializeField] private float _normalDecreaseSpeed;
    [SerializeField] private float _fastDecreaseSpeed;
    private float _decreaseSpeed;


    [Header("ApplyCount")]
    [SerializeField] private int _firstApplyCount;
    [SerializeField] private int _secondApplyCount;
    private int _applyCount;


    public void InitializePartingDebuffSo(GearController garController, MainGear mainGear, TilemapHandler tilemapHandler, Transform uiSpawnTrs)
    {
        _gearController = garController;
        _tilemapHandler = tilemapHandler;
        _mainGear = mainGear;
        _mainGear.OnEfficiencyChanged -= OnEfficiencyChanged;
        _mainGear.OnEfficiencyChanged += OnEfficiencyChanged;
        _isEnabled = true;
        _isTimeDecreasing = false;
        _currentLevel = 0;
        _applyCount = _firstApplyCount;
        _currentTime = _targetTime;
        _decreaseSpeed = _normalDecreaseSpeed;
        _levelActions = new Action<bool>[_requireLevels.Length + 1];

        _levelActions[0] = null;
        _levelActions[1] = SetFirstApplyCount;
        _levelActions[2] = SetDecreaseSpeed;
        _levelActions[3] = SetSecondApplyCount;

        _hasStack = true;
    }

    public override void Update()
    {
        base.Update();
        if (!_isTimeDecreasing)
            return;
        DecreaseTime();
    }

    private void OnEfficiencyChanged()
    {
        CheckLevel(_mainGear.GetOverGearCount());
    }
    private void DecreaseTime()
    {
        _currentTime -= _decreaseSpeed * (Time.deltaTime * StageSceneManager.Instance.TimeRatio);
        if (_currentTime < 0)
        {
            ActiveDebuff();
            _currentTime = _targetTime;
        }
    }

    private void ActiveDebuff()
    {
        for (int i = 0; i < _applyCount; i++)
        {
            _gearController.DecreaseRandomGearLevel();
        }
    }

    private void SetFirstApplyCount(bool active)
    {
        _isTimeDecreasing = active;
    }

    private void SetDecreaseSpeed(bool active)
    {
        if (active)
        {
            _decreaseSpeed = _fastDecreaseSpeed;
        }
        else
        {
            _decreaseSpeed = _normalDecreaseSpeed;
        }
    }
    private void SetSecondApplyCount(bool active)
    {
        if (active)
        {
            _applyCount = _secondApplyCount;
        }
        else
        {
            _applyCount = _firstApplyCount;
        }
    }

    public override int GetStackCount()
    {
        if (_mainGear.GetOverGearCount() <= 0)
            return 0;
        return _mainGear.GetOverGearCount();
    }
}
