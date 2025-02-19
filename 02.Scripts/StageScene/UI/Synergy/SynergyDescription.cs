using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SynergyDescription : BaseBehaviour
{
    [SerializeField] private TextMeshProUGUI _synergyNameText;
    [SerializeField] private TextMeshProUGUI _synergyInfoText;

    [SerializeField] private GameObject _levelDescriptionPrefab;
    private List<SynergyLevelDescription> _levelDescriptionList;


    //Color
    [SerializeField] private Color _highlightColor;
    [SerializeField] private Color _deHighlightColor;


    protected override void Initialize()
    {
        base.Initialize();
        GetComponent<Canvas>().sortingLayerName = "SynergyInfo";

    }

    #region Description
    public void SetDescription(SynergyUISo synergyUISo, Vector2 offset, Vector3 scale)
    {

        SetSynergyInfo(synergyUISo);
        transform.localScale = scale;
        gameObject.SetActive(false);
    }
    public void SetDescription(SynergyUISo synergyUISo)
    {
        SetSynergyInfo(synergyUISo);
        gameObject.SetActive(false);
    }
    private void SetSynergyInfo(SynergyUISo synergyUISo)
    {
        _synergyNameText.text = synergyUISo.SynergyName;
        _synergyInfoText.text = synergyUISo.SynergyInfo;

        ClearLevelDescriptions();

        _levelDescriptionList = new List<SynergyLevelDescription>();
        for (int i = 0; i < synergyUISo.SynergyLvlInfo.Length; i++)
        {
            var description = Instantiate(_levelDescriptionPrefab, transform).GetComponent<SynergyLevelDescription>();
            description.SetText(synergyUISo.SynergyLvlInfo[i]);
            _levelDescriptionList.Add(description);
        }
    }
    private void ClearLevelDescriptions()
    {
        if (_levelDescriptionList != null)
        {
            for (int i = _levelDescriptionList.Count - 1; i >= 0; i--)
            {
                Destroy(_levelDescriptionList[i].gameObject);
                _levelDescriptionList.RemoveAt(i);
            }
        }
    }
    #endregion
    #region Highlight
    public void UpdateHighlight(int level)
    {
        DeHighligthTexts();
        if (level != 0)
        {
            //_synergyInfoText.color = _highlightColor;
            _levelDescriptionList[level - 1].HighlightText(_highlightColor);
        }
    }

    private void DeHighligthTexts()
    {
        foreach (var description in _levelDescriptionList)
        {
            //_synergyInfoText.color = _deHighlightColor;
            description.DeHighlightText(_deHighlightColor);
        }
    }


    #endregion



    #region SynergyEffect
    private bool _showEffect = false;
    private SynergySo _synergySo = null;
    private SynergyLevelDescription _effectDescription = null;
    private void Update()
    {
        if (_showEffect && _synergySo && _effectDescription)
        {
            UpdateSynergyEffect();
        }
    }

    public void ShowSynergyEffect(SynergySo synergySo)
    {
        var effect = synergySo.GetSynergyEffect();
        if (synergySo.SynergyUiSo.CurrentSynergyEffectInfo == null || effect == null)
            return;
        _effectDescription = Instantiate(_levelDescriptionPrefab, transform).GetComponent<SynergyLevelDescription>();
        _showEffect = true;
        _synergySo = synergySo;
    }

    public void UpdateSynergyEffect()
    {
        var effect = _synergySo.GetSynergyEffect();
        if (_synergySo.SynergyUiSo.CurrentSynergyEffectInfo == null || effect == null)
            return;

        if (_synergySo.Level > 0)
        {
            _effectDescription.HighlightText(Color.white);
        }
        else
        {
            _effectDescription.HighlightText(Color.grey);
        }
        _effectDescription.SetText(_synergySo.SynergyUiSo.CurrentSynergyEffectInfo + effect);
    }
    public void CloseSynergyEffect()
    {
        if (!(_showEffect || _synergySo || _effectDescription))
        { return; }
        Destroy(_effectDescription.gameObject);
        _showEffect = false;
        _synergySo = null;
    }


    #endregion

#if UNITY_EDITOR

    protected override void OnBindField()
    {
        base.OnBindField();
        _synergyNameText = FindGameObjectInChildren<TextMeshProUGUI>("SynergyNameText");
        _synergyInfoText = FindGameObjectInChildren<TextMeshProUGUI>("SynergyInfoText");
        _levelDescriptionPrefab = FindObjectInAsset<SynergyLevelDescription>().gameObject;

    }
#endif
}
