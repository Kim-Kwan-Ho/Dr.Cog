using UnityEngine;

public abstract class AugmentSo : ScriptableObject
{
    [SerializeField] private int _index;
    public int Index { get { return _index; } }
    public Sprite AugmentImage;
    public string Name;
    public string Description;
    public abstract void ApplyAugment();
}
