using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SubGear : Gear, ISynergyable
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _gearSprite;
    [SerializeField] private SpriteRenderer _gearMergeSprite;
    [Header("Stat")]
    private GearContainer _gearContainer;
    public GearContainer GearContainer { get { return _gearContainer; } }


    [Header("Connection")]
    private bool _isConnected; // Is Connected To Main Gear
    public bool IsConnected { get { return _isConnected; } }


    [Header("UI")]
    [SerializeField] private SubGearUI _subGearUI;
    [SerializeField] private GameObject _statIncreaseTextGob;


    public event Action<bool, float> OnStatChanged;

    public delegate float? GetSubGearStats(Vector3 position);

    public GetSubGearStats OnGetSubGearStats;
    private List<float?> _subGearPlusStatList;

    protected override void Initialize()
    {
        base.Initialize();
        _isConnected = false;
        DeActiveGlow();
    }

    public void SetGearUIIcon(GearImageSo imageSo, ESubGearStatState state)
    {
        _subGearUI.UpdateIconUI(imageSo.FeelingIconImage);
        _subGearUI.SetStateUI(state);
    }

    public void SetGearSprite(GearImageSo imageSo, int level)
    {
        _gearSprite.sprite = imageSo.GearImage;
        _gearSprite.color = imageSo.BodyColor;
        _gearMergeSprite.sprite = imageSo.GearMergeSo.GearMergeImage;
        _gearMergeSprite.color = imageSo.GearMergeSo.BodyColor[level];
    }
    public void SetGear(GearContainer statSo, StageMemory stageMemory, ESubGearStatState state)
    {
        _gearContainer = statSo;
        StageMemory = stageMemory;
        _subGearUI.UpdateUI(_gearContainer, state);
        _subGearSynergyList = statSo.GearSo.GearStat.SubGearSynergyList;
        InitializeSynergy();
    }

    public void MergeGear()
    {
        if (_isConnected)
            OnStatChanged?.Invoke(_isClockwise, -_gearContainer.GearSo.GearStat.GetTotalMemory(_gearContainer.Upgrade, _gearContainer.Level));
        _gearContainer.Level++;
        SetGearSprite(_gearContainer.GearSo.GearImage, _gearContainer.Level);
        _subGearUI.UpdateUI(_gearContainer);
        if (_isConnected)
            OnStatChanged?.Invoke(_isClockwise, _gearContainer.GearSo.GearStat.GetTotalMemory(_gearContainer.Upgrade, _gearContainer.Level));
    }


    public void ChangeGearUI(ESubGearStatState state)
    {
        _subGearUI.SetStateUI(state);
    }
    public override void SetClockwise(bool isClockwise)
    {
        base.SetClockwise(isClockwise);
        _subGearUI.SetIconDirection(isClockwise);
    }

    #region Connecting
    public void ConnectGear()
    {
        if (_isConnected)
            return;

        _isConnected = true;
        _gearRotater.StartRotating();
        OnStatChanged?.Invoke(_isClockwise, _gearContainer.GearSo.GearStat.GetTotalMemory(_gearContainer.Upgrade, _gearContainer.Level));
        AddSynergyable();
        ApplySynergy();
        SynergyManager.Instance.AddSynergy(_gearContainer.GearSo.GearStat.SynergyList, _gearContainer.GearSo.GearID, this);
    }
    public void DisconnectGear()
    {
        if (!_isConnected)
        {
            OnStatChanged = null;
            return;
        }
        else
        {
            _isConnected = false;
            OnStatChanged?.Invoke(_isClockwise, -_gearContainer.GearSo.GearStat.GetTotalMemory(_gearContainer.Upgrade, _gearContainer.Level));
            OnStatChanged = null;
            _gearRotater.StopRotating();
            RemoveSynergyable();
            RemoveSynergy();
            SynergyManager.Instance.RemoveSynergy(_gearContainer.GearSo.GearStat.SynergyList, _gearContainer.GearSo.GearID, this);
        }

    }

    public void RemoveGear()
    {
        DisconnectGear();
        Destroy(this.gameObject);
    }
    #endregion

    #region Stat
    public List<float?> CollectSubGearStat()
    {
        List<float?> statList = new List<float?>();
        _subGearPlusStatList = new List<float?>();
        if (OnGetSubGearStats != null)
        {
            foreach (GetSubGearStats method in OnGetSubGearStats.GetInvocationList())
            {
                _subGearPlusStatList.Add(method?.Invoke(transform.position));
            }
        }

        statList.Add(_subGearPlusStatList.Sum());
        return statList;
    }

    #endregion

    #region Synergy
    [Header("Synergy")]
    [SerializeField] private SpriteRenderer _glowSprite;
    private List<SubGearSynergySo> _subGearSynergyList;
    public Dictionary<ESynergyType, SynergySo> SynergyDic { get; set; }

    public void ActiveGlow()
    {
        _glowSprite.enabled = true;
    }

    public void DeActiveGlow()
    {
        _glowSprite.enabled = false;
    }
    public void InitializeSynergy()
    {
        SynergyDic = new Dictionary<ESynergyType, SynergySo>();
        foreach (var subGearSynergySo in _subGearSynergyList)
        {
            SynergyDic[subGearSynergySo.SynergyType] = subGearSynergySo;
        }
    }
    public void AddSynergyable()
    {
        SynergyManager.Instance.AddSynergyable(this);
    }

    public void RemoveSynergyable()
    {
        DeActiveGlow();
        SynergyManager.Instance.RemoveSynergyable(this);
    }

    public void ApplySynergy()
    {
        foreach (var synergySo in _subGearSynergyList)
        {
            synergySo.ApplySynergy(this);
        }
    }

    public void RemoveSynergy()
    {
        foreach (var synergySo in _subGearSynergyList)
        {
            synergySo.RemoveSynergy(this);
        }
    }

    #endregion

    #region Debuff

    public bool CheckCanDecreaseLevel()
    {
        return _gearContainer.Level > 0;
    }
    public void DecreaseGearLevel()
    {
        if (_isConnected)
            OnStatChanged?.Invoke(_isClockwise, -_gearContainer.GearSo.GearStat.GetTotalMemory(_gearContainer.Upgrade, _gearContainer.Level));
        _gearContainer.Level--;
        SetGearSprite(_gearContainer.GearSo.GearImage, _gearContainer.Level);
        _subGearUI.UpdateUI(_gearContainer);
        if (_isConnected)
            OnStatChanged?.Invoke(_isClockwise, _gearContainer.GearSo.GearStat.GetTotalMemory(_gearContainer.Upgrade, _gearContainer.Level));
    }
    #endregion
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _glowSprite = FindGameObjectInChildren<SpriteRenderer>("GlowSprite");
        _subGearUI = GetComponentInChildren<SubGearUI>();
        _gearSprite = FindGameObjectInChildren<SpriteRenderer>("SubGearSprite");
        _gearMergeSprite = FindGameObjectInChildren<SpriteRenderer>("SubGearMergeSprite");
    }
#endif
}

#region Legacy
/*
public void IncreaseStat(bool isFullCodition)
   {
       GearStatIncreaseText statIncreaseText = Instantiate(_statIncreaseGob, transform.position, Quaternion.identity).GetComponent<GearStatIncreaseText>();
       Debug.Log(_depth);
       float amount = _gearContainer.GearSo.GearStat.Memory[_gearContainer.Level] * _fSynergy;
       if (isFullCodition)
       {
           amount *= _fSynergy;

       }

       if (CheckBSynergyCritical())
       {
           if (SynergyStatDic.TryGetValue(EGearSynergy.B, out SynergyStatsSo synergyStat) &&
               synergyStat is BSynergyStatSo bSynergyStat)
           {
               amount *= (1 + (bSynergyStat.IncreasePercent[SynergyDic[EGearSynergy.B]] / 100f));
               statIncreaseText.SetText(amount, true);
           }
           else
           {
               throw new Exception($"{this.name} Synergy B Error");
           }
       }
       else
       {
           statIncreaseText.SetText(amount);
       }
       _memoryController.AddMemory(amount);
   }
   public (EgearSignType, float) GetSubGearStat(bool isFullCodition)
   {
       GearStatIncreaseText statIncreaseText = Instantiate(_statIncreaseGob, transform.position, Quaternion.identity).GetComponent<GearStatIncreaseText>();
       float amount = 0;
       switch (_gearContainer.GearSo.GearStat.GearType)
       {
           case EgearType.Fixed:
               amount = _gearContainer.GearSo.GearStat.Memory[_gearContainer.Level] * _fSynergy;
               break;
           case EgearType.Change:
           case EgearType.ExChange:
               amount = Random.Range(_gearContainer.GearSo.GearStat.RangeMemories[_gearContainer.Level].Min, _gearContainer.GearSo.GearStat.RangeMemories[_gearContainer.Level].Max + 1) * _fSynergy;
               break;
       }
       if (isFullCodition)
       {
           amount *= _fSynergy;

       }

       if (CheckBSynergyCritical())
       {
           if (SynergyStatDic.TryGetValue(EGearSynergy.B, out SynergyStatsSo synergyStat) &&
               synergyStat is BSynergyStatSo bSynergyStat)
           {
               amount *= (1 + (bSynergyStat.IncreasePercent[SynergyDic[EGearSynergy.B]] / 100f));
               statIncreaseText.SetText(amount, true);
           }
           else
           {
               throw new Exception($"{this.name} Synergy B Error");
           }
       }
       else
       {
           statIncreaseText.SetText(amount);
       }
       return (_gearContainer.GearSo.GearStat.SignType, amount);
   }
#region FSynergy
   [SerializeField] private float _fSynergy;

   private void RefreshFSynergy()
   {
       if (SynergyStatDic.TryGetValue(EGearSynergy.F, out SynergyStatsSo synergyStat) &&
           synergyStat is FSynergyStatSo fSynergyStat)
       {
           _fSynergy = 1 + (fSynergyStat.IncreasePercent[SynergyDic[EGearSynergy.F]] / 100f);
       }
       else
       {
           throw new Exception($"{this.name} Synergy F Error");
       }

   }

   #endregion

*/
#endregion

#region Legacy11.19

/*
using System;
   using System.Collections.Generic;
   using TMPro;
   using UnityEngine;
   using Random = UnityEngine.Random;
   
   public class SubGear : Gear, ISynergyable
   {
       [Header("Stat")]
       private GearContainer _gearContainer;
       public GearContainer GearContainer { get { return _gearContainer; } }
   
   
       [Header("Connection")]
       private bool _isConnected; // Is Connected To Main Gear
       public bool IsConnected { get { return _isConnected; } }
   
   
       [Header("UI")]
       [SerializeField] private TextMeshPro _levelText;
       [SerializeField] private GameObject _statIncreaseTextGob;
   
   
       public event Action<bool, EgearSignType, float> OnStatChanged;
   
       public delegate (EgearSignType, float)? GetSubGearStats();
   
       public GetSubGearStats OnGetSubGearStats;
   
       protected override void Initialize()
       {
           base.Initialize();
           _isConnected = false;
       }
   
       public void SetGear(GearContainer statSo, StageMemory stageMemory)
       {
           _gearContainer = statSo;
           StageMemory = stageMemory;
           _levelText.text = (_gearContainer.Level + 1).ToString();
           SetSynergies();
       }
   
       public void MergeGear()
       {
           if (_isConnected)
               OnStatChanged?.Invoke(_isClockwise, _gearContainer.GearSo.GearStat.SignType, -_gearContainer.GearSo.GearStat.GetTotalMemory(_gearContainer.Upgrade, _gearContainer.Level));
           _gearContainer.Level++;
           _levelText.text = (_gearContainer.Level + 1).ToString();
           if (_isConnected)
               OnStatChanged?.Invoke(_isClockwise, _gearContainer.GearSo.GearStat.SignType, _gearContainer.GearSo.GearStat.GetTotalMemory(_gearContainer.Upgrade, _gearContainer.Level));
       }
       #region Connecting
       public void ConnectGear()
       {
           _isConnected = true;
           _gearRotater.StartRotating();
           OnStatChanged?.Invoke(_isClockwise, _gearContainer.GearSo.GearStat.SignType, _gearContainer.GearSo.GearStat.GetTotalMemory(_gearContainer.Upgrade, _gearContainer.Level));
           AddSynergyable();
           SynergyManager.Instance.AddSynergy(_gearContainer.GearSo.GearStat.SynergyList, _gearContainer.GearSo.GearID);
       }
       public void DisconnectGear()
       {
           if (!_isConnected)
           {
               OnStatChanged = null;
               return;
           }
           else
           {
               _isConnected = false;
               OnStatChanged?.Invoke(_isClockwise, _gearContainer.GearSo.GearStat.SignType, -_gearContainer.GearSo.GearStat.GetTotalMemory(_gearContainer.Upgrade, _gearContainer.Level));
               OnStatChanged = null;
               _gearRotater.StopRotating();
               RemoveSynergyable();
               SynergyManager.Instance.RemoveSynergy(_gearContainer.GearSo.GearStat.SynergyList, _gearContainer.GearSo.GearID);
           }
   
       }
   
       public void RemoveGear()
       {
           DisconnectGear();
           Destroy(this.gameObject);
       }
       #endregion
   
       #region Stat
       public List<(EgearSignType, float)?> CollectSubGearStat()
       {
           List<(EgearSignType, float)?> statList = new List<(EgearSignType, float)?>();
           GearStatIncreaseText statIncreaseText = Instantiate(_statIncreaseTextGob, transform.position, Quaternion.identity).GetComponent<GearStatIncreaseText>();
           float amount = _gearContainer.GearSo.GearStat.GetTotalMemory(_gearContainer.Upgrade, _gearContainer.Level);
           if (_gearContainer.GearSo.GearStat.SignType == EgearSignType.Plus ||
               _gearContainer.GearSo.GearStat.SignType == EgearSignType.Minus)
           {
               statIncreaseText.SetText(amount);
           }
           else
           {
               statIncreaseText.SetText(amount, true);
           }
           statList.Add(GetBSynergy());
           return statList;
       }
       //public (EgearSignType, float) GetSubGearStat()
       //{
       //    return (_gearContainer.GearSo.GearStat.SignType, _gearContainer.GearSo.GearStat.UpgradeMemory[_gearContainer.Upgrade].Memory[_gearContainer.Level]);
       //}
   
       #endregion
   
   
   
       #region Synergy
   
   
   
       #region SynergyMain
       public Dictionary<EGearSynergy, int> SynergyDic { get; set; }
       public Dictionary<EGearSynergy, SynergyStatsSo> SynergyStatDic { get; set; }
   
       private bool CheckContainsSynergy(EGearSynergy synergy)
       {
           return SynergyDic.ContainsKey(synergy);
       }
   
       public void SetSynergies()
       {
           SynergyStatDic = new Dictionary<EGearSynergy, SynergyStatsSo>();
           SynergyDic = new Dictionary<EGearSynergy, int>();
           foreach (var synergy in _gearContainer.GearSo.GearStat.SubGearSynergyList)
           {
               SynergyDic[synergy.SynergyType] = 0;
               SynergyStatDic[synergy.SynergyType] = synergy;
           }
       }
   
       public void AddSynergyable()
       {
           SynergyManager.Instance.AddSynergyable(this);
       }
   
       public void RemoveSynergyable()
       {
           SynergyManager.Instance.RemoveSynergyable(this);
       }
       public void ApplySynergyEffect()
       {
       }
   
       public void UpdateSynergyLevel(EGearSynergy synergy, int level)
       {
           if (SynergyDic.ContainsKey(synergy))
           {
               SynergyDic[synergy] = level;
           }
           ApplySynergyEffect();
       }
       #endregion
       #region BSynergy
       private bool CheckBSynergyCritical()
       {
           int r = Random.Range(0, 100);
           if (SynergyStatDic.TryGetValue(EGearSynergy.B, out SynergyStatsSo synergyStat) &&
               synergyStat is BSynergyStatSo bSynergyStat)
           {
               return r < bSynergyStat.ActivePercent;
           }
           else
           {
               throw new Exception($"{this.name} Synergy B Error");
           }
       }
   
       private (EgearSignType, float)? GetBSynergy()
       {
           if (!CheckContainsSynergy(EGearSynergy.B) || SynergyDic[EGearSynergy.B] <= 0)
               return null;
   
           if (CheckBSynergyCritical())
           {
               if (SynergyStatDic.TryGetValue(EGearSynergy.B, out SynergyStatsSo synergyStat) &&
                   synergyStat is BSynergyStatSo bSynergyStat)
               {
                   float amount = bSynergyStat.IncreaseAmount[SynergyDic[EGearSynergy.B]];
                   GearStatIncreaseText statIncreaseText = Instantiate(_statIncreaseTextGob, transform.position, Quaternion.identity).GetComponent<GearStatIncreaseText>();
                   statIncreaseText.SetText(amount, true, true);
                   return (EgearSignType.Multiply, amount);
               }
               else
               {
                   throw new Exception($"{this.name} Synergy B Error");
               }
           }
           else
           {
               return null;
           }
       }
       #endregion
   
   
   
   
       #endregion
   
       #region Legacy
       /*
    public void IncreaseStat(bool isFullCodition)
          {
              GearStatIncreaseText statIncreaseText = Instantiate(_statIncreaseGob, transform.position, Quaternion.identity).GetComponent<GearStatIncreaseText>();
              Debug.Log(_depth);
              float amount = _gearContainer.GearSo.GearStat.Memory[_gearContainer.Level] * _fSynergy;
              if (isFullCodition)
              {
                  amount *= _fSynergy;
   
              }
   
              if (CheckBSynergyCritical())
              {
                  if (SynergyStatDic.TryGetValue(EGearSynergy.B, out SynergyStatsSo synergyStat) &&
                      synergyStat is BSynergyStatSo bSynergyStat)
                  {
                      amount *= (1 + (bSynergyStat.IncreasePercent[SynergyDic[EGearSynergy.B]] / 100f));
                      statIncreaseText.SetText(amount, true);
                  }
                  else
                  {
                      throw new Exception($"{this.name} Synergy B Error");
                  }
              }
              else
              {
                  statIncreaseText.SetText(amount);
              }
              _memoryController.AddMemory(amount);
          }
          public (EgearSignType, float) GetSubGearStat(bool isFullCodition)
          {
              GearStatIncreaseText statIncreaseText = Instantiate(_statIncreaseGob, transform.position, Quaternion.identity).GetComponent<GearStatIncreaseText>();
              float amount = 0;
              switch (_gearContainer.GearSo.GearStat.GearType)
              {
                  case EgearType.Fixed:
                      amount = _gearContainer.GearSo.GearStat.Memory[_gearContainer.Level] * _fSynergy;
                      break;
                  case EgearType.Change:
                  case EgearType.ExChange:
                      amount = Random.Range(_gearContainer.GearSo.GearStat.RangeMemories[_gearContainer.Level].Min, _gearContainer.GearSo.GearStat.RangeMemories[_gearContainer.Level].Max + 1) * _fSynergy;
                      break;
              }
              if (isFullCodition)
              {
                  amount *= _fSynergy;
   
              }
   
              if (CheckBSynergyCritical())
              {
                  if (SynergyStatDic.TryGetValue(EGearSynergy.B, out SynergyStatsSo synergyStat) &&
                      synergyStat is BSynergyStatSo bSynergyStat)
                  {
                      amount *= (1 + (bSynergyStat.IncreasePercent[SynergyDic[EGearSynergy.B]] / 100f));
                      statIncreaseText.SetText(amount, true);
                  }
                  else
                  {
                      throw new Exception($"{this.name} Synergy B Error");
                  }
              }
              else
              {
                  statIncreaseText.SetText(amount);
              }
              return (_gearContainer.GearSo.GearStat.SignType, amount);
          }
       #region FSynergy
          [SerializeField] private float _fSynergy;
   
          private void RefreshFSynergy()
          {
              if (SynergyStatDic.TryGetValue(EGearSynergy.F, out SynergyStatsSo synergyStat) &&
                  synergyStat is FSynergyStatSo fSynergyStat)
              {
                  _fSynergy = 1 + (fSynergyStat.IncreasePercent[SynergyDic[EGearSynergy.F]] / 100f);
              }
              else
              {
                  throw new Exception($"{this.name} Synergy F Error");
              }
   
          }
   
          #endregion
   
       * /
       #endregion
   #if UNITY_EDITOR
       protected override void OnBindField()
       {
           base.OnBindField();
           _levelText = FindGameObjectInChildren<TextMeshPro>("LevelText");
       }
   #endif
   }
   

*/
#endregion