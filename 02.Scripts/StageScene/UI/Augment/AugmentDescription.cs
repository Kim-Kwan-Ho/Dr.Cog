using TMPro;
using UnityEngine;

public class AugmentDescription : BaseBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionText;
    public void SetAugmentDescriptionText(string description)
    {
        GetComponent<Canvas>().sortingLayerName = "SynergyInfo";
        _descriptionText.text = description;
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _descriptionText = FindGameObjectInChildren<TextMeshProUGUI>("DescriptionText");
    }

#endif


}
