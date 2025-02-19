using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryUI : BaseBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _memoryText;
    [SerializeField] private TextMeshProUGUI _goalMemoryText;
    [Header("MemoryAnimation")]
    [SerializeField] private Animation _memoryTextAnimation;

    public void SetGoalMemoryText(float amount)
    {
        _goalMemoryText.text = amount.ToString("F2");
    }

    public void UpdateMemoryText(float amount)
    {
        _memoryText.text = amount.ToString("F2");
    }
    public void UpdateMemoryTextAnimation(float amount)
    {
        _memoryText.text = amount.ToString("F2");
        _memoryTextAnimation.Play("MemoryUpdate");
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _memoryText = FindGameObjectInChildren<TextMeshProUGUI>("MemoryText");
        _goalMemoryText = FindGameObjectInChildren<TextMeshProUGUI>("GoalMemoryText");
    }
#endif
}

#region Legacy
/*
using System;
   using System.Collections;
   using System.Collections.Generic;
   using TMPro;
   using UnityEngine;
   using UnityEngine.UI;
   
   public class MemoryUI : BaseBehaviour
   {
       [SerializeField] private Slider _memorySlider;
       [SerializeField] private TextMeshProUGUI _memoryText;
       //[SerializeField] private TextMeshProUGUI _memoryDecreaseText;
       protected override void Initialize()
       {
           base.Initialize();
       }
   
       public void InitializeSlider(float startValue, float maxValue)
       {
           _memorySlider.minValue = 0;
           _memorySlider.value = startValue;
           _memorySlider.maxValue = maxValue;
       }
       public void UpdateMemorySlider(float value)
       {
           _memorySlider.value = value;
       }
       public void UpdateMemoryText(float amount)
       {
           _memoryText.text = amount.ToString("F1");
       }
   
       //public void UpdateMemoryDecreaseText(float amount)
       //{
       //    _memoryDecreaseText.text = "-" + amount + "/s";
       //}
   
   #if UNITY_EDITOR
       protected override void OnBindField()
       {
           base.OnBindField();
           _memorySlider = FindGameObjectInChildren<Slider>("MemorySlider");
           _memoryText = FindGameObjectInChildren<TextMeshProUGUI>("MemoryText");
           //_memoryDecreaseText = FindGameObjectInChildren<TextMeshProUGUI>("MemoryDecreaseText");
       }
   #endif
   }
   
*/
#endregion