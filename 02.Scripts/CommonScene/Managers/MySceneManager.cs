using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using static LocaleManager;
using Object = UnityEngine.Object;

[RequireComponent(typeof(SceneChangeEvent))]
public class MySceneManager : BaseBehaviour
{
    public SceneChangeEvent EventSceneChanged;
    public static MySceneManager Instance;

    [Header("Fade")]
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeTimer = 1.5f;
    private object _sceneInitialize;
    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnEnable()
    {
        EventSceneChanged.OnSceneChanged += Event_OnSceneChanged;
        SceneManager.sceneLoaded += InitializeScene;
    }
    private void OnDisable()
    {
        EventSceneChanged.OnSceneChanged -= Event_OnSceneChanged;
        SceneManager.sceneLoaded -= InitializeScene;
    }



    private void Event_OnSceneChanged(SceneChangeEventArgs sceneChangeEventArgs)
    {
        _sceneInitialize = sceneChangeEventArgs.SceneInitialize;
        StartCoroutine(CoFadeOut(sceneChangeEventArgs));

        if (!CheckEpicOrBossStage(_sceneInitialize))
            SoundManager.Instance.PlayFadeOutBGM(_fadeTimer);
    }

    private void InitializeScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == Enum.GetName(typeof(ESceneName), ESceneName.LoadingScene))
            return;

        if (scene.name == Enum.GetName(typeof(ESceneName), ESceneName.StageScene) || scene.name == Enum.GetName(typeof(ESceneName), ESceneName.EpicStageScene))
        {
            StageInfoSo roomInfo = (StageInfoSo)_sceneInitialize;
            roomInfo = (StageInfoSo)_sceneInitialize;
            roomInfo.InitializeRoomInfoSo();
            StageSceneManager.Instance.EventStageScene.CallStageInitialize(roomInfo);
        }
        else if (scene.name == Enum.GetName(typeof(ESceneName), ESceneName.TutorialScene))
        {
            StageInfoSo tutorialInfo = (StageInfoSo)_sceneInitialize;
            tutorialInfo.InitializeRoomInfoSo();
            StageSceneManager.Instance.EventStageScene.CallStageInitialize(tutorialInfo);
        }
        else if (scene.name == Enum.GetName(typeof(ESceneName), ESceneName.WorldScene))
        {
            if (_sceneInitialize != null)
            {
                WorldSceneInfo worldInfo = (WorldSceneInfo)_sceneInitialize;
                if ((worldInfo.Act == 0 && worldInfo.Depth.Count > 1) || worldInfo.Act > 0)
                    MapManager.Instance.SetContinueMap(worldInfo);
                else
                {
                    MapManager.Instance.SettingAct();
                    SoundManager.Instance.PlayBgmSound(Bgm.WorldMap);
                }
            }
            else
            {
                MapManager.Instance.SetNewMap();
            }

        }
        _sceneInitialize = null;
    }

    private IEnumerator CoFadeIn()
    {
        if (!CheckEpicOrBossStage(_sceneInitialize))
            SoundManager.Instance.PlayFadeInBGM(_fadeTimer);

        float time = 0;
        Color targetColor = new Color(0, 0, 0, 0);
        Color curColor = _fadeImage.color;

        while (time < _fadeTimer)
        {
            _fadeImage.color = Color.Lerp(curColor, targetColor, time / _fadeTimer);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        _fadeImage.color = targetColor;
        StartScene();
        _fadeImage.gameObject.SetActive(false);
    }
    private void StartScene()
    {
        if (SceneManager.GetActiveScene().name == Enum.GetName(typeof(ESceneName), ESceneName.StageScene))
        {
            StageSceneManager.Instance.EventStageScene.CallActivateAnimation();
        }
        else if (SceneManager.GetActiveScene().name == Enum.GetName(typeof(ESceneName), ESceneName.WorldScene))
        {
            MapManager.Instance.EventWorldScene.CallWorldSceneEnter();
        }
        else if (SceneManager.GetActiveScene().name == Enum.GetName(typeof(ESceneName), ESceneName.EpicStageScene))
        {
            StageSceneManager.Instance.EventStageScene.CallActivateAnimation();
            //Epic Before Dialog를 불러오기
            /*if (MapManager.Instance.GetActNum() == 4)
            {
                UIManager.Instance.OpenPopupUI<DialogTextBoxPopup>().StartInitialDialogue(DialogueType.StageBefore, MapManager.Instance.GetActNum(),null, true, MapManager.Instance.IsTrueStage);
            }
            else
            {
                //StageSceneManager.Instance.EventStageScene.CallActivateAnimation();
                //UIManager.Instance.OpenPopupUI<DialogTextBoxPopup>().StartInitialDialogue(DialogueType.StageBefore, MapManager.Instance.GetActNum());
            }*/
        }
        else if(SceneManager.GetActiveScene().name == Enum.GetName(typeof(ESceneName), ESceneName.TutorialScene))
        {
            StageSceneManager.Instance.EventStageScene.CallActivateAnimation();
        }
    }
    private IEnumerator CoLoadScene(ESceneName sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(Enum.GetName(typeof(ESceneName), sceneName), LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync(Enum.GetName(typeof(ESceneName), ESceneName.LoadingScene));
        op.allowSceneActivation = true;
        StartCoroutine(CoFadeIn());
    }
    private IEnumerator CoFadeOut(SceneChangeEventArgs sceneChangeEventArgs = null)
    {
        _fadeImage.gameObject.SetActive(true);
        float time = 0;
        Color targetColor = new Color(0, 0, 0, 1);
        Color curColor = _fadeImage.color;

        while (time < _fadeTimer)
        {
            _fadeImage.color = Color.Lerp(curColor, targetColor, time / _fadeTimer);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        _fadeImage.color = targetColor;
        yield return StartCoroutine(CoUnloadScene(sceneChangeEventArgs));
    }
    private IEnumerator CoUnloadScene(SceneChangeEventArgs sceneChangeEventArgs)
    {
        SceneManager.LoadScene(Enum.GetName(typeof(ESceneName), ESceneName.LoadingScene));
        yield return StartCoroutine(CoLoadScene(sceneChangeEventArgs.SceneName));
    }

    private bool CheckEpicOrBossStage(object initialize)
    {
        if (initialize == null || initialize is not StageInfoSo stageInfoSo)
            return false;

        if (stageInfoSo.StageType == EStageType.Boss || stageInfoSo.StageType == EStageType.Epic)
        {
            if (stageInfoSo is EpicStageInfoSo epicStageInfoSo)
                SoundManager.Instance.SetEpicStageBgms(epicStageInfoSo);
            return true;
        }

        return false;
    }

    public bool CheckCurrentScene(ESceneName sceneType)
    {
        return SceneManager.GetActiveScene().name == Enum.GetName(typeof(ESceneName), sceneType);
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        EventSceneChanged = GetComponent<SceneChangeEvent>();
        _fadeImage = FindGameObjectInChildren<Image>("FadeImage");
    }
#endif
}