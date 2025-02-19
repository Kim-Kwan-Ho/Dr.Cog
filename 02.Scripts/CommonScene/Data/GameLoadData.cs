using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameLoadData
{
    private List<int> _augmentIndexList;
    public List<int> AugmentIndexList { get { return _augmentIndexList; } }
    private List<int> _epicUpgradeList;
    public List<int> EpicUpgradeList { get { return _epicUpgradeList; } }

    private Dictionary<int, bool> _playerGearDic;
    public Dictionary<int, bool> PlayerGearDic { get { return _playerGearDic; } }


    private WorldSceneInfo _worldInfo;
    public WorldSceneInfo WorldInfo { get { return _worldInfo; } }
    public GameLoadData()
    {
        _augmentIndexList = new List<int>();
        _epicUpgradeList = new List<int>();
        _playerGearDic = new Dictionary<int, bool>();
        _worldInfo = new WorldSceneInfo(0,null);
    }


    public void AddAugment(AugmentSo augment)
    {
        _augmentIndexList.Add(augment.Index);
        SaveData();
    }

    public void AddEpicUpgrade(int index)
    {
        _epicUpgradeList.Add(index);
        SaveData();
    }

    public void ResetGameLoadData()
    {
        _augmentIndexList = new List<int>();
        _epicUpgradeList = new List<int>();
        _playerGearDic = new Dictionary<int, bool>();
        _worldInfo = new WorldSceneInfo(0,null);
        SaveData();
    }

    public void SaveStageData(int act, List<int> depth)
    {
        _worldInfo = new WorldSceneInfo(act, depth);
        SaveData();
    }
    public void SaveData()
    {
        SaveLoadFile.SaveGameData(this);
    }

    public void AddPlayerGear(int[] gearIds)
    {
        for (int i = 0; i < gearIds.Length; i++)
        {
            _playerGearDic[gearIds[i]] = false;
        }
        SaveLoadFile.SaveGameData(this);
    }
    public void RemovePlayerGear(int[] gearIds)
    {
        for (int i = 0; i < gearIds.Length; i++)
        {
            _playerGearDic.Remove(gearIds[i]);
        }
        SaveLoadFile.SaveGameData(this);
    }
    public void UpgradeGear(int gearId)
    {
        _playerGearDic[gearId] = true;
        SaveLoadFile.SaveGameData(this);
    }

    public bool CheckValidData()
    {
        if (_playerGearDic == null || _playerGearDic.Count <= 0)
        {
            return false;
        }
        return true;
    }
}


[Serializable]
public struct WorldSceneInfo
{
    public int Act;
    public List<int> Depth;
    public WorldSceneInfo(int act, List<int> depth)
    {
        Depth = new List<int>();
        if (depth != null)
        {
            foreach (var i in depth)
            {
                Depth.Add(i);
            }
        }
    
        Act = act;
    }
}
