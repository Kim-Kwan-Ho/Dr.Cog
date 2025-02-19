using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GearClockStats : BaseBehaviour
{

    [Header("ClockStat")]
    [SerializeField] private TextMeshProUGUI _clockPlusStatText;
    [SerializeField] private TextMeshProUGUI _counterClockPlusStatText;

    [Header("StatAnimation")]
    [SerializeField] private Animation _clockPlusStatTextAnimation;
    [SerializeField] private Animation _counterClockPlusStatTextAnimation;



    public void SetClockStat(float plusStat)
    {
        _clockPlusStatText.text = plusStat.ToString();
        _clockPlusStatTextAnimation.Play("LeftStatAnimation");
    }

    public void SetCounterClockStat(float plusStat)
    {
        _counterClockPlusStatText.text = plusStat.ToString();
        _counterClockPlusStatTextAnimation.Play("RightStatAnimation");
    }
    //public void SetClockStatAnimation(float plusStat)
    //{
    //    _clockPlusStatText.text = plusStat.ToString();
    //    _clockPlusStatTextAnimation.Play("LeftStatAnimation");
    //}

    //public void SetCounterClockStatAnimation(float plusStat)
    //{
    //    _counterClockPlusStatText.text = plusStat.ToString();
    //    _counterClockPlusStatTextAnimation.Play("RightStatAnimation");
    //}


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _clockPlusStatText = FindGameObjectInChildren<TextMeshProUGUI>("ClockPlusStatText");
        _counterClockPlusStatText = FindGameObjectInChildren<TextMeshProUGUI>("CounterClockPlusStatText");

        _clockPlusStatTextAnimation = _clockPlusStatText.GetComponent<Animation>();
        _counterClockPlusStatTextAnimation = _counterClockPlusStatText.GetComponent<Animation>();

    }
#endif
}
