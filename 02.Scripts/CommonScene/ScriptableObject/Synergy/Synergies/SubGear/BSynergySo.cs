using UnityEngine;


[CreateAssetMenu(fileName = "BSynergy", menuName = "Scriptable Objects/Synergy/SubGear/BSynergy", order = 2)]
public class BSynergySo : SubGearSynergySo
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
        amount *= SynergyManager.Instance.GetSubGearSynergyCount(SynergyType);
        statIncreaseText.SetBSynergyText();
        return amount;
    }
}
