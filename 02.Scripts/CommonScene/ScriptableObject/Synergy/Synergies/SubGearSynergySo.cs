using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubGearSynergySo : SynergySo
{
    public abstract void ApplySynergy(SubGear subGear);
    public abstract void RemoveSynergy(SubGear subGear);

}
