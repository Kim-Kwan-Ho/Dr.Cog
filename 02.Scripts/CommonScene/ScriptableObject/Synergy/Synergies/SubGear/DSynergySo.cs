using UnityEngine;


[CreateAssetMenu(fileName = "DSynergy", menuName = "Scriptable Objects/Synergy/SubGear/DSynergy", order = 4)]
public class DSynergySo : SubGearSynergySo
{
    [SerializeField] private GameObject _statIncreaseTextGob;
    [SerializeField] private float[] _increaseAmount;

    public override void ApplySynergy(SubGear subGear)
    {
        subGear.OnGetSubGearStats += GetSubGearSynergyStat;
    }

    public override void RemoveSynergy(SubGear subGear)
    {
        subGear.OnGetSubGearStats -= GetSubGearSynergyStat;
    }

    private float? GetSubGearSynergyStat(Vector3 position)
    {
        if (!_isActive || _level == 0)
            return null;

        float amount = _increaseAmount[_level];
        GearStatIncreaseText statIncreaseText = Instantiate(_statIncreaseTextGob, position, Quaternion.identity).GetComponent<GearStatIncreaseText>();
        statIncreaseText.SetDSynergyText();
        return amount;
    }
}