using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DisarrayDebuff", menuName = "Scriptable Objects/Debuffs/DisarrayDebuff")]
public class DisarrayDebuffSo : DebuffSo
{
    private MainGear _mainGear;
    private int _currentCount;
    [SerializeField] private int _targetMemoryCount;
    private float _additiveStatRatio;



    public void InitializeDisarrayDebuffSo(MainGear mainGear, Transform uiSpawnTrs)
    {
        _mainGear = mainGear;
        _currentCount = 0;
        _hasStack = true;
        _mainGear.OnMemoryIncreased -= IncreaseMemoryCount;
        _mainGear.OnMemoryIncreased += IncreaseMemoryCount;

    }


    private void IncreaseMemoryCount()
    {
        _currentCount++;

        if (_currentCount >= _targetMemoryCount)
        {
            _mainGear.OnGetDebuffStatRatio += GetDebuffStatRatio;
            _currentCount = 0;
        }
        else
        {
            _mainGear.OnGetDebuffStatRatio -= GetDebuffStatRatio;
        }
    }
    private float GetDebuffStatRatio()
    {
        SoundManager.Instance.PlaySfxSound(Sfx.SE_EpicStage2_Cancel);
        SoundManager.Instance.StopSfxSound(Sfx.SE_AddMemory);
        _mainGear.GetComponent<Animation>().Play("MainGearEpicStage2Animation");
        return 0;
    }

    public override int GetStackCount()
    {
        return _targetMemoryCount - _currentCount;
    }
}
