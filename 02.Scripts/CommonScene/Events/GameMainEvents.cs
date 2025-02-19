using System;
using UnityEngine;

public class GameMainEvents : MonoBehaviour
{
    public Action OnGameFailed;
    public Action OnGameCleared;
    public Action OnDataCleared;
    public Action<PlayerGameData> OnPlayerDataLoaded;
    public Action OnLanguageChanged;
    public void CallPlayerDataLoaded(PlayerGameData data)
    {
        OnPlayerDataLoaded?.Invoke(data);
    }

    public void CallDataClear()
    {
        OnDataCleared?.Invoke();
    }
    public void CallGameFailed()
    {
        CallDataClear();
        OnGameFailed?.Invoke();
    }
    public void CallGameClear()
    {
        CallDataClear();
        OnGameCleared?.Invoke();
    }

    public void CallLanguageChanged()
    {
        OnLanguageChanged?.Invoke();
    }
}
