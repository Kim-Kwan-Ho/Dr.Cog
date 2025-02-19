using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QA_AugmentAddPopup : RewardPopup
{
    [SerializeField] private PlayerAugmentSo _playerAugmentSo;
    [SerializeField] private AugmentSo[] _augments;
    private HashSet<int> _selectedIndexHash;
    private List<RewardAugment> _augmentList;
    [SerializeField] private int _spawnCount;
    [SerializeField] private Button _refreshButton;

    [SerializeField] private Button _closeButton;
    [SerializeField] private List<AugmentSo> _rewardAugmentList;
    protected override void Initialize()
    {
        _confirmButton.interactable = true;
        _selectedIndexHash = new HashSet<int>();
        _confirmButton.onClick.AddListener(ConfirmReward);
        _augmentList = new List<RewardAugment>();
        CreateRewards();
        _refreshButton.onClick.AddListener(RefreshAugments);
        _closeButton.onClick.AddListener(ClosePopupUI);
    }

    public AugmentSo[] GetRandomAugment(int count)
    {
        _rewardAugmentList = new List<AugmentSo>();

        while (_rewardAugmentList.Count < count)
        {
            int r = Random.Range(0, _augments.Length);

            if (!_playerAugmentSo.AugmentList.Contains(_augments[r]) && (!_rewardAugmentList.Contains(_augments[r])))
            {
                _rewardAugmentList.Add(_augments[r]);
            }
        }

        return _rewardAugmentList.ToArray();
    }
    protected override void CreateRewards()
    {
        var augments = GetRandomAugment(_spawnCount);
        for (int i = 0; i < augments.Length; i++)
        {
            RewardAugment augment = Instantiate(_rewardGob, _rewardGroup.transform).GetComponent<RewardAugment>();
            augment.SetRewardAugment(i, augments[i]);
            augment.OnRewardClick += SelectReward;
            _augmentList.Add(augment);
        }
    }
    protected override void ConfirmReward()
    {
        foreach (var index in _selectedIndexHash)
        {
            _playerAugmentSo.AddPlayerAugment(_rewardAugmentList[index]);
            GameManager.GameLoadData.AddAugment(_rewardAugmentList[index]);
        }
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
    }

    private void RefreshAugments()
    {
        foreach (var augment in _augmentList)
        {
            Destroy(augment.gameObject);
        }
        _augmentList = new List<RewardAugment>();
        _selectedIndexHash = new HashSet<int>();
        CreateRewards();
    }

#if UNITY_EDITOR


    protected override void OnBindField()
    {
        base.OnBindField();
        _playerAugmentSo = FindObjectInAsset<PlayerAugmentSo>();
        _augments = FindObjectsInAsset<AugmentSo>().ToArray();
        _closeButton = FindGameObjectInChildren<Button>("CloseButton");

    }
#endif
}
