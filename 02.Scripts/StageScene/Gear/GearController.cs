using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GearController : BaseBehaviour
{
    [Header("Gears")]
    private Dictionary<Vector2Int, SubGear> _gearDic = new Dictionary<Vector2Int, SubGear>(); // All Gears 
    [SerializeField] private MainGear _mainGear;

    [Header("Controllers")]
    [SerializeField] private TilemapHandler _tilemapHandler; // TO 
    [SerializeField] private StageMemory _stageMemory;



    [Header("Rotation")]
    [SerializeField] private GearRotationSo _gearRotationSo;
    private bool _isMainGearClockwise;

    [Header("UI")]
    [SerializeField] private GearControllerUI _gearControllerUI;
    private ESubGearStatState _subGearStatState;
    protected override void Initialize()
    {
        base.Initialize();
        _isMainGearClockwise = true;
        _timePaused = true;
        _subGearStatState = ESubGearStatState.None;
        _mainGear.SetClockwise(_isMainGearClockwise);
        SetUI();
    }

    private void Update()
    {
        if (!_timePaused)
            RotateGear();
    }

    private void SetUI()
    {
        _gearControllerUI.SetControllerUI(SetSubGearStatUI);
    }

    private void SetSubGearStatUI(ESubGearStatState state)
    {
        _subGearStatState = state;
        foreach (var subGear in _gearDic)
        {
            subGear.Value.ChangeGearUI(state);
        }
    }

    #region Connection
    public void AddGear(Vector2Int pos, GameObject gob, bool isClockwise, GearContainer gearContainer)
    {
        SubGear gear = gob.GetComponent<SubGear>();
        gear.transform.SetParent(transform);
        gear.SetGear(gearContainer, _stageMemory, _subGearStatState);
        _gearDic[pos] = gear;
        gear.SetClockwise(isClockwise != _isMainGearClockwise);
        RenewConnectedGear();
    }


    private void RenewConnectedGear()
    {
        var connectedGearList = _tilemapHandler.GetConnectedGearPositions();
        Dictionary<int, HashSet<SubGear>> connectedGears = new Dictionary<int, HashSet<SubGear>>();
        foreach (var gear in connectedGearList)
        {
            int depth = _tilemapHandler.GetCellDepth(gear, false);
            if (!connectedGears.ContainsKey(depth))
            {
                connectedGears[depth] = new HashSet<SubGear>();
            }
            connectedGears[depth].Add(_gearDic[gear]);
        }
        _mainGear.SetConnectedGear(connectedGears);

    }

    public void RemoveGear(ref GearContainer gearContainer, Vector2Int pos)
    {
        gearContainer = _gearDic[pos].GearContainer;
        _mainGear.RemoveGear(_tilemapHandler.GetCellDepth(pos, false), _gearDic[pos]);
        _gearDic.Remove(pos);
        RenewConnectedGear();
    }
    #endregion
    #region Rotate
   
    private void RotateGear()
    {
        Vector3 mainGearSpeed = new Vector3(0, 0, _gearRotationSo.MainGearSpeed * (Time.deltaTime * StageSceneManager.Instance.TimeRatio) * _mainGear.GetEfficiency());
        Vector3 subGearSpeed = mainGearSpeed * _gearRotationSo.SubGearRatio;
        if (_isMainGearClockwise)
        {
            _gearRotationSo.CurrentMainGearRotation -= mainGearSpeed;
            if (_gearRotationSo.CurrentMainGearRotation.z < 0)
            {
                _gearRotationSo.CurrentMainGearRotation = new Vector3(0, 0, 360);
            }
        }
        else
        {
            _gearRotationSo.CurrentMainGearRotation += mainGearSpeed;
            if (_gearRotationSo.CurrentMainGearRotation.z > 360)
            {
                _gearRotationSo.CurrentMainGearRotation = Vector3.zero;
            }
        }

        _gearRotationSo.CurrentSubGearClockwiseRotation -= subGearSpeed;
        if (_gearRotationSo.CurrentSubGearClockwiseRotation.z < 0)
        {
            _gearRotationSo.CurrentSubGearClockwiseRotation = new Vector3(0, 0, 360);
        }
        _gearRotationSo.CurrentSubGearCounterClockwiseRotation += subGearSpeed;
        if (_gearRotationSo.CurrentSubGearCounterClockwiseRotation.z > 360)
        {
            _gearRotationSo.CurrentSubGearCounterClockwiseRotation = Vector3.zero;
        }
    }
    #endregion


    #region Event

    private bool _timePaused;
    private void OnEnable()
    {
        StageSceneManager.Instance.EventStageScene.OnTimePaused += Event_TimePaused;
        StageSceneManager.Instance.EventStageScene.OnTimeResumed += Event_TimeResumed;
    }
    private void OnDisable()
    {
        StageSceneManager.Instance.EventStageScene.OnTimePaused -= Event_TimePaused;
        StageSceneManager.Instance.EventStageScene.OnTimeResumed -= Event_TimeResumed;
    }

    private void Event_TimePaused()
    {
        _timePaused = true;
    }
    private void Event_TimeResumed()
    {
        _timePaused = false;
    }
    #endregion

    #region Merge
    public bool CheckCanMergeGear(Vector2Int pos, GearContainer gearContainer)
    {
        if (!_gearDic.ContainsKey(pos))
            return false;
        if (gearContainer.Level + 1 >= gearContainer.GearSo.GearStat.MaxLevel)
            return false;

        return gearContainer.Compare(_gearDic[pos].GearContainer);
    }

    public void MergeGear(Vector2Int pos)
    {
        _gearDic[pos].MergeGear();
    }
    #endregion

    #region Debuff

    public void DecreaseRandomGearLevel()
    {
        var pos = GetRandomGearPosition();
        if (_gearDic[pos].CheckCanDecreaseLevel())
        {
            _gearDic[pos].DecreaseGearLevel();
        }
        else
        {
            _mainGear.RemoveGear(_tilemapHandler.GetCellDepth(pos, false), _gearDic[pos]);
            _tilemapHandler.RemoveGear(pos);
            _gearDic.Remove(pos);
            RenewConnectedGear();
        }
    }

    private Vector2Int GetRandomGearPosition()
    {
        return _gearDic.Select(s => s.Key).ElementAt(Random.Range(0, _gearDic.Count));

    }

    #endregion
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _gearRotationSo = FindObjectInAsset<GearRotationSo>("GearRotation", EDataType.asset);
        _mainGear = FindAnyObjectByType<MainGear>();
        _tilemapHandler = FindAnyObjectByType<TilemapHandler>();
        _stageMemory = FindAnyObjectByType<StageMemory>();
        _gearControllerUI = FindAnyObjectByType<GearControllerUI>();
    }
#endif
}


