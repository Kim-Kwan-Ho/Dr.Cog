using System;
using System.Collections.Generic;
using UnityEngine;
public class SceneChangeEvent : MonoBehaviour
{
    public Action<SceneChangeEventArgs> OnSceneChanged;
    public void CallSceneChange(ESceneName sceneName, object sceneInitialize)
    {
        OnSceneChanged?.Invoke(new SceneChangeEventArgs() { SceneName = sceneName, SceneInitialize = sceneInitialize });
    }
}

public class SceneChangeEventArgs : EventArgs
{
    public ESceneName SceneName;
    public object SceneInitialize;
}
