using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearDataEvents : MonoBehaviour
{
    public Action<GearAddedEventArgs> OnGearAdded;
    public Action OnPlayerDeckReset;
    public Action<GearRemovedEventArgs> OnGearRemoved;
    public Action<GearUpgradedEventArgs> OnGearUpgraded;
    public Action<GearLoadedEventArgs> OnGearLoaded;
    public void CallGearAdded(int[] gearIds)
    {
        OnGearAdded?.Invoke(new GearAddedEventArgs() { GearIds = gearIds });
    }
    public void CallGearRemoved(int[] gearIds)
    {
        OnGearRemoved?.Invoke(new GearRemovedEventArgs() { GearIds = gearIds });
    }

    public void CallGearUpgraded(int gearIds)
    {
        OnGearUpgraded?.Invoke(new GearUpgradedEventArgs() { GearId = gearIds });
    }
    public void CallPlayerDeckReset()
    {
        OnPlayerDeckReset?.Invoke();
    }

    public void CallGearLoaded(KeyValuePair<int, bool>[] playerGears)
    {
        OnGearLoaded?.Invoke(new GearLoadedEventArgs() { PlayerGears = playerGears });
    }

}


public class GearAddedEventArgs : EventArgs
{
    public int[] GearIds;
}

public class GearRemovedEventArgs : EventArgs
{
    public int[] GearIds;
}

public class GearUpgradedEventArgs : EventArgs
{
    public int GearId;
}

public class GearLoadedEventArgs : EventArgs
{
    public KeyValuePair<int, bool>[] PlayerGears;
}