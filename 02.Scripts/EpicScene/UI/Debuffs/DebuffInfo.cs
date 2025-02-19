using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebuffInfo : BaseBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _debuffImage;
    [SerializeField] private DebuffDescription _debuffDescription;

    public void SetDebuffInfo(DebuffUISo debuffUiSo, bool hasStack)
    {
        _debuffImage.sprite = debuffUiSo.DebuffSprite;
        _debuffDescription.SetDebuffDescription(debuffUiSo, hasStack);
        gameObject.SetActive(true);
    }

    public void LevelChanged(int level)
    {
        _debuffDescription.SetTextHighlight(level);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _debuffDescription.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _debuffDescription.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _debuffImage = FindGameObjectInChildren<Image>("DebuffImage");
        _debuffDescription = GetComponentInChildren<DebuffDescription>();
    }
#endif
}