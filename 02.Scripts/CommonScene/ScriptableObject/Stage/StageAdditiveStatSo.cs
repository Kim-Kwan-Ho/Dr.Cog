using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StageAdditiveStat", menuName = "Scriptable Objects/Stages/StageAdditiveStat")]
public class StageAdditiveStatSo : ScriptableObject
{
    [SerializeField] private List<StageAdditiveStat> _additiveStatList;


    public void InitializeStageSo()
    {
        _additiveStatList = new List<StageAdditiveStat>();
    }

    public void AddStageAdditiveStat(StageAdditiveStat stat)
    {
        _additiveStatList.Add(stat);
    }

    public float GetTotalStageTime(float time)
    {
        float amount = time;
        foreach (var additiveStat in _additiveStatList)
        {
            amount += additiveStat.StageTime;
        }
        return amount;
    }

    public float GetTotalTargetMemory(float memory)
    {
        float amount = memory;
        foreach (var additiveStat in _additiveStatList)
        {
            amount += additiveStat.StageMemory * memory;
        }
        return amount;
    }

}
[Serializable]
public struct StageAdditiveStat
{
    [SerializeField] private float _stageTime;
    public float StageTime { get { return _stageTime; } }
    [SerializeField] private float _stageMemory;
    public float StageMemory { get { return _stageMemory; } }
}
