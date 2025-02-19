using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DebuffSkillDescription : BaseBehaviour
{
    [SerializeField] private TextMeshProUGUI _skillNameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    public void SetDebuffDescriptionText(string name, string description)
    {
        GetComponent<Canvas>().sortingLayerName = "SynergyInfo";
        _skillNameText.text = name;
        _descriptionText.text= description;
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _skillNameText  = FindGameObjectInChildren<TextMeshProUGUI>("SkillNameText");
        _descriptionText = FindGameObjectInChildren<TextMeshProUGUI>("DescriptionText");
    }

#endif
}
