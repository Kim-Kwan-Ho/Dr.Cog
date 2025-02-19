using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyStackUI : BaseBehaviour
{
    private SynergySo _synergySo;

    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _stackText;

    public void InitializeSynergyStackUi(SynergySo synergySo)
    {
        _iconImage.sprite = synergySo.SynergyUiSo.SynergyIcon;
        _synergySo = synergySo;
    }

    private void Update()
    {
        UpdateSynergyStack();
    }

    public void RefreshSynergy()
    {
        if (_synergySo.Level == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    private void UpdateSynergyStack()
    {
        _stackText.text = _synergySo.GetStack().ToString();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _iconImage = FindGameObjectInChildren<Image>("IconImage");
        _stackText = FindGameObjectInChildren<TextMeshProUGUI>("StackText");
    }
#endif
}
