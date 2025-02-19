using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EpicUpgradeSo : ScriptableObject
{
    [SerializeField] private int _index;
    public int Index { get { return _index; } }
    private string _name;
    public string Name { get { return _name; } }
    private string _description;
    public string Description { get { return _description; } }
    public abstract void ApplyEpicUpgrade();
}
