using System;
using System.Collections.Generic;
using UnityEngine;


public enum EStageRewardType
{
    None = 0,
    GearAdd = 1,
    GearUpgrade = 2,
    GearRemove = 3,
    Augment = 4,
    MainGearUpgrade = 5,
    Dialog = 6,
    // Todo Remove this
    End
}

public enum EStageType
{
    Normal,
    Epic,
    Boss,
    World
}

[CreateAssetMenu(fileName = "StageInfo_", menuName = "Scriptable Objects/Stages/StageInfo")]
public class
    StageInfoSo : ScriptableObject
{
    [SerializeField] private EStageType _stageType;
    public EStageType StageType { get { return _stageType; } }

    [SerializeField] private EStageRewardType _rewardType;
    public EStageRewardType RewardType { get { return _rewardType; } }

    [SerializeField] private float _stageTime;
    public float StageTime { get { return _stageTime; } }
    [SerializeField] private float _goalMemory;
    public float GoalMemory { get { return _goalMemory; } }

    [SerializeField] private List<SynergyForCheering> _startSynergyForCheeringList;
    private Dictionary<ESynergyType, SortedSet<int>> _curSynergyForCheeringDic;
    public Dictionary<ESynergyType, SortedSet<int>> CurSynergyForCheeringDic { get { return _curSynergyForCheeringDic; } }

    [SerializeField] private Bgm _stageBgm;
    public Bgm StageBgm { get { return _stageBgm; } }

    [SerializeField] private MapPositionInfo _mapPositionInfo;
    public MapPositionInfo MapPositionInfo { get { return _mapPositionInfo; } }

    [SerializeField] private string _stageName;
    public string StageName { get { return _stageName; } }
    [SerializeField] private string _discription;
    public string Discription { get { return _discription; } }
    public void SetStageName(string name)
    {
        _stageName = name;
    }

    public void SetDiscription(string description)
    {
        _discription = description;
    }

    public void InitializeRoomInfoSo()
    {
        _curSynergyForCheeringDic = new Dictionary<ESynergyType, SortedSet<int>>();
        foreach (var synergyForCheering in _startSynergyForCheeringList)
        {
            if (!_curSynergyForCheeringDic.ContainsKey(synergyForCheering.SynergyType))
            {
                _curSynergyForCheeringDic[synergyForCheering.SynergyType] = new SortedSet<int>();
            }
            _curSynergyForCheeringDic[synergyForCheering.SynergyType].Add(synergyForCheering.SynergyLevel);

        }
    }
}

[Serializable]
public struct SynergyForCheering
{
    [SerializeField] private ESynergyType _synergyType;
    public ESynergyType SynergyType { get { return _synergyType; } }
    [SerializeField] private int _synergyLevel;
    public int SynergyLevel { get { return _synergyLevel; } }
}


[Serializable]
public struct MapPositionInfo
{
    [SerializeField] private int _stage;
    public int Stage { get { return _stage; } }
    [SerializeField] private int _depth;
    public int Depth { get { return _depth; } }
    [SerializeField] private int _height;

    public int Height { get { return _height; } }

    public MapPositionInfo(int stage, int depth, int height)
    {
        _stage = stage;
        _depth = depth;
        _height = height;
    }
}