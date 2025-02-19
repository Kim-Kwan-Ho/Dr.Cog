using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SynergyLevelDescription : BaseBehaviour
{

    [SerializeField] private TextMeshProUGUI _levelDescriptionText;

    public void SetText(string text)
    {
        _levelDescriptionText.text = text;
    }


    public void HighlightText(Color color)
    {
        //_levelDescriptionText.color = Color.black;
        _levelDescriptionText.color = color;
    }

    public void DeHighlightText(Color color)
    {
        //_levelDescriptionText.color = Color.gray;
        _levelDescriptionText.color = color;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _levelDescriptionText = GetComponent<TextMeshProUGUI>();
    }
#endif
}
