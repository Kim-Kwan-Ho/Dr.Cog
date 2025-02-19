using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffGroup : BaseBehaviour
{
    private DebuffSo _debuffSo;
    [SerializeField] private GameObject _debuffPanel;
    [SerializeField] private DebuffInfo _debuffInfo;
    [SerializeField] private DebuffStackInfo _stackInfo;
    private bool _hasStack;

    protected override void Initialize()
    {
        base.Initialize();
        _hasStack = false;
    }



    private void OnEnable()
    {
        StageSceneManager.Instance.EventStageScene.OnActiveDebuff += Event_ActiveDebuff;
    }

    private void OnDisable()
    {
        StageSceneManager.Instance.EventStageScene.OnActiveDebuff -= Event_ActiveDebuff;
    }

    private void Update()
    {
        UpdateStack();
    }

    private void UpdateStack()
    {
        if (!_hasStack)
            return;

        _stackInfo.UpdateStack(_debuffSo.GetStackCount());

    }
    private void Event_ActiveDebuff(StageSceneDebuffEventArgs eventArgs)
    {
        _debuffSo = eventArgs.DebuffSo;

        _hasStack = _debuffSo.HasStack;

        _debuffPanel.SetActive(true);
        _debuffInfo.SetDebuffInfo(_debuffSo.DebuffUiSo, _hasStack);
        if (_hasStack)
        {
            _debuffSo.DebuffUiSo.LevelChanged += _debuffInfo.LevelChanged;
            _stackInfo.SetStackInfo(_debuffSo.DebuffUiSo);
        }

        StartCoroutine(UpdateRect());
    }


    public IEnumerator UpdateRect()
    {
        foreach (var verticalLayoutGroup in GetComponentsInChildren<VerticalLayoutGroup>())
        {
            verticalLayoutGroup.enabled = false;
        }
        yield return new WaitForSeconds(0.1F);
        foreach (var verticalLayoutGroup in GetComponentsInChildren<VerticalLayoutGroup>())
        {
            verticalLayoutGroup.enabled = true;
        }

        foreach (var rectTransform in GetComponentsInChildren<RectTransform>())
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
        Canvas.ForceUpdateCanvases();

    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _debuffInfo = GetComponentInChildren<DebuffInfo>();
        _stackInfo = GetComponentInChildren<DebuffStackInfo>();
    }
#endif
}
