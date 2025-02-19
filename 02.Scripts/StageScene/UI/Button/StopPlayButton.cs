using UnityEngine;
using UnityEngine.UI;




public enum EStopPlayState
{
    Stop = 0,
    Play = 1
}

public class StopPlayButton : BaseBehaviour
{
    private EStopPlayState _state;
    [SerializeField] private Button _stopPlayButton;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Sprite[] _stopPlaySprites;
    [SerializeField] private KeyCode _stopPlayKey = KeyCode.Space;
    protected override void Initialize()
    {
        base.Initialize();
        _state = EStopPlayState.Play;
        _buttonImage.sprite = _stopPlaySprites[(int)_state];
        _stopPlayButton.onClick.AddListener(StopAndPlay);
        _stageStarted = false;
        _stageEnded = false;
    }
    #region Event

    private void Update()
    {
        if (Input.GetKeyDown(_stopPlayKey))
        {
            StopAndPlay();
        }
    }
    private bool _stageStarted;
    private bool _stageEnded;
    private void OnEnable()
    {
        StageSceneManager.Instance.EventStageScene.OnStageStarted += Event_OnStageStarted;
        StageSceneManager.Instance.EventStageScene.OnStageFailed += Event_OnStageEnded;
        StageSceneManager.Instance.EventStageScene.OnStageSucceed += Event_OnStageEnded;
    }

    private void OnDisable()
    {
        StageSceneManager.Instance.EventStageScene.OnStageStarted -= Event_OnStageStarted;
        StageSceneManager.Instance.EventStageScene.OnStageFailed -= Event_OnStageEnded;
        StageSceneManager.Instance.EventStageScene.OnStageSucceed -= Event_OnStageEnded;
    }

    private void Event_OnStageStarted()
    {
        _stageStarted = true;
    }

    private void Event_OnStageEnded()
    {
        _stageEnded = true;
    }
    #endregion
    private void StopAndPlay()
    {
        SoundManager.Instance.PlaySfxSound(Sfx.SE_ButtonSelect);
        if (_stageEnded || !_stageStarted)
            return;

        StageSceneManager.Instance.EventStageScene.CallRefresh();
        if (_state == EStopPlayState.Play)
        {
            _state = EStopPlayState.Stop;
            StageSceneManager.Instance.EventStageScene.CallTimePaused();
        }
        else
        {
            _state = EStopPlayState.Play;
            StageSceneManager.Instance.EventStageScene.CallTimeResumed();
        }
        _buttonImage.sprite = _stopPlaySprites[(int)_state];
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _stopPlayButton = GetComponent<Button>();
        _buttonImage = FindGameObjectInChildren<Image>("ButtonImage");
    }
#endif
}
