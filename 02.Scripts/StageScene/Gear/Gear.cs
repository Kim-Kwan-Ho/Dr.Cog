using UnityEngine;


public enum EGearType
{
    None = 0,
    Main = 1,
    Small = 2,
    Big = 3
}


public class Gear : BaseBehaviour
{
    [SerializeField] protected GearRotater _gearRotater;

    [SerializeField] protected StageMemory StageMemory;
    protected bool _isClockwise;
    public virtual void SetClockwise(bool isClockwise)
    {
        _isClockwise = isClockwise;
        _gearRotater.SetRotateDirection(_isClockwise);
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _gearRotater = GetComponentInChildren<GearRotater>();
    }
#endif
}
