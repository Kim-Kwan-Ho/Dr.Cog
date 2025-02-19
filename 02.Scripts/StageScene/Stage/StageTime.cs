using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTime : BaseBehaviour
{

    [Header("UI")]
    [SerializeField] private StageTimeUI _stageTimeUI;


    [Header("Time")]
    private float _time = 0;
    private bool _timePaused;
    protected override void Initialize()
    {
        base.Initialize();
        _timePaused = true;
    }
    public void SetTime(float time)
    {
        _time = time;
        _stageTimeUI.InitializeSlider(_time, time);
    }

    // 변경하는 경우 자식인 Tutorial_StageTime도 같이 변경해야 함
    protected virtual void Update()
    {
        UpdateStageTime();
        UpdateUI();
    }

    protected void UpdateStageTime()
    {
        if (!_timePaused)
        {
            _time -= (Time.deltaTime * StageSceneManager.Instance.TimeRatio);
            if (_time <= 0)
            {
                _timePaused = true;
                StageSceneManager.Instance.EventStageScene.CallStageFailed();
            }
        }
    }
    protected void UpdateUI()
    {
        _stageTimeUI.UpdateTime(_time);
    }

    #region Event

    private void OnEnable()
    {
        StageSceneManager.Instance.EventStageScene.OnTimePaused += Event_TimePaused;
        StageSceneManager.Instance.EventStageScene.OnTimeResumed += Event_TimeResumed;
    }

    private void OnDisable()
    {
        StageSceneManager.Instance.EventStageScene.OnTimePaused -= Event_TimePaused;
        StageSceneManager.Instance.EventStageScene.OnTimeResumed -= Event_TimeResumed;
    }

    private void Event_TimePaused()
    {
        _timePaused = true;
    }

    private void Event_TimeResumed()
    {
        _timePaused = false;
    }

    #endregion

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _stageTimeUI = GameObject.FindAnyObjectByType<StageTimeUI>();
    }
#endif
}
