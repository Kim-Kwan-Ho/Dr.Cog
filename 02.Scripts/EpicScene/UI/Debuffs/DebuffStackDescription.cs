using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebuffStackDescription : BaseBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionText;
    public void SetText(string text)
    {
        _descriptionText.text = text;
    }

    public void HighlightText(bool active)
    {
        if (active)
        {
            _descriptionText.color = Color.red;
        }
        else
        {
            _descriptionText.color = Color.gray;
        }
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _descriptionText = FindGameObjectInChildren<TextMeshProUGUI>("DescriptionText");
    }
#endif
}
