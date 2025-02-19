using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QA_StageSucceedPopup : UIPopup
{
    [SerializeField] private Button _nextStageButton;


    protected override void Initialize()
    {
        base.Initialize();
        _nextStageButton.onClick.AddListener(GoNextStage);
    }

    private void GoNextStage()
    {
        MapManager.Instance.NextAct();
        MySceneManager.Instance.EventSceneChanged.CallSceneChange(ESceneName.WorldScene, null);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _nextStageButton = FindGameObjectInChildren<Button>("NextStageButton");
    }
#endif
}
