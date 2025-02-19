using System;
using UnityEngine;
using Utils;

public static class ResolutionManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void StartScreen()
    {
        if (!PlayerPrefs.HasKey(Utils.Constants.SCREEN_INDEX))
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
            PlayerPrefs.SetInt(Constants.SCREEN_INDEX, 0);
        }
        else
        {
            int index = PlayerPrefs.GetInt(Utils.Constants.SCREEN_INDEX);
            if (index == 0)
            {
                Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
            }
            else
            {
                Screen.SetResolution(Constants.SCREEN_RESOLUTION[index].XSize, Constants.SCREEN_RESOLUTION[index].YSize, FullScreenMode.Windowed);
            }
        }
    }

}
