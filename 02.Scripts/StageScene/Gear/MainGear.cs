using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MainGear : Gear, ISynergyable
{

    private SortedDictionary<int, HashSet<SubGear>> _connectedGears = new SortedDictionary<int, HashSet<SubGear>>();

    [Header("Stat")]
    [SerializeField] private PlayerStatSo _playerStatSo;
    private float _curTime;
    private MainGearStat _stat;

    private int _subGearCount;
    public int SubGearCount { get { return _subGearCount; } }
    private int _power;
    private float _statIncreaseTime;
    private float _attachedGearDecreaseAmount;


    [Header("UI")]
    [SerializeField] private MainGearStatUI _statUI;
    [SerializeField] private MainGearTimeUI _timeUI;
    [SerializeField] private GearClockStats _clockStatUI;
    [SerializeField] private GameObject _statIncreaseTextGob;
    [SerializeField] private Vector3 _statIncreaseOffSet;
    #region Action & Delegate
    [Header("Stat Increase Speed")]
    public Action OnSpeedChanged;
    public delegate float GetStatIncreaseSpeedRatio();
    public GetStatIncreaseSpeedRatio OnGetStatIncreaseSpeedRatio;
    private List<float> _statIncreaseSpeedRatioList;

    //[field: Header("Attached Gear Speed Decrease")]
    public delegate float GetSpeedDecreaseRatio();
    public GetSpeedDecreaseRatio OnGetSpeedDecreaseRatio;
    private List<float> _statDecreaseRatioList;

    public delegate float GetStatIndecreaseAmount();
    public GetStatIndecreaseAmount OnGetStatIndecreaseAmount; // debuff
    private List<float> _statIndecreaseList;
    [Header("Efficiency Changed")]
    public Action OnEfficiencyChanged;


    //[field: Header("Memory Increased")]
    public event Action OnMemoryIncreased;


    //[field: Header("Additive Memory Ratio")]
    public delegate float GetAdditiveStatRatio();
    public GetAdditiveStatRatio OnGetAdditiveStatRatio;
    private List<float> _additiveStatRatioList;

    public delegate float GetDebuffStatRatio();
    public GetDebuffStatRatio OnGetDebuffStatRatio;
    //[field: Header("Additive Memory ")]
    public delegate float GetAdditiveMemory(float amount);
    public GetAdditiveMemory OnGetAdditiveMemory;
    private List<float> _onGearAdditiveList;
    #endregion



    protected override void Initialize()
    {
        base.Initialize();
        _stat = new MainGearStat(_playerStatSo);
        _power = _playerStatSo.CurrentPlayerStat.Power;
        _statIncreaseTime = _playerStatSo.CurrentPlayerStat.StatIncreaseTime;
        _attachedGearDecreaseAmount = _playerStatSo.CurrentPlayerStat.AttachedGearDecreaseAmount;
        _curTime = 0;
        _subGearCount = 0;
        _timePaused = true;
        _gearRotater.StartRotating();
        SetMainGearSynergy();
        InitializeActionDelegate();
        UpdateStatUI(true);
        UpdateStatUI(false);
    }

    private void InitializeActionDelegate()
    {
        OnSpeedChanged += ChangeGearSpeed;
        OnEfficiencyChanged += UpdateEfficiency;
        OnEfficiencyChanged += UpdateUI;
        OnEfficiencyChanged?.Invoke();
    }
    protected void Update()
    {
        if (_timePaused)
            return;
        StatIncreaseTimer();
        UpdateSynergy();
        if (OnGetAdditiveStatRatio != null)
        {
            _additiveStatRatioList = new List<float>();
            foreach (GetAdditiveStatRatio ratio in OnGetAdditiveStatRatio.GetInvocationList())
            {
                _additiveStatRatioList.Add(ratio.Invoke());
            }
            _statUI.UpdatePlusRatioText(_additiveStatRatioList.Sum());
        }
        else
        {
            _statUI.UpdatePlusRatioText(0);
        }
    }



    #region Speed
    private void ChangeGearSpeed()
    {
        _statIncreaseTime = _playerStatSo.CurrentPlayerStat.StatIncreaseTime;
        if (OnGetStatIndecreaseAmount != null)
        {
            _statIndecreaseList = new List<float>();
            foreach (GetStatIndecreaseAmount amount in OnGetStatIndecreaseAmount.GetInvocationList())
            {
                _statIndecreaseList.Add(amount.Invoke());
            }
            _statIncreaseTime += _statIndecreaseList.Sum();
        }

        if (OnGetStatIncreaseSpeedRatio != null)
        {
            _statIncreaseSpeedRatioList = new List<float>();
            foreach (GetStatIncreaseSpeedRatio ratio in OnGetStatIncreaseSpeedRatio.GetInvocationList())
            {
                _statIncreaseSpeedRatioList.Add(ratio.Invoke());
            }
            _statIncreaseTime = _playerStatSo.CurrentPlayerStat.StatIncreaseTime * (_statIncreaseSpeedRatioList.Sum() / _statIncreaseSpeedRatioList.Count);
        }
    }

    #endregion
    #region Efficiency

    public void IncreasePower()
    {
        _power += 1;
        OnEfficiencyChanged?.Invoke();
    }

    public float GetEfficiency()
    { return _stat.Efficiency; }
    private void UpdateEfficiency()
    {
        int count = 0;
        foreach (var VARIABLE in _connectedGears)
        {
            count += VARIABLE.Value.Count;
        }
        _subGearCount = count;
        float amount;

        if (count > _power)
        {
            if (OnGetSpeedDecreaseRatio == null)
            {
                amount = ((_subGearCount - _power) * _attachedGearDecreaseAmount);
            }
            else
            {
                _statDecreaseRatioList = new List<float>();
                foreach (GetSpeedDecreaseRatio method in OnGetSpeedDecreaseRatio.GetInvocationList())
                {
                    _statDecreaseRatioList.Add(method.Invoke());
                }
                amount = (_subGearCount - _power) * (_attachedGearDecreaseAmount / _statDecreaseRatioList.Sum());
            }

        }
        else
        {
            amount = 0;
        }
        _stat.SetEfficiency(Mathf.Max(1 - amount, 0));
    }
    #endregion
    #region UI
    private void UpdateUI()
    {
        UpdateMainGearUI();
    }
    private void UpdateMainGearUI()
    {

        _statUI.UpdateCurrentPowerText(_subGearCount, _power);
        _statUI.UpdateEfficiencyText(_stat.Efficiency);
    }

    private void UpdateStatUI(bool clockwise)
    {
        if (clockwise)
        {
            _clockStatUI.SetClockStat(_stat.ClockPlusStat);
        }
        else
        {
            _clockStatUI.SetCounterClockStat(_stat.CounterPlusStat);
        }
    }
    #endregion
    #region Connection

    private void UpdateStat(bool isClockwise, float stat)
    {
        _stat.UpdateStat(isClockwise, stat);
        UpdateStatUI(isClockwise);
        UpdateUI();
    }
    public void RemoveGear(int depth, SubGear gear)
    {
        if (_connectedGears.ContainsKey(depth) && _connectedGears[depth].Contains(gear))
        {
            _connectedGears[depth].Remove(gear);
        }
        gear.RemoveGear();
    }

    public void SetConnectedGear(Dictionary<int, HashSet<SubGear>> connectedGearDic)
    {
        foreach (var dicValue in connectedGearDic)
        {
            if (!_connectedGears.ContainsKey(dicValue.Key))
            {
                _connectedGears[dicValue.Key] = new HashSet<SubGear>();
            }

            foreach (var gear in dicValue.Value)
            {
                if (_connectedGears[dicValue.Key].Add(gear))
                {
                    gear.OnStatChanged += UpdateStat;
                    gear.ConnectGear();
                }
            }
        }
        HashSet<(int, SubGear)> disconnectGearList = new HashSet<(int, SubGear)>();

        foreach (var dicValue in _connectedGears)
        {
            if (!connectedGearDic.ContainsKey(dicValue.Key))
            {
                foreach (var gear in dicValue.Value)
                {
                    disconnectGearList.Add((dicValue.Key, gear));
                }
            }
            else
            {
                foreach (var gear in dicValue.Value)
                {
                    if (!connectedGearDic[dicValue.Key].Contains(gear))
                    {
                        disconnectGearList.Add((dicValue.Key, gear));
                    }
                }
            }
        }

        foreach (var gear in disconnectGearList)
        {
            _connectedGears[gear.Item1].Remove(gear.Item2);
            gear.Item2.DisconnectGear();
        }
        OnEfficiencyChanged?.Invoke();
    }

    #endregion
    #region Memory Increase

    private bool CheckClockwiseByDepth(int depth)
    {
        return (depth % 2 == 1 && _isClockwise);
    }
    private void StatIncreaseTimer()
    {

        _curTime += (Time.deltaTime * StageSceneManager.Instance.TimeRatio);
        _timeUI.UpdateTimeImage(_curTime / (_statIncreaseTime / _stat.Efficiency));
        if (_curTime >= (_statIncreaseTime / _stat.Efficiency))
        {
            _curTime = 0;
            IncreaseStat();
        }
    }

    private void IncreaseStat()
    {

        float clockPlus = 0;
        float counterPlus = 0;

        for (int i = 0; i < 20; i++)
        {
            if (_connectedGears.ContainsKey(i))
            {
                bool isClockwise = CheckClockwiseByDepth(i);

                foreach (var gear in _connectedGears[i])
                {
                    var statList = gear.CollectSubGearStat();
                    foreach (var stat in statList)
                    {
                        if (!stat.HasValue)
                            break;
                        if (isClockwise)
                        {
                            clockPlus += stat.Value;
                        }
                        else
                        {
                            counterPlus += stat.Value;
                        }
                    }
                }
            }

        }


        float clockStat = (_stat.ClockPlusStat + clockPlus) * (_stat.CounterPlusStat + counterPlus);
        float amount = clockStat;

        OnMemoryIncreased?.Invoke();
        IncreaseMemory(amount);
    }

    private void IncreaseMemory(float amount)
    {
        if (OnGetAdditiveStatRatio != null)
        {
            _additiveStatRatioList = new List<float>();
            foreach (GetAdditiveStatRatio method in OnGetAdditiveStatRatio.GetInvocationList())
            {
                _additiveStatRatioList.Add(method());
            }
            amount *= (1 + (_additiveStatRatioList.Sum()));
        }
        float additiveMemory = 0;
        if (OnGetDebuffStatRatio != null)
        {
            amount *= OnGetDebuffStatRatio.Invoke();
        }
        if (OnGetAdditiveMemory != null)
        {
            _onGearAdditiveList = new List<float>();
            foreach (GetAdditiveMemory method in OnGetAdditiveMemory.GetInvocationList())
            {
                _onGearAdditiveList.Add(method.Invoke(amount));
            }
            additiveMemory += _onGearAdditiveList.Sum();
        }

        float totalMemory = amount + additiveMemory;

        StageMemory.AddMemory(totalMemory);
        GearStatIncreaseText statIncreaseText = Instantiate(_statIncreaseTextGob, transform.position, Quaternion.identity).GetComponent<GearStatIncreaseText>();
        statIncreaseText.SetMainGearText(totalMemory, _statIncreaseOffSet);
    }

    #endregion
    #region Event

    private bool _timePaused = true;
    private void OnEnable()
    {
        AddSynergyable();
        StageSceneManager.Instance.EventStageScene.OnTimePaused += Event_TimePaused;
        StageSceneManager.Instance.EventStageScene.OnTimeResumed += Event_TimeResumed;
    }

    private void OnDisable()
    {
        RemoveSynergyable();
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


    #region Synergy
    [Header("Synergy")]
    [SerializeField] private List<MainGearSynergySo> _mainGearSynergyList;
    public Dictionary<ESynergyType, SynergySo> SynergyDic { get; set; }

    public void InitializeSynergy()
    {
        SynergyDic = new Dictionary<ESynergyType, SynergySo>();
        foreach (var mainGearSynergySo in _mainGearSynergyList)
        {
            SynergyDic[mainGearSynergySo.SynergyType] = mainGearSynergySo;
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

    private void SetMainGearSynergy()
    {
        foreach (var mainGearSynergySo in _mainGearSynergyList)
        {
            mainGearSynergySo.SetMainGear(this);
        }
    }
    public void ApplySynergy()
    {
        foreach (var mainGearSynergySo in _mainGearSynergyList)
        {
            mainGearSynergySo.ApplySynergy();
        }
    }

    public void RemoveSynergy()
    {
        foreach (var mainGearSynergySo in _mainGearSynergyList)
        {
            mainGearSynergySo.RemoveSynergy();
        }
    }

    private void UpdateSynergy()
    {
        foreach (var mainGearSynergySo in _mainGearSynergyList)
        {
            mainGearSynergySo.UpdateSynergy();
        }
    }


    public (int three, int four) GetGearsCountByLevel()
    {
        int three = 0, four = 0;

        foreach (var connectedGear in _connectedGears)
        {
            foreach (var gear in connectedGear.Value)
            {
                if (gear.GearContainer.Level == 2)
                    three++;
                if (gear.GearContainer.Level == 3)
                    four++;
            }
        }
        return (three, four);
    }
    #endregion

    #region Debuff

    public int GetOverGearCount()
    {
        return _subGearCount - _power;
    }

    #endregion
    #region Legacy
    /*
    #region Rotation
       private void SetRotations()
       {
           _gearRotationSo.CurrentMainGearRotation = _gearRotationSo.MainGearStartRotation;
           _gearRotationSo.CurrentSubGearClockwiseRotation = _gearRotationSo.SubGearStartClockwiseRotation;
           _gearRotationSo.CurrentSubGearCounterClockwiseRotation = _gearRotationSo.SubGearStartCounterClockwiseRotation;
           transform.eulerAngles = _gearRotationSo.CurrentMainGearRotation;
       }
       protected override void RotateGear()
       {
           if (_isClockwise)
           {
               _gearRotationSo.CurrentMainGearRotation -= new Vector3(0, 0, _mainGearSpeed * Time.deltaTime * _efficiency);
               if (_gearRotationSo.CurrentMainGearRotation.z < 0)
               {
                   _gearRotationSo.CurrentMainGearRotation = new Vector3(0, 0, 360);
               }
           }
           else
           {
               _gearRotationSo.CurrentMainGearRotation += new Vector3(0, 0, _mainGearSpeed * Time.deltaTime * _efficiency);
               if (_gearRotationSo.CurrentMainGearRotation.z > 360)
               {
                   _gearRotationSo.CurrentMainGearRotation = Vector3.zero;
               }
           }

           _gearRotationSo.CurrentSubGearClockwiseRotation -= new Vector3(0, 0, _subGearSpeed * Time.deltaTime * _efficiency);
           if (_gearRotationSo.CurrentSubGearClockwiseRotation.z < 0)
           {
               _gearRotationSo.CurrentSubGearClockwiseRotation = new Vector3(0, 0, 360);
           }
           _gearRotationSo.CurrentSubGearCounterClockwiseRotation += new Vector3(0, 0, _subGearSpeed * Time.deltaTime * _efficiency);
           if (_gearRotationSo.CurrentSubGearCounterClockwiseRotation.z > 360)
           {
               _gearRotationSo.CurrentSubGearCounterClockwiseRotation = Vector3.zero;
           }


           transform.eulerAngles = _gearRotationSo.CurrentMainGearRotation;
       }
       #endregion
    */
    #endregion
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        StageMemory = GameObject.FindAnyObjectByType<StageMemory>();
        _timeUI = GetComponentInChildren<MainGearTimeUI>();
        _statIncreaseOffSet = FindGameObjectInChildren("TimeImage").transform.position;
        _statUI = GameObject.FindAnyObjectByType<MainGearStatUI>();
        _clockStatUI = GameObject.FindAnyObjectByType<GearClockStats>();
    }

#endif
}

#region Legacy11.19
/*
using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Drawing;
   using Unity.VisualScripting;
   using UnityEngine;
   
   public class MainGear : Gear, ISynergyable
   {
   
       private SortedDictionary<int, HashSet<SubGear>> _connectedGears = new SortedDictionary<int, HashSet<SubGear>>();
   
       [Header("Stat")]
       [SerializeField] private PlayerStatSo _playerStatSo;
       private float _curTime;
       private MainGearStat _stat;
       private Coroutine _statIncreaseRoutine;
   
       private int _subGearCount;
       private float _power;
       private float _statIncreaseTime;
       private float _attachedGearDecreaseAmount;
   
   
       [Header("UI")]
       [SerializeField] private MainGearStatUI _statUI;
       [SerializeField] private GameObject _statIncreaseTextGob;
   
       protected override void Initialize()
       {
           base.Initialize();
           _stat = new MainGearStat(_playerStatSo);
           _power = _playerStatSo.CurrentPlayerStat.Power;
           _statIncreaseTime = _playerStatSo.CurrentPlayerStat.StatIncreaseTime;
           _attachedGearDecreaseAmount = _playerStatSo.CurrentPlayerStat.AttachedGearDecreaseAmount;
           _statIncreaseRoutine = null;
           _stageEnded = false;
           _stageSucceed = false;
           _curTime = 0;
           _subGearCount = 0;
           _gearRotater.StartRotating();
           SetSynergies();
           UpdateEfficiency();
           UpdateUI();
   
       }
   
   
       protected void Update()
       {
           if (_stageEnded || _stageSucceed)
               return;
           StatIncreaseTimer();
       }
       #region Efficiency
       public float GetEfficiency()
       { return _stat.Efficiency; }
       private void UpdateEfficiency()
       {
           int count = 0;
           foreach (var VARIABLE in _connectedGears)
           {
               count += VARIABLE.Value.Count;
           }
   
           _subGearCount = count;
           float amount;
   
           if (count > _power)
           {
               amount = (_subGearCount - _power) * _attachedGearDecreaseAmount / _eSynergy;
           }
           else
           {
               amount = 0;
           }
           _stat.SetEfficiency(Mathf.Max(1 - amount, 0));
           UpdateUI();
       }
   
       #endregion
       #region UI
   
       private void UpdateUI()
       {
           UpdateStatUI();
       }
       private void UpdateStatUI()
       {
           _statUI.UpdateCurrentPowerText(_subGearCount, _power);
           _statUI.UpdateEfficiencyText(_stat.Efficiency);
           _statUI.SetClockStat(_stat.ClockPlusStat, _stat.ClockMultiStat);
           _statUI.SetCounterClockStat(_stat.CounterPlusStat, _stat.CounterMultiStat);
       }
   
       #endregion
       #region Connection
   
       private void UpdateStat(bool isClockwise, EgearSignType type, float stat)
       {
           _stat.UpdateStat(isClockwise, type, stat);
           UpdateStatUI();
       }
       public void RemoveGear(int depth, SubGear gear)
       {
           if (_connectedGears.ContainsKey(depth) && _connectedGears[depth].Contains(gear))
           {
   
               _connectedGears[depth].Remove(gear);
               //if (gear.IsConnected)
               //{
               //    var stat = gear.GetSubGearStat();
               //    _stat.UpdateStat(CheckClockwiseByDepth(depth), stat.Item1, -stat.Item2);
               //}
           }
           gear.RemoveGear();
       }
   
       public void SetConnectedGear(Dictionary<int, HashSet<SubGear>> connectedGearDic)
       {
           foreach (var dicValue in connectedGearDic)
           {
               if (!_connectedGears.ContainsKey(dicValue.Key))
               {
                   _connectedGears[dicValue.Key] = new HashSet<SubGear>();
               }
   
               foreach (var gear in dicValue.Value)
               {
                   if (_connectedGears[dicValue.Key].Add(gear))
                   {
                       gear.OnStatChanged += UpdateStat;
                       gear.ConnectGear();
                       //var stat = gear.GetSubGearStat();
                       //_stat.UpdateStat(CheckClockwiseByDepth(dicValue.Key), stat.Item1, stat.Item2);
                   }
               }
   
           }
           HashSet<(int, SubGear)> disconnectGearList = new HashSet<(int, SubGear)>();
   
           foreach (var dicValue in _connectedGears)
           {
               if (!connectedGearDic.ContainsKey(dicValue.Key))
               {
                   foreach (var gear in dicValue.Value)
                   {
                       disconnectGearList.Add((dicValue.Key, gear));
                   }
               }
               else
               {
                   foreach (var gear in dicValue.Value)
                   {
                       if (!connectedGearDic[dicValue.Key].Contains(gear))
                       {
                           disconnectGearList.Add((dicValue.Key, gear));
                       }
                   }
               }
           }
   
           foreach (var gear in disconnectGearList)
           {
               _connectedGears[gear.Item1].Remove(gear.Item2);
               //var stat = gear.Item2.GetSubGearStat();
               //_stat.UpdateStat(CheckClockwiseByDepth(gear.Item1), stat.Item1, -stat.Item2);
               gear.Item2.DisconnectGear();
           }
           UpdateEfficiency();
           UpdateUI();
       }
   
       #endregion
       #region Memory Increase
   
       private bool CheckClockwiseByDepth(int depth)
       {
           return (depth % 2 == 1 && _isClockwise);
       }
       private void StatIncreaseTimer()
       {
   
           _curTime += Time.deltaTime;
           if (_curTime >= (_statIncreaseTime / _stat.Efficiency))
           {
               _curTime = 0;
               _statIncreaseRoutine = StartCoroutine(IncreaseStat());
           }
       }
   
       private IEnumerator IncreaseStat()
       {
   
           float clockPlus = 0;
           float clockMulti = 0;
           float counterPlus = 0;
           float counterMulti = 0;
   
           for (int i = 0; i < 20; i++)
           {
               if (_connectedGears.ContainsKey(i))
               {
                   bool isClockwise = CheckClockwiseByDepth(i);
   
                   foreach (var gear in _connectedGears[i])
                   {
                       var statList = gear.CollectSubGearStat();
                       foreach (var stat in statList)
                       {
                           if (!stat.HasValue)
                               break;
                           if (isClockwise)
                           {
                               switch (stat.Value.Item1)
                               {
                                   case EgearSignType.Plus:
                                       clockPlus += stat.Value.Item2;
                                       break;
                                   case EgearSignType.Minus:
                                       clockPlus -= stat.Value.Item2;
                                       break;
                                   case EgearSignType.Multiply:
                                       clockMulti += stat.Value.Item2;
                                       break;
                               }
                           }
                           else
                           {
                               switch (stat.Value.Item1)
                               {
                                   case EgearSignType.Plus:
                                       counterPlus += stat.Value.Item2;
                                       break;
                                   case EgearSignType.Minus:
                                       counterPlus -= stat.Value.Item2;
                                       break;
                                   case EgearSignType.Multiply:
                                       counterMulti += stat.Value.Item2;
                                       break;
                               }
                           }
                       }
                   }
                   yield return new WaitForSeconds(0.15f);
               }
   
           }
   
   
   
           float clockStat = (_stat.ClockPlusStat + clockPlus) * (_stat.ClockMultiStat + clockMulti);
   
           float counterClockStat = (_stat.CounterPlusStat + counterPlus) * (_stat.CounterMultiStat + counterMulti);
           if (_debuffReady && _debuffSo is ADebuffSo aDebuff)
           {
               if (aDebuff.IsClockDirection)
               {
                   clockStat = 0;
               }
               else
               {
                   counterClockStat = 0;
               }
           }
   
   
           float amount = clockStat + counterClockStat;
   
           UpdateEfficiency();
           IncreaseASynergyStat();
           IncreaseMemoryByGSynergy();
           IncreaseHSynergyStat();
           IncreaseMemory(amount);
           IncreaseDebuffCount();
           RestartGSynergy();
           yield return new WaitForSeconds(1f);
           IncreaseEnd();
       }
   
       private void IncreaseMemory(float amount)
       {
           amount *= HSynergyMemoryRatio() * FSynergyMemoryRatio();
           float gearStatAmount = amount * IncreaseMemoryByGSynergy();
           if (_debuffReady && _debuffSo is BDebuffSo bDebuff)
           {
               amount = 0;
               gearStatAmount = 0;
           }
           StageMemory.AddMemory(amount + gearStatAmount);
           GearStatIncreaseText statIncreaseText = Instantiate(_statIncreaseTextGob, transform.position, Quaternion.identity).GetComponent<GearStatIncreaseText>();
           statIncreaseText.SetMainGearText(amount + gearStatAmount);
           ResetDebuff();
       }
   
       private void IncreaseEnd()
       {
           _statIncreaseRoutine = null;
           if (_stageSucceed)
               return;
           if (_stageEnded)
           {
               StageSceneManager.Instance.EventStageScene.CallFinalScoreAdded();
           }
           CheckDebuffCount();
       }
   
       #endregion
       #region Event
       private bool _stageEnded;
       private bool _stageSucceed;
       private void OnEnable()
       {
           AddSynergyable();
           StageSceneManager.Instance.EventStageScene.OnStageTimeEnded += Event_StageTimeEnded;
           StageSceneManager.Instance.EventStageScene.OnStageSucceed += Event_StageSucceed;
           StageSceneManager.Instance.EventStageScene.OnActiveDebuff += Event_ActiveDebuff;
       }
   
       private void OnDisable()
       {
           RemoveSynergyable();
           StageSceneManager.Instance.EventStageScene.OnStageTimeEnded -= Event_StageTimeEnded;
           StageSceneManager.Instance.EventStageScene.OnStageSucceed -= Event_StageSucceed;
           StageSceneManager.Instance.EventStageScene.OnActiveDebuff -= Event_ActiveDebuff;
       }
   
       private void Event_StageTimeEnded()
       {
           _stageEnded = true;
           if (_statIncreaseRoutine == null)
           {
               StageSceneManager.Instance.EventStageScene.CallFinalScoreAdded();
           }
       }
   
       private void Event_StageSucceed()
       {
           _stageSucceed = true;
       }
   
       private void Event_ActiveDebuff(StageSceneDebuffEventArgs eventArgs)
       {
           _debuffSo = eventArgs.DebuffSo;
       }
   
       #endregion
   
   
       #region Synergy
       #region SynergyMain
       [Header("Synergy")]
       [SerializeField] private List<SynergyStatsSo> _synergyList;
       public Dictionary<EGearSynergy, int> SynergyDic { get; set; }
       public Dictionary<EGearSynergy, SynergyStatsSo> SynergyStatDic { get; set; }
       public void SetSynergies()
       {
           SynergyStatDic = new Dictionary<EGearSynergy, SynergyStatsSo>();
           SynergyDic = new Dictionary<EGearSynergy, int>();
           foreach (var synergy in _synergyList)
           {
               SynergyDic[synergy.SynergyType] = 0;
               SynergyStatDic[synergy.SynergyType] = synergy;
           }
   
           _curAAmount = 0;
           _eSynergy = 1f;
           _curGAmount = 0;
           _curHAmount = 0;
           _hStack = 0;
           _curFSynergy = 0;
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
           RefreshASynergy();
           RefreshESynergy();
           RefreshFSynergy();
           RefreshGSynergy();
           RefreshHSynergy();
       }
   
       public void UpdateSynergyLevel(EGearSynergy synergy, int level)
       {
           SynergyDic[synergy] = level;
           ApplySynergyEffect();
       }
   
   
       #endregion
       #region ASynergy
       [SerializeField] private int _curAAmount;
       private void IncreaseASynergyStat()
       {
           if (SynergyDic[EGearSynergy.A] > 0)
           {
               _curAAmount++;
               RefreshASynergy();
           }
       }
       private void RefreshASynergy()
       {
           if (SynergyStatDic.TryGetValue(EGearSynergy.A, out SynergyStatsSo synergyStat) &&
               synergyStat is ASynergyStatSo aSynergyStat)
           {
               if (_curAAmount >= aSynergyStat.Amount[SynergyDic[EGearSynergy.A]])
               {
                   _curAAmount -= aSynergyStat.Amount[SynergyDic[EGearSynergy.A]];
                   _power++;
                   UpdateEfficiency();
                   UpdateUI();
               }
           }
           else
           {
               throw new Exception($"{this.name} Synergy A Error");
           }
   
       }
   
   
   
       #endregion
       #region ESynergy
       [SerializeField] private float _eSynergy;
       private void RefreshESynergy()
       {
           if (SynergyDic[EGearSynergy.E] == 0)
           {
               _eSynergy = 1;
           }
           else
           {
   
               if (SynergyStatDic.TryGetValue(EGearSynergy.E, out SynergyStatsSo synergyStat) &&
                   synergyStat is ESynergyStatSo eSynergyStat)
               {
                   _eSynergy = eSynergyStat.DecreaseAmount[SynergyDic[EGearSynergy.E]];
               }
               else
               {
                   throw new Exception($"{this.name} Synergy E Error");
               }
   
           }
       }
   
   
   
       #endregion
       #region FSynergy
   
       private float _curFSynergy;
   
       private void RefreshFSynergy()
       {
           if (SynergyDic[EGearSynergy.F] == 0)
           {
               _curFSynergy = 0;
           }
           else
           {
   
               if (SynergyStatDic.TryGetValue(EGearSynergy.F, out SynergyStatsSo synergyStat) &&
                   synergyStat is FSynergyStatSo fSynergyStat)
               {
                   _curFSynergy = fSynergyStat.IncreasePercent[SynergyDic[EGearSynergy.F]];
               }
               else
               {
                   throw new Exception($"{this.name} Synergy F Error");
               }
           }
           _statUI.SetFSynergy(SynergyDic[EGearSynergy.F], _curFSynergy);
       }
       private float FSynergyMemoryRatio()
       {
           if (SynergyDic[EGearSynergy.F] <= 0)
           {
               return 1;
           }
           else
           {
               return 1 + (_curFSynergy / 100f);
           }
       }
       #endregion
       #region GSynergy
   
       [SerializeField] private float _curGAmount;
       [SerializeField] private float _targetGAmount;
       private Coroutine _synergyGRoutine;
       private GSynergyUI _gSynergyUI;
       private void RefreshGSynergy()
       {
   
           if (SynergyDic[EGearSynergy.G] == 0)
           {
               if (_synergyGRoutine != null)
               {
                   StopCoroutine(_synergyGRoutine);
                   _synergyGRoutine = null;
               }
               _curGAmount = 0;
               if (_gSynergyUI != null)
               {
                   _gSynergyUI.gameObject.SetActive(false);
               }
           }
           else
           {
               if (SynergyStatDic.TryGetValue(EGearSynergy.G, out SynergyStatsSo synergyStat) &&
                   synergyStat is GSynergyStatSo gSynergyStat)
               {
                   _targetGAmount = gSynergyStat.IncreaseAmount[SynergyDic[EGearSynergy.G]];
                   if (_gSynergyUI == null)
                   {
                       _gSynergyUI = Instantiate(gSynergyStat.IncreaseStatUI, transform).GetComponent<GSynergyUI>();
                   }
               }
               else
               {
                   throw new Exception($"{this.name} Synergy G Error");
               }
           }
       }
       private float IncreaseMemoryByGSynergy()
       {
           if (SynergyDic[EGearSynergy.G] <= 0)
           {
               return 0;
           }
   
           if (_synergyGRoutine != null)
           {
               StopCoroutine(_synergyGRoutine);
           }
   
           return _curGAmount / 100f;
       }
   
       private void RestartGSynergy()
       {
           if (SynergyDic[EGearSynergy.G] <= 0)
           {
               return;
           }
           else
           {
               _gSynergyUI.gameObject.SetActive(true);
               _synergyGRoutine = StartCoroutine(CoGSynergy());
           }
       }
       private IEnumerator CoGSynergy()
       {
   
           _curGAmount = _targetGAmount;
           _gSynergyUI.SetText(_curGAmount);
           float decreaseAmount = 0;
           if (SynergyStatDic.TryGetValue(EGearSynergy.G, out SynergyStatsSo synergyStat) &&
               synergyStat is GSynergyStatSo gSynergyStat)
           {
               decreaseAmount = gSynergyStat.DecreaseAmountPerSec;
           }
           else
           {
               throw new Exception($"{this.name} Synergy G Error");
           }
           while (_curGAmount >= 0)
           {
               yield return new WaitForSeconds(1f);
               _curGAmount -= decreaseAmount;
               _gSynergyUI.SetText(_curGAmount);
           }
           _curGAmount = 0;
       }
   
       /*private void IncreaseMemoryByGSynergy()
       {
           if (SynergyDic[EGearSynergy.G] <= 0)
           {
               return;
           }
   
           if (_synergyGRoutine != null)
           {
               StopCoroutine(_synergyGRoutine);
           }
           _memoryController.AddMemory(_curGAmount);
           _gSynergyUI.gameObject.SetActive(true);
           _synergyGRoutine = StartCoroutine(CoGSynergy());
       }* /
       #endregion
       #region HSynergy
       private int _curHAmount;
       private int _hStack;
       private int _hRatio;
       private void RefreshHSynergy()
       {
           if (SynergyStatDic.TryGetValue(EGearSynergy.H, out SynergyStatsSo synergyStat) &&
               synergyStat is HSynergyStatSo hSynergyStat)
           {
               _hRatio = hSynergyStat.IncreaseStatRatio[SynergyDic[EGearSynergy.H]];
           }
           else
           {
               throw new Exception($"{this.name} Synergy H Error");
           }
           _statUI.SetHSynergy(SynergyDic[EGearSynergy.H], _hStack, _hRatio);
       }
       private void IncreaseHSynergyStat()
       {
           if (SynergyDic[EGearSynergy.H] <= 0)
           {
               return;
           }
           _curHAmount++;
           if (SynergyStatDic.TryGetValue(EGearSynergy.H, out SynergyStatsSo synergyStat) &&
               synergyStat is HSynergyStatSo hSynergyStat)
           {
               if (_curHAmount >= hSynergyStat.RequireTurnStack)
               {
                   _hStack++;
                   _curHAmount = 0;
               }
           }
           else
           {
               throw new Exception($"{this.name} Synergy H Error");
           }
           _statUI.SetHSynergy(SynergyDic[EGearSynergy.H], _hStack, _hRatio);
       }
   
       private float HSynergyMemoryRatio()
       {
           if (SynergyDic[EGearSynergy.H] <= 0)
           {
               return 1;
           }
           else
           {
               return 1 + ((_hRatio * _hStack) / 100f);
           }
       }
   
   
       #endregion
       #endregion
   
       #region Debuff
       [SerializeField] private DebuffSo _debuffSo;
       private bool _debuffReady = false;
       private void IncreaseDebuffCount()
       {
           if (_debuffSo != null)
           {
               _debuffSo.AddCount();
           }
       }
   
       private void CheckDebuffCount()
       {
           if (_debuffSo == null)
               _debuffReady = false;
           else
               _debuffReady = _debuffSo.CheckDebuffCount();
   
   
           if (_debuffReady)
               ReadyDebuff();
       }
   
       private void ReadyDebuff()
       {
           if (_debuffSo is ADebuffSo aDebuff)
           {
               aDebuff.MakeRandomDirection(transform.position);
           }
       }
   
       private void ResetDebuff()
       {
           if (!_debuffReady)
               return;
   
           _debuffReady = false;
           _debuffSo.ActiveDebuff();
   
       }
       #endregion
       #region Legacy
       /*
       #region Rotation
          private void SetRotations()
          {
              _gearRotationSo.CurrentMainGearRotation = _gearRotationSo.MainGearStartRotation;
              _gearRotationSo.CurrentSubGearClockwiseRotation = _gearRotationSo.SubGearStartClockwiseRotation;
              _gearRotationSo.CurrentSubGearCounterClockwiseRotation = _gearRotationSo.SubGearStartCounterClockwiseRotation;
              transform.eulerAngles = _gearRotationSo.CurrentMainGearRotation;
          }
          protected override void RotateGear()
          {
              if (_isClockwise)
              {
                  _gearRotationSo.CurrentMainGearRotation -= new Vector3(0, 0, _mainGearSpeed * Time.deltaTime * _efficiency);
                  if (_gearRotationSo.CurrentMainGearRotation.z < 0)
                  {
                      _gearRotationSo.CurrentMainGearRotation = new Vector3(0, 0, 360);
                  }
              }
              else
              {
                  _gearRotationSo.CurrentMainGearRotation += new Vector3(0, 0, _mainGearSpeed * Time.deltaTime * _efficiency);
                  if (_gearRotationSo.CurrentMainGearRotation.z > 360)
                  {
                      _gearRotationSo.CurrentMainGearRotation = Vector3.zero;
                  }
              }
   
              _gearRotationSo.CurrentSubGearClockwiseRotation -= new Vector3(0, 0, _subGearSpeed * Time.deltaTime * _efficiency);
              if (_gearRotationSo.CurrentSubGearClockwiseRotation.z < 0)
              {
                  _gearRotationSo.CurrentSubGearClockwiseRotation = new Vector3(0, 0, 360);
              }
              _gearRotationSo.CurrentSubGearCounterClockwiseRotation += new Vector3(0, 0, _subGearSpeed * Time.deltaTime * _efficiency);
              if (_gearRotationSo.CurrentSubGearCounterClockwiseRotation.z > 360)
              {
                  _gearRotationSo.CurrentSubGearCounterClockwiseRotation = Vector3.zero;
              }
   
   
              transform.eulerAngles = _gearRotationSo.CurrentMainGearRotation;
          }
          #endregion
       * /
       #endregion
   #if UNITY_EDITOR
       protected override void OnBindField()
       {
           base.OnBindField();
           StageMemory = GameObject.FindAnyObjectByType<StageMemory>();
           _statUI = GameObject.FindAnyObjectByType<MainGearStatUI>();
       }
   
   #endif
   }
   

*/
#endregion