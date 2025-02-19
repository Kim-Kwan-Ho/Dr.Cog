using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageTimeUI : BaseBehaviour
{

    [Header("UI")]
    [SerializeField] private Slider _stageSlider;
    [SerializeField] private Image _stageSliderFillImage;
    [SerializeField] private TextMeshProUGUI _stageText;

    [SerializeField] private Color[] _timeStateColor;
    [SerializeField] private float[] _timeBoundary;
    private int _stateIndex = 0;

    public void InitializeSlider(float startValue, float maxValue)
    {
        _stageSlider.minValue = 0;
        _stageSlider.value = startValue;
        _stageSlider.maxValue = maxValue;
    }
    public void UpdateTime(float time)
    {
        _stageSlider.value = time;
        _stageText.text = time.ToString("F1");
        _stageSliderFillImage.color = Color.Lerp(_timeStateColor[0], _timeStateColor[2], 1 - time / _stageSlider.maxValue);


        //if (_stateIndex<_timeBoundary.Length)
        //{
        //    if (_stageSlider.value / _stageSlider.maxValue < _timeBoundary[_stateIndex])
        //    {
        //        UpdateSliderColor();
        //        _stateIndex++;
        //    }
        //}
    }

    private void UpdateSliderColor()
    {
        if (_stageSliderFillImage != null)
        {
            _stageSliderFillImage.color = _timeStateColor[_stateIndex];
            if (_stateIndex == _timeBoundary.Length - 1) _stageText.color = _timeStateColor[_stateIndex];
        }
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _stageSlider = FindGameObjectInChildren<Slider>("StageSlider");
        _stageSliderFillImage = _stageSlider.fillRect.GetComponent<Image>();
        _stageText = FindGameObjectInChildren<TextMeshProUGUI>("StageText");
    }
#endif
}
