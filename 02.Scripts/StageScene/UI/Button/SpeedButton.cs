using UnityEngine;
using UnityEngine.UI;



public enum ESpeedState
{
    Normal = 0,
    Double = 1,
    Triple = 2

}
public class SpeedButton : BaseBehaviour
{
    [SerializeField] private Button _speedButton;
    [SerializeField] private Image _speedImage;
    [SerializeField] private Sprite[] _speedSprites;
    private ESpeedState _speedState;
    protected override void Initialize()
    {
        base.Initialize();
        _speedState = ESpeedState.Normal;
        ChangeSprite();
        _speedButton.onClick.AddListener(ChangeSpeed);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeSpeed(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeSpeed(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeSpeed(2);
        }
    }

    private void ChangeSpeed(int index)
    {
        _speedState = (ESpeedState)index;
        ChangeSprite();
        StageSceneManager.Instance.EventStageScene.CallTimeRatioChanged((int)_speedState + 1);
    }
    private void ChangeSpeed()
    {
        SoundManager.Instance.PlaySfxSound(Sfx.SE_ButtonSelect);
        if (_speedState == ESpeedState.Normal)
        {
            _speedState = ESpeedState.Double;
        }
        else if (_speedState == ESpeedState.Double)
        {
            _speedState = ESpeedState.Triple;
        }
        else
        {
            _speedState = ESpeedState.Normal;
        }
        ChangeSprite();
        StageSceneManager.Instance.EventStageScene.CallTimeRatioChanged((int)_speedState + 1);
    }
    private void ChangeSprite()
    {
        _speedImage.sprite = _speedSprites[(int)_speedState];
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _speedButton = FindGameObjectInChildren<Button>("SpeedButton");
        _speedImage = FindGameObjectInChildren<Image>("SpeedImage");
    }
#endif



}
