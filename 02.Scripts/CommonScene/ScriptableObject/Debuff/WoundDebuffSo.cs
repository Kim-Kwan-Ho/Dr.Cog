using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "WoundDebuff", menuName = "Scriptable Objects/Debuffs/WoundDebuff")]
public class WoundDebuffSo : DebuffSo
{

    [Header("Debuff Time")]
    private float _currentTime;
    [SerializeField] private float _woundSpwanTime;


    [Header("Components")]
    private TilemapHandler _tilemapHandler;
    private MainGear _mainGear;
    private SupplySystem _supplySystem;

    [Header("Wounds")]
    [SerializeField] private GameObject _woundGob;
    private int _currentWoundCount;
    [SerializeField] private int _maxWoundCount;
    [SerializeField] private float _woundLifeTime;

    [Header("DebuffStats")]
    [SerializeField] private float _memoryDecreaseRatio;
    [SerializeField] private float _supplyTimeIncreaseAmount;
    [SerializeField] private float _statIncreaseTime;
    public void InitializeWoundDebuffSo(TilemapHandler tilemap, MainGear mainGear, SupplySystem supplySystem, Transform uiSpawnTrs)
    {
        _currentTime = 0;
        _currentLevel = 0;
        _currentWoundCount = 0;
        _currentTime = _woundSpwanTime;
        _isEnabled = true;
        _levelActions = new Action<bool>[_requireLevels.Length + 1];
        _levelActions[0] = null;
        _levelActions[1] = SetMemoryIncreaseDebuff;
        _levelActions[2] = SetSupplyTimeDeubff;
        _levelActions[3] = SetStatIncreaseTimeDebuff;
        _tilemapHandler = tilemap;
        _mainGear = mainGear;
        _supplySystem = supplySystem;
        _hasStack = true;
    }

    public override void Update()
    {
        base.Update();
        if (_currentWoundCount < _maxWoundCount)
        {
            UpdateSpawnTime();
        }
    }

    private void UpdateSpawnTime()
    {
        _currentTime -= (Time.deltaTime * StageSceneManager.Instance.TimeRatio);
        if (_currentTime <= 0)
        {
            SpawnWound();
            _currentTime = _woundSpwanTime;

        }
    }

    private void SpawnWound()
    {
        Wound wound = Instantiate(_woundGob).GetComponent<Wound>();
        wound.SetWound(_woundLifeTime);
        wound.OnDestroy += RemoveWound;
        _tilemapHandler.BatchWound(wound);
        _currentWoundCount++;
        CheckLevel(_currentWoundCount);

    }
    private void RemoveWound(Vector2Int pos)
    {
        _currentWoundCount--;
        CheckLevel(_currentWoundCount);
    }

    private void SetMemoryIncreaseDebuff(bool active)
    {
        if (active)
        {
            _mainGear.OnGetAdditiveStatRatio -= GetMemoryDecreaseRatio;
            _mainGear.OnGetAdditiveStatRatio += GetMemoryDecreaseRatio;
        }
        else
        {
            _mainGear.OnGetAdditiveStatRatio -= GetMemoryDecreaseRatio;
        }
    }
    private float GetMemoryDecreaseRatio()
    {
        return  -(_memoryDecreaseRatio/100f);
    }


    private void SetSupplyTimeDeubff(bool active)
    {
        if (active)
        {
            _supplySystem.OnGetSupplyIndecreaseTime -= GetSupplyDcreaseTime;
            _supplySystem.OnGetSupplyIndecreaseTime += GetSupplyDcreaseTime;
        }
        else
        {
            _supplySystem.OnGetSupplyIndecreaseTime -= GetSupplyDcreaseTime;
        }
    }
    private float GetSupplyDcreaseTime()
    {
        return _supplyTimeIncreaseAmount;
    }
    private void SetStatIncreaseTimeDebuff(bool active)
    {
        if (active)
        {
            _mainGear.OnGetStatIndecreaseAmount -= GetStatIncreaseTime;
            _mainGear.OnGetStatIndecreaseAmount += GetStatIncreaseTime;
        }
        else
        {
            _mainGear.OnGetStatIndecreaseAmount -= GetStatIncreaseTime;
        }
        _mainGear.OnSpeedChanged?.Invoke();
    }
    private float GetStatIncreaseTime()
    {
        return _statIncreaseTime;
    }

    public override int GetStackCount()
    {
        return _currentWoundCount;
    }
}
