using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESynergyType
{
    A = 0, B = 1, C = 2, D = 3, E = 4, F = 5, G = 6, H = 7, I = 8, J = 9, K = 10
}


[CreateAssetMenu(fileName = "SynergyLevel_", menuName = "Scriptable Objects/Synergy/SynergyLevelSo")]
public class SynergyLevelSo : ScriptableObject
{
    [SerializeField] private int[] _requireCount;
    public int[] RequireCount { get { return _requireCount; } }
}
