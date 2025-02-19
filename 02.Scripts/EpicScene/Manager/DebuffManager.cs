using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : BaseBehaviour
{
    private DebuffSo _debuffSo;
    private bool _timePaused;
    [SerializeField] private Transform _debuffCanvas;
    protected override void Initialize()
    {
        base.Initialize();
        _timePaused = true;
    }
    private void Update()
    {
        if (_timePaused)
            return;
        if (_debuffSo != null)
        {
            _debuffSo.Update();
        }
    }

    private void OnEnable()
    {
        StageSceneManager.Instance.EventStageScene.OnActiveDebuff += Event_ActiveDebuff;
        StageSceneManager.Instance.EventStageScene.OnTimePaused += Event_TimePaused;
        StageSceneManager.Instance.EventStageScene.OnTimeResumed += Event_TimeResumed;
        StageSceneManager.Instance.EventStageScene.OnStageSucceed += Event_StageSucceed;
    }

    private void OnDisable()
    {
        StageSceneManager.Instance.EventStageScene.OnActiveDebuff -= Event_ActiveDebuff;
        StageSceneManager.Instance.EventStageScene.OnTimePaused -= Event_TimePaused;
        StageSceneManager.Instance.EventStageScene.OnTimeResumed -= Event_TimeResumed;
        StageSceneManager.Instance.EventStageScene.OnStageSucceed -= Event_StageSucceed;
    }

    private void Event_TimePaused()
    {
        _timePaused = true;
    }

    private void Event_TimeResumed()
    {
        _timePaused = false;
    }

    private void Event_ActiveDebuff(StageSceneDebuffEventArgs eventArgs)
    {
        if (eventArgs.DebuffSo is WoundDebuffSo woundDebuff)
        {
            woundDebuff.InitializeWoundDebuffSo(FindAnyObjectByType<TilemapHandler>(), FindAnyObjectByType<MainGear>(), FindAnyObjectByType<SupplySystem>(), _debuffCanvas);
            _debuffSo = woundDebuff;
        }
        else if (eventArgs.DebuffSo is YearningDebuffSo yearningDebuff)
        {
            yearningDebuff.InitializeYearningDebuffSo(FindAnyObjectByType<SupplySystem>(), _debuffCanvas);
            _debuffSo = yearningDebuff;
        }
        else if (eventArgs.DebuffSo is DisarrayDebuffSo disarrayDebuff)
        {
            disarrayDebuff.InitializeDisarrayDebuffSo(FindAnyObjectByType<MainGear>(), _debuffCanvas);
            _debuffSo = disarrayDebuff;
        }
        else if (eventArgs.DebuffSo is PartingDebuffSo partingDebuff)
        {
            partingDebuff.InitializePartingDebuffSo(FindAnyObjectByType<GearController>(), FindAnyObjectByType<MainGear>(),
                FindAnyObjectByType<TilemapHandler>(), _debuffCanvas);
            _debuffSo = partingDebuff;
        }
    }

    private void Event_StageSucceed()
    {
        if (_debuffSo is WoundDebuffSo woundDebuff)
        {
            GameManager.Instance.SetAchievement(EAchievement.ACHIEVEMENT_TRUEBOSS_CLEAR);
            GameManager.GameLoadData.ResetGameLoadData();
        }
        else if (_debuffSo is YearningDebuffSo yearningDebuff)
        {
            GameManager.Instance.SetAchievement(EAchievement.ACHIEVEMENT_NORMALBOSS_CLEAR);
            GameManager.GameLoadData.ResetGameLoadData();
        }
        else if (_debuffSo is DisarrayDebuffSo disarrayDebuff)
        {
            GameManager.Instance.SetAchievement(EAchievement.ACHIEVEMENT_BOSS2_CLEAR);

        }
        else if (_debuffSo is PartingDebuffSo partingDebuff)
        {
            GameManager.Instance.SetAchievement(EAchievement.ACHIEVEMENT_BOSS3_CLEAR);
        }
        else
        {
            GameManager.Instance.SetAchievement(EAchievement.ACHIEVEMENT_BOSS1_CLEAR);
        }
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _debuffCanvas = GameObject.Find("DebuffCanvas").transform;
    }


#endif

}
