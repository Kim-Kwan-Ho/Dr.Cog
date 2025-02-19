using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SynergyInfoUI : BaseBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _synergyNameText;

    [SerializeField] private SynergyDescription _synergyDescription;
    private bool _canShowOption;
    public void SetSynergyInfo(SynergyUISo synergyUiSo)
    {
        _iconImage.sprite = synergyUiSo.SynergyIcon;
        _synergyNameText.text = synergyUiSo.SynergyName;
        _synergyDescription.gameObject.SetActive(false);
        _canShowOption = false;
    }
    public void SetSynergyInfo(SynergyUISo synergyUiSo, Vector3 offSet, Vector3 scale)
    {
        _iconImage.sprite = synergyUiSo.SynergyIcon;
        _synergyNameText.text = synergyUiSo.SynergyName;
        _canShowOption = true;

        if (_canShowOption)
        {
            _synergyDescription.SetDescription(synergyUiSo);
            _synergyDescription.gameObject.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_canShowOption)
            return;
        _synergyDescription.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_canShowOption)
            return;
        _synergyDescription.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _iconImage = FindGameObjectInChildren<Image>("IconImage");
        _synergyNameText = FindGameObjectInChildren<TextMeshProUGUI>("SynergyNameText");
        _synergyDescription = GetComponentInChildren<SynergyDescription>();
    }
#endif


}
