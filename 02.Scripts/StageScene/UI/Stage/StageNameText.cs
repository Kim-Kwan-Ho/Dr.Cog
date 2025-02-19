using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageNameText : BaseBehaviour
{
    [SerializeField] private TextMeshProUGUI _stageNameText;
    private void OnEnable()
    {
        StageSceneManager.Instance.EventStageScene.OnStageInitialize += Event_StageInitialize;
    }

    private void OnDisable()
    {
        StageSceneManager.Instance.EventStageScene.OnStageInitialize -= Event_StageInitialize;
    }

    private void Event_StageInitialize(StageInfoSo stageInfoSo)
    {
        _stageNameText.text = stageInfoSo.StageName;
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _stageNameText = GetComponent<TextMeshProUGUI>();
    }
#endif

}
