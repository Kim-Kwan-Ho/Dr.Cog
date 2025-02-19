

using UnityEngine;
using static LocaleManager;

public class StageReward : BaseBehaviour
{
    private EStageRewardType _rewardType;
    [SerializeField] private int _rewardCount;
    private MapPositionInfo _positionInfo;
    public void SetReward(EStageRewardType rewardType, MapPositionInfo positionInfo)
    {
        _rewardType = rewardType;
        _positionInfo = positionInfo;
    }




    private void OpenFailurePopup()
    {
        GameManager.GameLoadData.ResetGameLoadData();
        UIManager.Instance.OpenPopupUI<StageFailedPopup>();
    }


    #region Reward
    private void OpenRewardPopup()
    {
        GameManager.PlayerGameData.AddClearCount(_positionInfo);
        switch (_rewardType)
        {
            case EStageRewardType.GearAdd:
                UIManager.Instance.OpenPopupUI<GearAddRewardPopup>().SetRewardGears(GearDataManager.Instance.GetNonRepetitionGears(_rewardCount));
                break;
            case EStageRewardType.GearRemove:
                UIManager.Instance.OpenPopupUI<GearRemoveRewardPopup>().SetRewardGears(GearDataManager.Instance.GetPlayerGearData());
                break;
            case EStageRewardType.GearUpgrade:
                UIManager.Instance.OpenPopupUI<GearUpgradeRewardPopup>().SetRewardGears(GearDataManager.Instance.GetPlayerUpgradeGears());
                break;
            case EStageRewardType.Augment:
                UIManager.Instance.OpenPopupUI<AugmentRewardPopup>().SetRewardAugments(GetComponent<StageAugmentReward>().GetRandomAugment(_rewardCount));
                break;
            case EStageRewardType.Dialog:
                UIManager.Instance.OpenPopupUI<EpicUpgradeRewardPopup>().SetEpicUpgradeReward(GetComponent<StageEpicReward>().GetAllEpicUpgrade());
                break;
            case EStageRewardType.End:
                UIManager.Instance.OpenPopupUI<StageSucceedPopup>();
                break;
        }
    }

    #endregion



    #region Event

    private void OnEnable()
    {
        StageSceneManager.Instance.EventStageScene.OnStageSucceed += Event_StageSucceed;
        StageSceneManager.Instance.EventStageScene.OnStageFailed += Event_StageFailed;
    }

    private void OnDisable()
    {
        StageSceneManager.Instance.EventStageScene.OnStageSucceed -= Event_StageSucceed;
        StageSceneManager.Instance.EventStageScene.OnStageFailed -= Event_StageFailed;
    }

    private void Event_StageSucceed()
    {
        OpenRewardPopup();
    }

    private void Event_StageFailed()
    {
        OpenFailurePopup();
    }
    #endregion

}
