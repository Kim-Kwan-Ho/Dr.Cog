using System.Collections.Generic;
using UnityEngine;

public class QA_GearSelectPopup : RewardPopup
{
    private GearSo[] _rewardGears;
    private HashSet<int> _selectedIndexHash;
    public void SetRewardGears(GearSo[] gears)
    {
        _selectedIndexHash = new HashSet<int>();
        _rewardGears = gears;
        CreateRewards();
        _normalGearInfo.gameObject.SetActive(true);
        _upgradeGearInfo.gameObject.SetActive(true);
        _confirmButton.interactable = true;

    }
    protected override void CreateRewards()
    {
        for (int i = 0; i < _rewardGears.Length; i++)
        {
            RewardPlayerGear gear = Instantiate(_rewardGob, _rewardGroup.transform).GetComponent<RewardPlayerGear>();
            gear.SetRewardCheckGear(i, _rewardGears[i]);
            gear.OnRewardClick += SelectReward;
        }
    }

    protected override void ConfirmReward()
    {
        List<int> gearIdList = new List<int>();

        foreach (var index in _selectedIndexHash)
        {
            gearIdList.Add(_rewardGears[index].GearID);
        }
        GearDataManager.Instance.EventGearData.CallGearAdded(gearIdList.ToArray());
        ClosePopupUI();
    }
    protected override void PassReward()
    {
        ClosePopupUI();
    }
    protected override void SelectReward(int index)
    {
        if (_selectedIndexHash.Contains(index))
        {
            _selectedIndexHash.Remove(index);
        }
        else
        {
            _selectedIndexHash.Add(index);
        }


            _normalGearInfo.SetGearInfos(_rewardGears[index], false, 0);
            _upgradeGearInfo.SetGearInfos(_rewardGears[index], true, 0);
    }

}
