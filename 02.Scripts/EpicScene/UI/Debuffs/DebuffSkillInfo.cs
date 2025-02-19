using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DebuffSkillInfo : BaseBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Image _iconImage;
    [SerializeField] private DebuffSkillDescription _debuffDescription;


    public void SetDebuffSkillInfo(DebuffSkillUISo debuffSkillUiSo)
    {
        _iconImage.sprite = debuffSkillUiSo.DebuffSkillSprite;
        _debuffDescription.SetDebuffDescriptionText(debuffSkillUiSo.DebuffSkillName, debuffSkillUiSo.DebuffSkillDescripiton);
        gameObject.SetActive(true);
    }

    public void ActiveSkill()
    {
        _iconImage.color = Color.white;
    }
    public void DeactiveSkill()
    {
        _iconImage.color = Color.grey;
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
        _iconImage = FindGameObjectInChildren<Image>("IconImage");
        _debuffDescription = GetComponentInChildren<DebuffSkillDescription>();
    }
#endif
}
