
public abstract class MainGearSynergySo : SynergySo
{
    protected MainGear _mainGear;

    
    public abstract void ApplySynergy();
    public abstract void RemoveSynergy();

    public virtual void UpdateSynergy() { }

    public override void InitializeSynergySo()
    {
        base.InitializeSynergySo();
        _mainGear = null;
    }
    public void SetMainGear(MainGear mainGear)
    {
        _mainGear = mainGear;
    }
    public override void SetLevel(int count)
    {
      _level = CheckSynergyLevel(count);
;
        if (!_isActive)
        {
            if (_level > 0)
            {
                _isActive = true;
                ApplySynergy();
            }
        }
        else
        {
            if (_level == 0)
            {
                _isActive = false;
                RemoveSynergy();
            }
        }
    }
}
