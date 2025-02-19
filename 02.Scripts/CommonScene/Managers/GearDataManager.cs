using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(GearDataEvents))]
public class GearDataManager : BaseBehaviour
{
    public static GearDataManager Instance;
    public GearDataEvents EventGearData;

    private Dictionary<int, GearSo> _gearDic = new Dictionary<int, GearSo>();
    private Dictionary<int, PlayerGear> _playerGearDic = new Dictionary<int, PlayerGear>();

    [SerializeField] private List<SynergySo> _synergyList;
    private Dictionary<ESynergyType, SynergySo> _synergyDic;
    protected override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        LoadAllGears();
        _synergyDic = _synergyList.ToDictionary(d => d.SynergyType);
    }


    private void LoadAllGears()
    {
        foreach (var gearSo in Resources.LoadAll<GearSo>("ScriptableObjects/GearSo/VSBuild"))
        {
            gearSo.GearStat.InitializeGearSo();
            _gearDic[gearSo.GearID] = gearSo;
        }
    }
    public GearSo GetGearData(int gearId)
    {
        return _gearDic[gearId];
    }

    public GearStatSo[] GetAllGears()
    {
        return _gearDic.Select(x => x.Value.GearStat).ToArray();
    }
    public List<GearStatSo> GetSynergyGears(SynergySo synergy)
    {
        List<GearStatSo> gearStatList = new List<GearStatSo>();
        foreach (var gearSo in _gearDic)
        {
            if (gearSo.Value.GearStat.SynergyList.Contains(synergy))
            {
                gearStatList.Add(gearSo.Value.GearStat);
            }
        }
        return gearStatList;
    }

    public PlayerGear GetPlayerSynergyGear(SynergySo synergy)
    {
        List<PlayerGear> playerGearList = new List<PlayerGear>();
        foreach (var playerGear in _playerGearDic)
        {
            if (playerGear.Value.GearSo.GearStat.SynergyList.Contains(synergy))
            {
                playerGearList.Add(playerGear.Value);
            }
        }

        int r = Random.Range(0, playerGearList.Count);
        return playerGearList[r];
    }
    public PlayerGear[] GetPlayerGearData()
    {
        return _playerGearDic.Select(s => s.Value).ToArray();
    }
    public PlayerGear[] GetPlayerUpgradeGears()
    {
        return _playerGearDic.Select(s => s.Value).Where(w => w.Upgrade == false).ToArray();
    }

    public PlayerGear GetPlayerRandomGear()
    {
        return _playerGearDic.Select(s => s.Value).ElementAt(Random.Range(0, _playerGearDic.Count));
    }

    public GearSo[] GetNonRepetitionGears(int count)
    {
        count = Mathf.Min(_gearDic.Count - _playerGearDic.Count, count);
        HashSet<GearSo> gearHash = new HashSet<GearSo>(count);
        while (gearHash.Count < count)
        {
            int r = Random.Range(0, _gearDic.Count);
            if (!_playerGearDic.ContainsKey(r))
            {
                gearHash.Add(_gearDic[r]);
            }
        }
        return gearHash.ToArray();
    }

    public List<ESynergyType> GetCombinableSynergy()
    {
        Dictionary<ESynergyType, int> synergyDic = new Dictionary<ESynergyType, int>();
        foreach (var playerGear in _playerGearDic)
        {
            foreach (var synergy in playerGear.Value.GearSo.GearStat.SynergyList)
            {
                if (!synergyDic.ContainsKey(synergy.SynergyType))
                {
                    synergyDic[synergy.SynergyType] = 0;
                }

                synergyDic[synergy.SynergyType]++;
            }
        }

        List<ESynergyType> synergyList = new List<ESynergyType>();
        foreach (var synergy in synergyDic)
        {
            if (_synergyDic[synergy.Key].CheckSynergyLevel(synergy.Value) > 0)
            {
                synergyList.Add(synergy.Key);
            }
        }
        return synergyList;
    }
    public List<SynergyInfo> GetCurrentGearDeckSynergyInfos()
    {
        Dictionary<ESynergyType, int> synergyDic = new Dictionary<ESynergyType, int>();
        foreach (var playerGear in _playerGearDic)
        {
            foreach (var synergy in playerGear.Value.GearSo.GearStat.SynergyList)
            {
                if (!synergyDic.ContainsKey(synergy.SynergyType))
                {
                    synergyDic[synergy.SynergyType] = 0;
                }
                synergyDic[synergy.SynergyType]++;
            }
        }
        List<SynergyInfo> synergyInfoList = new List<SynergyInfo>();
        foreach (var synergy in synergyDic)
        {
            int level = _synergyDic[synergy.Key].CheckSynergyLevel(synergy.Value);
            if (_synergyDic[synergy.Key].CheckIsMaxLevel(level))
            {
                SynergyInfo synergyInfo = new SynergyInfo(_synergyDic[synergy.Key], synergy.Key, level, synergy.Value, 0, true);
                synergyInfoList.Add(synergyInfo);
            }
            else
            {
                SynergyInfo synergyInfo = new SynergyInfo(_synergyDic[synergy.Key], synergy.Key, level, synergy.Value, _synergyDic[synergy.Key].SynergyLevel.RequireCount[level], false);
                synergyInfoList.Add(synergyInfo);
            }
        }
        return synergyInfoList;
    }
    public List<SynergyInfo> GetPlayerNotContainsSynergyInfos()
    {
        Dictionary<ESynergyType, int> synergyDic = new Dictionary<ESynergyType, int>();
        foreach (var gear in _gearDic)
        {
            if (_playerGearDic.ContainsKey(gear.Key))
                continue;

            foreach (var synergy in gear.Value.GearStat.SynergyList)
            {
                if (!synergyDic.ContainsKey(synergy.SynergyType))
                {
                    synergyDic[synergy.SynergyType] = 0;
                }

            }
        }
        List<SynergyInfo> synergyInfoList = new List<SynergyInfo>();
        foreach (var synergy in synergyDic)
        {
            int level = 0;
            SynergyInfo synergyInfo = new SynergyInfo(_synergyDic[synergy.Key], synergy.Key, level, synergy.Value, _synergyDic[synergy.Key].SynergyLevel.RequireCount[level], false);
            synergyInfoList.Add(synergyInfo);
        }
        return synergyInfoList;
    }
    #region Event

    private void OnEnable()
    {
        EventGearData.OnGearAdded += Event_GearAdded;
        EventGearData.OnGearRemoved += Event_GearRemoved;
        EventGearData.OnPlayerDeckReset += Event_PlayerResetDeck;
        EventGearData.OnGearUpgraded += Event_GearUpgraded;
        EventGearData.OnGearLoaded += Event_GearLoaded;
        GameManager.Instance.EventGameMain.OnDataCleared += Event_OnDataCleared;
    }

    private void OnDisable()
    {
        EventGearData.OnGearAdded -= Event_GearAdded;
        EventGearData.OnGearRemoved -= Event_GearRemoved;
        EventGearData.OnPlayerDeckReset -= Event_PlayerResetDeck;
        EventGearData.OnGearUpgraded -= Event_GearUpgraded;
        EventGearData.OnGearLoaded -= Event_GearLoaded;
        GameManager.Instance.EventGameMain.OnDataCleared -= Event_OnDataCleared;
    }


    private void Event_GearAdded(GearAddedEventArgs eventArgs)
    {
        AddPlayerGear(eventArgs.GearIds);
        GameManager.GameLoadData.AddPlayerGear(eventArgs.GearIds);
    }



    private void AddPlayerGear(int[] gearIds)
    {
        for (int i = 0; i < gearIds.Length; i++)
        {
            PlayerGear playerGear = new PlayerGear(_gearDic[gearIds[i]]);
            _playerGearDic[gearIds[i]] = playerGear;
        }
    }
    private void Event_GearRemoved(GearRemovedEventArgs eventArgs)
    {
        RemovePlayerGear(eventArgs.GearIds);
        GameManager.GameLoadData.RemovePlayerGear(eventArgs.GearIds);
    }

    private void RemovePlayerGear(int[] gearIds)
    {
        for (int i = 0; i < gearIds.Length; i++)
        {
            _playerGearDic.Remove(gearIds[i]);
        }
    }
    private void Event_PlayerResetDeck()
    {
        ResetPlayerDeck();
    }

    private void Event_GearUpgraded(GearUpgradedEventArgs eventArgs)
    {
        UpgradeGear(eventArgs.GearId);
        GameManager.GameLoadData.UpgradeGear(eventArgs.GearId);
    }

    private void Event_OnDataCleared()
    {
        ResetPlayerDeck();
        ResetGearStats();
    }
    private void UpgradeGear(int gearId)
    {
        _playerGearDic[gearId].Upgrade = true;
    }
    private void ResetPlayerDeck()
    {
        _playerGearDic = new Dictionary<int, PlayerGear>(0);
    }

    private void ResetGearStats()
    {
        foreach (var gearSo in _gearDic)
        {
            gearSo.Value.GearStat.InitializeGearSo();
        }
    }

    private void Event_GearLoaded(GearLoadedEventArgs eventArgs)
    {
        for (int i = 0; i < eventArgs.PlayerGears.Length; i++)
        {
            PlayerGear playerGear = new PlayerGear(_gearDic[eventArgs.PlayerGears[i].Key]);
            playerGear.Upgrade = eventArgs.PlayerGears[i].Value;
            _playerGearDic[eventArgs.PlayerGears[i].Key] = playerGear;
        }
    }

    #endregion

#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        EventGearData = GetComponent<GearDataEvents>();
        _synergyList = FindObjectsInAsset<SynergySo>();

    }

#endif


}
