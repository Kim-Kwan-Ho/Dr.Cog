using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : BaseBehaviour
{
    public static UIManager Instance;
    private int _order = -20;
    private HashSet<UIPopup> _popupHash = new HashSet<UIPopup>();
    [SerializeField] private GameObject _popupCanvas;
    [SerializeField] Canvas _canvas;

    protected override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnEnable()
    {
        MySceneManager.Instance.EventSceneChanged.OnSceneChanged += Event_SceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        MySceneManager.Instance.EventSceneChanged.OnSceneChanged -= Event_SceneChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _canvas = transform.GetChild(0).GetComponent<Canvas>();

        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        
    }

    public int GetPopupStack()
    {
        return _popupHash.Count;
    }
    public T OpenPopupUI<T>(string name = null, Transform parent = null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Instantiate(Resources.Load<GameObject>($"Prefabs/UI/Popup/{name}"),_popupCanvas.transform);
        _popupHash.Add(go.GetComponent<UIPopup>());
        return go.GetComponent<T>();
    }


    public void ClosePopupUI(UIPopup popup)
    {
        if (_popupHash.Count == 0)
            return;

        if (!_popupHash.Contains(popup))
        {
            Debug.Log("Close Popup Failed!");
            return;
        }
        else
        {
            _popupHash.Remove(popup);
            Destroy(popup.gameObject);
        }

    }

    public void ClosePopupUI()
    {
        if (_popupHash.Count == 0)
            return;
        UIPopup popup = _popupHash.Last();
        if (popup != null)
        {
            ClosePopupUI(popup);
        }
        _popupHash.Remove(popup);
        popup = null;
        _order--;
    }
    public void CloseAllPopupUI()
    {
        while (_popupHash.Count > 0)
        {
            ClosePopupUI();
        }
    }
    public void ClosePopup(UIPopup popup)
    {
        Destroy(popup.gameObject);
    }

    private void Event_SceneChanged(SceneChangeEventArgs eventArgs)
    {
        CloseAllPopupUI();
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _popupCanvas = FindGameObjectInChildren("PopupCanvas");
    }
#endif
}


