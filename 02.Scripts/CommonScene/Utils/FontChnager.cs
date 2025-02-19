using System.ComponentModel;
using TMPro;
using UnityEditor;
using UnityEngine;

public class FontChnager : BaseBehaviour
{
    [SerializeField] private TMP_FontAsset _font;
    [SerializeField] private TMP_FontAsset[] _fonts;
    [SerializeField] private int _fontIndex;
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab))
    //    {
    //        ChangeSceneFont();
    //    }

    //    if (Input.GetKeyDown(KeyCode.F1))
    //    {
    //        _fontIndex++;
    //        if (_fontIndex == _fonts.Length)
    //        {
    //            _fontIndex = 0;
    //        }
    //        _font = _fonts[_fontIndex];
    //    }

        
    //}

#if UNITY_EDITOR
    protected override void OnButtonField()
    {
        base.OnButtonField();
        ChangeSceneFont();
        //   ChangeAllAssetFont();
    }

    private void ChangeSceneFont()
    {
        var texts = FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (var text in texts)
        {
            text.font = _font;
        }
    }

    private void ChangeAllAssetFont()
    {
        var texts = FindObjectsInAsset<GameObject>("",EDataType.prefab);
        foreach (var text in texts)
        {
            var components = text.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var component in components)
            {
                component.font = _font;
            }
            PrefabUtility.SavePrefabAsset(text);
        }
    }
#endif
}
