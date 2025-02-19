using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class DebuffSo : ScriptableObject
{
    [Header("Debuffs")]
    protected bool _isEnabled;
    protected int _currentLevel;
    protected bool _hasStack;
    public bool HasStack { get { return _hasStack; } }
    [SerializeField] protected int[] _requireLevels;
    protected Action<bool>[] _levelActions;

    [Header("UI")]

    [SerializeField] private DebuffUISo _debuffUiSo;
    public DebuffUISo DebuffUiSo { get { return _debuffUiSo; } }
    public virtual void Update()
    {
        if (!_isEnabled)
            return;
    }

    protected void CheckLevel(int count)
    {
        for (int i = 0; i < _requireLevels.Length; i++)
        {
            if (count < _requireLevels[i])
            {
                SetLevel(i);
                return;
            }
        }
        SetLevel(_requireLevels.Length);
    }
    protected virtual void SetLevel(int level)
    {
        if (_currentLevel == level)
            return;

        _debuffUiSo.LevelChanged?.Invoke(level);
        _currentLevel = level;
        for (int i = 1; i <= level; i++)
        {
            _levelActions[i]?.Invoke(true);
        }

        for (int i = level + 1; i <= _requireLevels.Length; i++)
        {
            _levelActions[i]?.Invoke(false);
        }
    }

    public virtual int GetStackCount()
    {
        return 0;
    }
}
