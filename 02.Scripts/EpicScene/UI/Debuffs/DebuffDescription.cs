using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebuffDescription : BaseBehaviour
{
    [SerializeField] private TextMeshProUGUI _debuffNameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    private DebuffStackDescription[] _stackDescriptions;
    [SerializeField] private Transform _descriptionGroupTrs;
    [SerializeField] private GameObject _descriptionPrefab;



    public void SetDebuffDescription(DebuffUISo debuffUiSo, bool hasStack)
    {
        _debuffNameText.text = debuffUiSo.DebuffName;
        _descriptionText.text = debuffUiSo.DebuffDescription;
        if (hasStack)
        {
            SetStackDescriptions(debuffUiSo.StackDescriptions);
        }
        gameObject.SetActive(false);
    }
    private void SetStackDescriptions(string[] descriptions)
    {
        _stackDescriptions = new DebuffStackDescription[descriptions.Length];
        for (int i = 0; i < descriptions.Length; i++)
        {
            _stackDescriptions[i] = Instantiate(_descriptionPrefab, transform).GetComponent<DebuffStackDescription>();
            _stackDescriptions[i].SetText(descriptions[i]);
        }

        var size = GetComponent<RectTransform>().rect.height / 2;
        transform.localPosition -= new Vector3(0, size, 0);

    }

    public void SetTextHighlight(int level)
    {
        DeactiveAllHighlight();

        for (int i = 0; i < _stackDescriptions.Length; i++)
        {
            if (i < level)
                _stackDescriptions[i].HighlightText(true);
        }
    }
    protected void DeactiveAllHighlight()
    {
        for (int i = 0; i < _stackDescriptions.Length; i++)
        {
            _stackDescriptions[i].HighlightText(false);
        }
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _debuffNameText = FindGameObjectInChildren<TextMeshProUGUI>("DebuffNameText");
        _descriptionText = FindGameObjectInChildren<TextMeshProUGUI>("DescriptionText");
        _descriptionGroupTrs = FindGameObjectInChildren<Transform>("DescriptionGroup");
        _descriptionPrefab = FindObjectInAsset<DebuffStackDescription>().gameObject;
    }
#endif



}
