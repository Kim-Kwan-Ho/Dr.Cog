using System;

public class MainGearStat
{
    private float _clockPlusStat;
    public float ClockPlusStat { get { return _clockPlusStat; } }
    private float _counterPlusStat;
    public float CounterPlusStat { get { return _counterPlusStat; } }

    private float _efficiency;
    public float Efficiency { get { return _efficiency; } }

    public MainGearStat(PlayerStatSo stat)
    {
        _clockPlusStat = stat.CurrentPlayerStat.ClockPlusStat;
        _counterPlusStat = stat.CurrentPlayerStat.CounterPlusStat;
        _efficiency = 1;
    }

    public void UpdateStat(bool isClockwise, float amount)
    {
        if (isClockwise)
        {
            _clockPlusStat += amount;
        }
        else
        {
            _counterPlusStat += amount;
        }
    }

    public void SetEfficiency(float amount)
    {
        _efficiency = amount;
        _efficiency = (float)Math.Round((double)_efficiency, 3);
    }


}
