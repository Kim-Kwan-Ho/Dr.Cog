using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSceneEvents : MonoBehaviour
{
    public Action<StageInfoSo> OnStageInitialize;
    public Action<StageSceneDebuffEventArgs> OnActiveDebuff;
    public Action OnStageStarted;
    public Action OnTimePaused;
    public Action OnTimeResumed;
    public Action OnActivateAnimation;
    public Action OnAnimationEnded;
    public Action OnRefresh;
    public Action OnStageFailed;
    public Action OnStageSucceed;
    public Action<float> OnTimeRatioChanged;

    public Action<StageSceneCheerEventArgs> OnStartCheer;
    public void CallStageInitialize(StageInfoSo stageInfo)
    {
        OnStageInitialize?.Invoke(stageInfo);
    }
    public void CallActiveDebuff(DebuffSo debuffSo)
    {
        OnActiveDebuff?.Invoke(new StageSceneDebuffEventArgs() { DebuffSo = debuffSo });
    }

    public void CallActivateAnimation()
    {
        OnActivateAnimation?.Invoke();
    }

    public void CallAnimationEnded()
    {
        OnAnimationEnded?.Invoke();
    }
    public void CallStageStart()
    {
        OnStageStarted?.Invoke();
        OnTimeResumed?.Invoke();
    }

    public void CallRefresh()
    {
        OnRefresh?.Invoke();
    }
    public void CallTimeResumed()
    {
        OnTimeResumed?.Invoke();
    }

    public void CallTimePaused()
    {
        OnTimePaused?.Invoke();
    }

    public void CallStageFailed()
    {
        OnTimePaused?.Invoke();
        OnStageFailed?.Invoke();
    }

    public void CallStageSucceed()
    {
        OnTimePaused?.Invoke();
        OnStageSucceed?.Invoke();
    }

    public void CallStartCheer(ECheerType type, int index)
    {
        OnStartCheer?.Invoke(new StageSceneCheerEventArgs() { Type = type, Index = index });
    }

    public void CallTimeRatioChanged(float ratio)
    {
        OnTimeRatioChanged?.Invoke(ratio);
    }
}

public class StageSceneDebuffEventArgs : EventArgs
{
    public DebuffSo DebuffSo;
}

public class StageSceneCheerEventArgs : EventArgs
{
    public ECheerType Type;
    public int Index;
}