using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(StageSceneEvents))]
public class StageSceneManager : BaseBehaviour
{
    public static StageSceneManager Instance;
    public StageSceneEvents EventStageScene;
    [SerializeField] private GearRotationSo _gearRotationSo;


    private float _timeRatio;
    public float TimeRatio { get { return _timeRatio; } }



    protected override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        _timeRatio = 1;
    }

    private void OnEnable()
    {
        EventStageScene.OnStageInitialize += Event_StageInitialize;
        EventStageScene.OnTimeRatioChanged += Event_TimeRatioChanged;
    }

    private void OnDisable()
    {
        EventStageScene.OnStageInitialize -= Event_StageInitialize;
        EventStageScene.OnTimeRatioChanged -= Event_TimeRatioChanged;
    }

    private void Event_StageInitialize(StageInfoSo stageInfoSo)
    {
        _gearRotationSo.InitializeGearRotationSo();
        if (stageInfoSo.StageType == EStageType.Normal)
        {
            //EventStageScene.CallStageStart();
            //SoundManager.Instance.PlayFadeOutAndInDelay(stageInfoSo.StageBgm, 2.5f);
        }
        else if (stageInfoSo.StageType == EStageType.Epic)
        {
            if (stageInfoSo is EpicStageInfoSo epicStage)
            {
                SoundManager.Instance.SetEpicStageBgms(epicStage);
                epicStage.Debuff.DebuffUiSo.LevelChanged = null;
                EventStageScene.CallActiveDebuff(epicStage.Debuff);
                //UIManager.Instance.OpenPopupUI<DialogTextBoxPopup>().StartInitialDialogue(DialogueType.StageBefore, MapManager.Instance.GetActNum());
            }
        }
    }
    private void Event_TimeRatioChanged(float ratio)
    {
        _timeRatio = ratio;
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        EventStageScene = GetComponent<StageSceneEvents>();
    }
#endif
}
