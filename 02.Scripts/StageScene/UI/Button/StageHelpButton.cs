using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageHelpButton : BaseBehaviour
{
    [SerializeField] private Button _helpButton;
    protected override void Initialize()
    {
        base.Initialize();
        _helpButton.onClick.AddListener(OpenHelpPopup);
    }

    private void OpenHelpPopup()
    {
        UIManager.Instance.OpenPopupUI<OptionTutorialTextBoxPopup>();
    }




#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _helpButton = FindGameObjectInChildren<Button>("HelpButton");

    }
#endif



}
