using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebuffStackInfo : BaseBehaviour
{
    [SerializeField] private Image _stackImage;
    [SerializeField] private TextMeshProUGUI _stackText;
    public void SetStackInfo(DebuffUISo debuffUiSo)
    {
        _stackImage.sprite = debuffUiSo.StackSprite;
        gameObject.SetActive(true);
    }

    public void UpdateStack(int count)
    {
        _stackText.text = count.ToString();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _stackImage = FindGameObjectInChildren<Image>("StackImage");
        _stackText = FindGameObjectInChildren<TextMeshProUGUI>("StackText");
    }
#endif
}
