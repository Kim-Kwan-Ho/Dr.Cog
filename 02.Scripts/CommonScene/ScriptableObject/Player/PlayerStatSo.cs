using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Scriptable Objects/Player/PlayerStat")]
public class PlayerStatSo : ScriptableObject
{
    [SerializeField] private PlayerStat _startPlayerStat;
    [SerializeField] private PlayerStat _currentPlayerStat;
    public PlayerStat CurrentPlayerStat { get { return _currentPlayerStat; } }

    public void InitializePlayerStatSo()
    {
        _currentPlayerStat = _startPlayerStat;
    }

    public void IncreaseStat(PlayerStat stat)
    {
        _currentPlayerStat += stat;
    }

    public void DecreaseStat(PlayerStat stat)
    {
        _currentPlayerStat -= stat;
    }

    public void SwapStat(PlayerStat stat)
    {
        _currentPlayerStat = stat;
    }
}

//todo
[Serializable]
public struct PlayerStat
{
    [SerializeField] private int _power;
    public int Power { get { return _power; } }
    [SerializeField] private float _statTime;
    public float StatIncreaseTime { get { return _statTime; } }
    [SerializeField] private float _attachedGearDecreaseAmount;
    public float AttachedGearDecreaseAmount { get { return _attachedGearDecreaseAmount; } }
    [SerializeField] private float _clockPlusStat;
    public float ClockPlusStat { get { return _clockPlusStat; } }
    [SerializeField] private float _counterPlusStat;
    public float CounterPlusStat { get { return _counterPlusStat; } }

    public PlayerStat(int power, float statTime, float attachedGearDecreaseAmount, float clockPlusStat,
        float counterPlusStat)
    {
        _power = power;
        _statTime = statTime;
        _attachedGearDecreaseAmount = attachedGearDecreaseAmount;
        _clockPlusStat = clockPlusStat;
        _counterPlusStat = counterPlusStat;
    }
    public static PlayerStat operator +(PlayerStat stat1, PlayerStat stat2)
    {
        stat1._power += stat2.Power;
        stat1._statTime += stat2.StatIncreaseTime;
        stat1._attachedGearDecreaseAmount += stat2.AttachedGearDecreaseAmount;
        stat1._clockPlusStat += stat2.ClockPlusStat;
        stat1._counterPlusStat += stat2.CounterPlusStat;
        return stat1;
    }
    public static PlayerStat operator -(PlayerStat stat1, PlayerStat stat2)
    {
        stat1._power -= stat2.Power;
        stat1._statTime -= stat2.StatIncreaseTime;
        stat1._attachedGearDecreaseAmount -= stat2.AttachedGearDecreaseAmount;
        stat1._clockPlusStat -= stat2.ClockPlusStat;
        stat1._counterPlusStat -= stat2.CounterPlusStat;
        return stat1;
    }
}
