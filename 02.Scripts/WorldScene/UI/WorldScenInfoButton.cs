using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldScenInfoButton : BaseBehaviour
{
    [SerializeField] private Button _infoButton;

    protected override void Awake()
    {
        base.Awake();
        _infoButton.onClick.AddListener(OpenInfoPopup);
    }

    private void OpenInfoPopup()
    {
        UIManager.Instance.OpenPopupUI<TutorialTextBoxPopup>();
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _infoButton = GetComponent<Button>();
    }
#endif

}
