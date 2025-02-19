using UnityEngine;

public abstract class SynergySo : ScriptableObject
{
    [Header("UI")]
    [SerializeField] private SynergyUISo _synergyUiSo;
    public SynergyUISo SynergyUiSo { get { return _synergyUiSo; } }



    protected bool _isActive;
    protected int _level;
    public int Level { get { return _level; } }

    [Header("Level")]
    [SerializeField] private SynergyLevelSo _synergyLevel;
    public SynergyLevelSo SynergyLevel { get { return _synergyLevel; } }
    [SerializeField] private ESynergyType _synergyType;
    public ESynergyType SynergyType { get { return _synergyType; } }


    public virtual void InitializeSynergySo()
    {
        _isActive = false;
        _level = 0;
    }
    public virtual void SetLevel(int count)
    {
        _level = CheckSynergyLevel(count);
        if (!_isActive)
        {
            if (_level > 0)
            {
                _isActive = true;
            }
        }
        else
        {
            if (_level == 0)
            {
                _isActive = false;
            }
        }
    }
    public int CheckSynergyLevel(int count)
    {
        for (int i = 0; i < _synergyLevel.RequireCount.Length; i++)
        {
            if (count < _synergyLevel.RequireCount[i])
            {
                return i;
            }
        }

        return _synergyLevel.RequireCount.Length;
    }

    public bool CheckIsMaxLevel(int level)
    {
        return level == _synergyLevel.RequireCount.Length;
    }

    public virtual int GetStack()
    {
        return 0;
    }

    public virtual string GetSynergyEffect()
    {
        return null;
    }

}


public interface ISynergyInventoryCombiner
{
    InventorySystem Inventory { get; set; }
    void SetInventory(InventorySystem inventory);
}
