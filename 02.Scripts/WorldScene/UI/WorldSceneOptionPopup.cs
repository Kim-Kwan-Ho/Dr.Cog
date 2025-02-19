using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldSceneOptionPopup : OptionPopup
{
    [SerializeField] private Button _titleButton;
    [Header("Localization")]
    [SerializeField] private Button _localizationKorButton;
    [SerializeField] private Button _localizationEnButton;

    protected override void Initialize()
    {
        base.Initialize();
        _titleButton.onClick.AddListener(ReturnToTitle);
        _localizationEnButton.onClick.AddListener(() => LocalizationButton((int)ELanguage.English));
        _localizationKorButton.onClick.AddListener(() => LocalizationButton((int)ELanguage.Korean));
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ClosePopupSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ClosePopupSceneLoaded;
    }

    private void ClosePopupSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SoundManager.Instance.PlaySfxSound(Sfx.SE_ButtonSelect);
        ClosePopupUI();
    }
    private void LocalizationButton(int languageNum)
    {
        SoundManager.Instance.PlaySfxSound(Sfx.SE_ButtonSelect);
        LocaleManager.Instance.ChangeLocale(languageNum);
        LocaleManager.Instance.SetCurrentLanguageNum(languageNum);
        GameManager.PlayerGameData.ChangeLanguage((ELanguage)languageNum);
    }

    private void ReturnToTitle()
    {
        SoundManager.Instance.PlaySfxSound(Sfx.SE_ButtonSelect);
        GameManager.Instance.EventGameMain.CallGameFailed();
        MySceneManager.Instance.EventSceneChanged.CallSceneChange(ESceneName.TitleScene, null);
        SoundManager.Instance.PlayBgmSound(Bgm.Title);
        ClosePopupUI();
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _titleButton = FindGameObjectInChildren<Button>("TitleButton");
        _localizationEnButton = FindGameObjectInChildren<Button>("EnglishButton");
        _localizationKorButton = FindGameObjectInChildren<Button>("KoreanButton");
    }
#endif
}