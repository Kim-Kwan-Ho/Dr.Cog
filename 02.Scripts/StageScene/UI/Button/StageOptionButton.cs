using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StageOptionButton : BaseBehaviour
{
    [SerializeField] private Button _optionButton;
    [SerializeField] private KeyCode _optionKey = KeyCode.Escape;


    private UIPopup _optionPopup;
    protected override void Initialize()
    {
        base.Initialize();
        _optionPopup = null;
        _optionButton.onClick.AddListener(OpenOptionPopup);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_optionKey))
        {
            var dialoguePopup = FindAnyObjectByType<DialogTextBoxPopup>();
            if (dialoguePopup != null)
            {
                return;
            }
            var randomGearPopup = FindAnyObjectByType<RandomGearSelectPopup>();
            if (randomGearPopup != null)
            {
                return;
            }
            var synergyTablePopup = FindAnyObjectByType<SynergyTablePopup>();
           
            var infoPopup = FindAnyObjectByType<TutorialTextBoxPopup>();
            if (synergyTablePopup != null)
            {
                synergyTablePopup.ClosePopupUI();
            }
            else if (infoPopup != null)
            {
                infoPopup.CloseTutorialPopup();
            }
            else if (_optionPopup == null)
            {
                OpenOptionPopup();
            }
            else
            {
                CloseOptionPopup();
            }
        }
    }

    private void OpenOptionPopup()
    {

        SoundManager.Instance.PlaySfxSound(Sfx.SE_ButtonSelect);

        if (_optionPopup != null)
        {
            return;
        }

        
        if (MySceneManager.Instance.CheckCurrentScene(ESceneName.StageScene) ||
            MySceneManager.Instance.CheckCurrentScene(ESceneName.EpicStageScene))
        {
            StageSceneManager.Instance.EventStageScene.CallRefresh();
            _optionPopup = UIManager.Instance.OpenPopupUI<InGameOptionPopup>();
        }
        else if (MySceneManager.Instance.CheckCurrentScene(ESceneName.WorldScene))
        {
            _optionPopup = UIManager.Instance.OpenPopupUI<WorldSceneOptionPopup>();
        }
        


    }

    private void CloseOptionPopup()
    {
        SoundManager.Instance.PlaySfxSound(Sfx.SE_ButtonSelect);
        _optionPopup.ClosePopupUI();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _optionButton = GetComponent<Button>();
    }
#endif

}
