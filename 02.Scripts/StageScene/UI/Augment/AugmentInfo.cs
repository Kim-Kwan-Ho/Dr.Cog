using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AugmentInfo : BaseBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _iconImage;
    private AugmentSo _augmentSo;
    [SerializeField] private AugmentDescription _augmentDescription;

    public void SetAugmentInfo(AugmentSo augmentSo)
    {
        _augmentSo = augmentSo;
        _iconImage.sprite = _augmentSo.AugmentImage;
        _augmentDescription.SetAugmentDescriptionText(augmentSo.Description);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_augmentSo == null) 
            return;

        _augmentDescription.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_augmentSo == null) 
            return;
        _augmentDescription.gameObject.SetActive(false);
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _iconImage = FindGameObjectInChildren<Image>("IconImage");
        _augmentDescription = GetComponentInChildren<AugmentDescription>();
    }
#endif


}
