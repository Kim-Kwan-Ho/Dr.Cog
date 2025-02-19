using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISynergyable
{
    public Dictionary<ESynergyType, SynergySo> SynergyDic { get; set; }
    public void InitializeSynergy();
    void AddSynergyable();
    void RemoveSynergyable();

    void ApplySynergy();
    void RemoveSynergy();

}
public class SynergyManager : BaseBehaviour
{
    public static SynergyManager Instance;
    private Dictionary<ESynergyType, Dictionary<int, HashSet<SubGear>>> _subGearDic = new Dictionary<ESynergyType, Dictionary<int, HashSet<SubGear>>>();
    private HashSet<ISynergyable> _synergies = new HashSet<ISynergyable>();
    [SerializeField] private SynergyUI _synergyUI;
    [SerializeField] private SynergyStackGroup _stackGroup;
    [SerializeField] private List<SynergySo> _synergyList;
    private Dictionary<ESynergyType, SynergySo> _synergyDic = new Dictionary<ESynergyType, SynergySo>();

    protected override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        foreach (var synergy in _synergyList)
        {
            _synergyDic[synergy.SynergyType] = synergy;
            synergy.InitializeSynergySo();
        }
    }

    private void UpdateSynergies(ESynergyType synergyType)
    {
        if (_synergyDic.ContainsKey(synergyType))
        {
            _synergyDic[synergyType].SetLevel(_subGearDic[synergyType].Count);
            CheckSynergyCheering(synergyType);
        }

        bool isMax = _synergyDic[synergyType].CheckIsMaxLevel(_synergyDic[synergyType].Level);
        if (isMax)
        {
            SynergyInfo synergyInfo = new SynergyInfo(_synergyDic[synergyType], synergyType, _synergyDic[synergyType].Level,
                _subGearDic[synergyType].Count, 0, true);

            _synergyUI.UpdateSynergies(synergyInfo, ESynergyUIMode.InGame);
        }
        else
        {
            SynergyInfo synergyInfo = new SynergyInfo(_synergyDic[synergyType], synergyType, _synergyDic[synergyType].Level,
                _subGearDic[synergyType].Count, _synergyDic[synergyType].SynergyLevel.RequireCount[_synergyDic[synergyType].Level], false);
            _synergyUI.UpdateSynergies(synergyInfo, ESynergyUIMode.InGame);
        }
        _stackGroup.UpdateSynergyStack(synergyType);
    }

    private void CheckSynergyCheering(ESynergyType synergyType)
    {
        if (_stageInfoSo.CurSynergyForCheeringDic.Count <= 0)
            return;
        if (_stageInfoSo.CurSynergyForCheeringDic.ContainsKey(synergyType))
        {
            if (_synergyDic[synergyType].Level >= _stageInfoSo.CurSynergyForCheeringDic[synergyType].Min)
            {
                StageSceneManager.Instance.EventStageScene.CallStartCheer(ECheerType.Synergy, -1);
                _stageInfoSo.CurSynergyForCheeringDic[synergyType].Remove(_stageInfoSo.CurSynergyForCheeringDic[synergyType].Min);
                if (_stageInfoSo.CurSynergyForCheeringDic[synergyType].Count <= 0)
                    _stageInfoSo.CurSynergyForCheeringDic.Remove(synergyType);
            }
        }

    }
    public void AddSynergy(List<SynergySo> synergyList, int gearId, SubGear subGear)
    {
        foreach (var synergy in synergyList)
        {
            if (!_subGearDic.ContainsKey(synergy.SynergyType))
            {
                _subGearDic[synergy.SynergyType] = new Dictionary<int, HashSet<SubGear>>();
                _subGearDic[synergy.SynergyType][gearId] = new HashSet<SubGear>();
            }
            else if (!_subGearDic[synergy.SynergyType].ContainsKey(gearId))
            {
                _subGearDic[synergy.SynergyType][gearId] = new HashSet<SubGear>();
            }

            _subGearDic[synergy.SynergyType][gearId].Add(subGear);
            UpdateSynergies(synergy.SynergyType);
        }
    }

    public void RemoveSynergy(List<SynergySo> synergyList, int gearId, SubGear subGear)
    {
        foreach (var synergy in synergyList)
        {
            _subGearDic[synergy.SynergyType][gearId].Remove(subGear);
            if (_subGearDic[synergy.SynergyType][gearId].Count == 0)
            {
                _subGearDic[synergy.SynergyType].Remove(gearId);
            }
            UpdateSynergies(synergy.SynergyType);
        }
    }


    public void AddSynergyable(ISynergyable synergy)
    {
        _synergies.Add(synergy);
    }
    public void RemoveSynergyable(ISynergyable synergy)
    {
        _synergies.Remove(synergy);
    }

    public void ActiveGearGlow(ESynergyType type)
    {
        if (!_subGearDic.ContainsKey(type))
            return;
        foreach (var subGears in _subGearDic[type])
        {
            foreach (var subGear in subGears.Value)
            {
                subGear.ActiveGlow();
            }
        }
    }

    public void DeActiveGearGlow(ESynergyType type)
    {
        if (!_subGearDic.ContainsKey(type))
            return;
        foreach (var subGears in _subGearDic[type])
        {
            foreach (var subGear in subGears.Value)
            {
                subGear.DeActiveGlow();
            }
        }
    }
    public void DeActiveGearGlow()
    {
        foreach (var subGears in _subGearDic.Values)
        {
            foreach (var subGearHash in subGears.Values)
            {
                foreach (var subGear in subGearHash)
                {
                    subGear.DeActiveGlow();
                }

            }
        }
    }
    public int GetSubGearSynergyCount(ESynergyType type)
    {
        int count = 0;
        foreach (var subGear in _subGearDic[type])
        {
            count += subGear.Value.Count;
        }
        return count;
    }
    #region Event

    private StageInfoSo _stageInfoSo;
    private void OnEnable()
    {
        StageSceneManager.Instance.EventStageScene.OnStageInitialize += Event_StageInitialize;
    }

    private void OnDisable()
    {
        StageSceneManager.Instance.EventStageScene.OnStageInitialize -= Event_StageInitialize;
    }

    private void Event_StageInitialize(StageInfoSo stageInfo)
    {
        _stageInfoSo = stageInfo;
    }
    #endregion


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _synergyList = FindObjectsInAsset<SynergySo>();
        _synergyUI = FindAnyObjectByType<SynergyUI>();
        _stackGroup = FindAnyObjectByType<SynergyStackGroup>();
    }
#endif
}

#region  Legacy11.19
/*
using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;
   
   
   public interface ISynergyable
   {
       Dictionary<ESynergyType, SynergySo> SynergyDic { get; set; }
       void AddSynergyable();
       void RemoveSynergyable();
   }
   public class SynergyManager : BaseBehaviour
   {
       [SerializeField] private List<SynergySo> _synergyList;
       public static SynergyManager Instance;
       private Dictionary<ESynergyType, Dictionary<int, int>> _synergyDic = new Dictionary<ESynergyType, Dictionary<int, int>>();
       private HashSet<ISynergyable> _synergies = new HashSet<ISynergyable>();
       [SerializeField] private SynergyLevelsSo _synergyLevelsSo;
       [SerializeField] private SynergyUI _synergyUI;
   
   
       protected override void Initialize()
       {
           base.Initialize();
           if (Instance != null)
           {
               Destroy(this);
           }
           else
           {
               Instance = this;
           }
   
           foreach (var synergy in _synergyList)
           {
               synergy.InitializeSynergySo();
           }
       }
   
       private void UpdateSynergies(ESynergyType synergyTypeType)
       {
           foreach (var syn in _synergies)
           {
               int synergyLevel = CheckSynergyLevel(synergyTypeType, _synergyDic[synergyTypeType].Count);
               syn.UpdateSynergyLevel(synergyTypeType, synergyLevel);
               bool isMax = _synergyLevelsSo.SynergyLevelDic[synergyTypeType].Length == synergyLevel;
               if (isMax)
               {
                   _synergyUI.UpdateSynergies(synergyTypeType, synergyLevel, _synergyDic[synergyTypeType].Count, 0, true);
               }
               else
               {
                   _synergyUI.UpdateSynergies(synergyTypeType, synergyLevel, _synergyDic[synergyTypeType].Count, _synergyLevelsSo.SynergyLevelDic[synergyTypeType][synergyLevel]);
               }
   
           }
       }
       public void AddSynergy(List<ESynergyType> synergyList, int gearId)
       {
           foreach (var synergy in synergyList)
           {
               if (!_synergyDic.ContainsKey(synergy))
               {
                   _synergyDic[synergy] = new Dictionary<int, int>();
                   _synergyDic[synergy][gearId] = 0;
               }
               else if (!_synergyDic[synergy].ContainsKey(gearId))
               {
                   _synergyDic[synergy][gearId] = 0;
               }
   
               _synergyDic[synergy][gearId]++;
               UpdateSynergies(synergy);
           }
       }
   
       public void RemoveSynergy(List<ESynergyType> synergyList, int gearId)
       {
           foreach (var synergy in synergyList)
           {
               _synergyDic[synergy][gearId]--;
               if (_synergyDic[synergy][gearId] == 0)
               {
                   _synergyDic[synergy].Remove(gearId);
               }
               UpdateSynergies(synergy);
           }
       }
   
   
       public void AddSynergyable(ISynergyable synergy)
       {
           _synergies.Add(synergy);
       }
       public void RemoveSynergyable(ISynergyable synergy)
       {
           _synergies.Remove(synergy);
       }
   
       private int CheckSynergyLevel(ESynergyType type, int count)
       {
           for (int i = 0; i < _synergyLevelsSo.SynergyLevelDic[type].Length; i++)
           {
               if (count < _synergyLevelsSo.SynergyLevelDic[type][i])
               {
                   return i;
               }
           }
           return _synergyLevelsSo.SynergyLevelDic[type].Length;
       }
   
   #if UNITY_EDITOR
       protected override void OnBindField()
       {
           base.OnBindField();
           _synergyUI = FindAnyObjectByType<SynergyUI>();
       }
   #endif
   }
   
*/
#endregion
