using System;



[Serializable]
public class PlayerGameData
{

    #region Settings
    private ELanguage _language;

    public ELanguage Language
    {
        get { return _language; }
    }

    private float _sfxVolume;

    public float SfxVolume
    {
        get { return _sfxVolume; }
    }

    private float _bgmVolume;

    public float BgmVolume
    {
        get { return _bgmVolume; }
    }
    public void ChangeLanguage(ELanguage language)
    {
        _language = language;
        SaveData();
    }

    public void ResetData()
    {
        _nodeVisitedCount = new int[4, 8, 5];
        _nodeClearedCount = new int[4, 8, 5];
        _playCount = 0;
        SaveData();
    }

    public void ChangeVolume(float sfx, float bgm)
    {
        _sfxVolume = sfx;
        _bgmVolume = bgm;
        SaveData();
    }

    public void SaveData()
    {
        SaveLoadFile.SavePlayerData(this);
    }

    #endregion


    #region Play Count
    private int _playCount;
    private int[,,] _nodeVisitedCount;
    private int[,,] _nodeClearedCount;

    public void AddPlayCount()
    {
        _playCount++;
        if (IsFirstPlay())
        {
            GameManager.Instance.SetAchievement(EAchievement.ACHIEVEMENT_PLAY_FIRST);
        }
    }

    public bool IsFirstPlay()
    {
        return _playCount == 1;
    }
    public void AddVisitedCount(MapPositionInfo info)
    {
        _nodeVisitedCount[info.Stage, info.Depth, info.Height]++;
        SaveData();
    }

    public int GetRoundCount()
    {
        return _nodeClearedCount[3, 0, 0] + _nodeClearedCount[3, 0, 1];
    }
    public int GetVisitedCount(MapPositionInfo info)
    {
        return _nodeVisitedCount[info.Stage, info.Depth, info.Height];
    }
    public void AddClearCount(MapPositionInfo info)
    {
        _nodeClearedCount[info.Stage, info.Depth, info.Height]++;
        SaveData();
    }
    public int GetClearedCount(MapPositionInfo info)
    {
        return _nodeClearedCount[info.Stage, info.Depth, info.Height];
    }

    public bool GetIsNotShowEnding(bool isTrueEnding)
    {
        if (isTrueEnding) return _nodeClearedCount[3, 0, 0] > 1;  //0 true ending
        return _nodeClearedCount[3, 0, 1] >= 1;  //1 normal
    }

    public bool GetIsNotFirstCleared()
    {
        return (_nodeClearedCount[3, 0, 0] + _nodeClearedCount[3, 0, 1] >= 2);
    }

    public bool GetIsNotFirstBoss(DialogueType dialogueState, bool isTrueEnding)
    {
        if (dialogueState == DialogueType.StageBefore)
        {
            if (isTrueEnding) return (_nodeClearedCount[3, 0, 0] >= 1);
            else return (_nodeClearedCount[3, 0, 1] >= 1);
        }
        else
        {
            if (isTrueEnding) return (_nodeClearedCount[3, 0, 0] >= 2);
            else return (_nodeClearedCount[3, 0, 1] >= 2);
        }
    }

    public bool GetIsNotFirstEpic(DialogueType dialogueState,int actNum)
    {
        if (dialogueState == DialogueType.StageBefore)
        {
            return (_nodeClearedCount[actNum, 5, 0] >= 1);
        }
        else
        {
            return (_nodeClearedCount[actNum, 5, 0] > 1);
        }
    }

    public bool GetIsNotFirstEntered(MapPositionInfo positionInfo)
    {
        if (positionInfo.Stage == 0 && positionInfo.Depth == 1)
        {
            return (_nodeVisitedCount[0, 1, 0] + _nodeVisitedCount[0, 1, 1] + _nodeVisitedCount[0, 1, 2] > 1);
        }

        return false;
    }

    #endregion



    #region Achievement

    private int _gearAddCount;
    public int GearAddCount { get { return _gearAddCount; } }

    public void AddGearAddCount()
    {
        _gearAddCount++;
        GameManager.Instance.CheckAchievement(EAchievement.ACHIEVEMENT_GEAR_ADD);
    }
    private int _gearRemoveCount;
    public int GearRemoveCount { get { return _gearRemoveCount; } }

    public void AddGearRemoveCount()
    {
        _gearRemoveCount++;
        GameManager.Instance.CheckAchievement(EAchievement.ACHIEVEMENT_GEAR_REMOVE);
    }
    private int _gearUpgradeCount;
    public int GearUpgradeCount { get { return _gearUpgradeCount; } }

    public void AddGearUpgradeCount()
    {
        _gearUpgradeCount++;
        GameManager.Instance.CheckAchievement(EAchievement.ACHIEVEMENT_GEAR_UPGRADE);
    }
    private int _augmentAddCount;
    public int AugmentAddCount { get { return _augmentAddCount; } }

    public void AddAugmentAddCount()
    {
        _augmentAddCount++;
        GameManager.Instance.CheckAchievement(EAchievement.ACHIEVEMENT_AUGMENT_ADD);
    }
    private int _minusGearSupplyCount;
    public int MinusGearSupplyCount { get { return _minusGearSupplyCount; } }
    public void AddMinusGearSupplyCount()
    {
        _minusGearSupplyCount++;
        GameManager.Instance.CheckAchievement(EAchievement.ACHIEVEMENT_MINUSGEAR_SUPPLY);
    }
    #endregion
    public PlayerGameData()
    {

        _language = ELanguage.English;
        _sfxVolume = 1;
        _bgmVolume = 1;
        _playCount = 0;

        _nodeVisitedCount = new int[4, 8, 5];
        _nodeClearedCount = new int[4, 8, 5];

        _gearAddCount = 0;
        _gearRemoveCount = 0;
        _gearUpgradeCount = 0;
        _augmentAddCount = 0;
        _minusGearSupplyCount = 0;

    }






    // Todo Remove this after QA
    public void ChangeVisitedCount(MapPositionInfo info, int count)
    {
        _nodeVisitedCount[info.Stage, info.Depth, info.Height] = count;
        SaveData();

    }

    public void ChangeClearedCount(MapPositionInfo info, int count)
    {
        _nodeClearedCount[info.Stage, info.Depth, info.Height] = count;
        SaveData();
    }

    public void HardResetData()
    {
        _playCount = 0;
        _nodeVisitedCount = new int[4, 8, 5];
        _nodeClearedCount = new int[4, 8, 5];
        _gearAddCount = 0;
        _gearRemoveCount = 0;
        _gearUpgradeCount = 0;
        _augmentAddCount = 0;
        _minusGearSupplyCount = 0;
        SaveData();
    }
}


public enum ELanguage
{
    English, Korean
}
