using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearPack : BaseBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TextMeshProUGUI _packNameText;
    [SerializeField] private TextMeshProUGUI _packEffectText;
    [SerializeField] private TextMeshProUGUI _synergyText;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private Transform _gearImages;
    [SerializeField] private Image _gearPackIcon;

    [Header("Prefab")]
    [SerializeField] private GameObject _gearImageGob;

    public event Action<int> OnPackClick;
    private int _index;


    public void SetGearPack(int index, GearPackSo gearPackSo)
    {
        _index = index;
        _toggle.isOn = false;
        _packNameText.text = gearPackSo.PackName;
        _packEffectText.text = gearPackSo.EffectInfo;
        _synergyText.text = gearPackSo.SynergyInfo;
        _infoText.text = gearPackSo.PackInfo;
        for (int i = 0; i < gearPackSo.GearIds.Length; i++)
        {
            Image image = Instantiate(_gearImageGob, _gearImages).GetComponent<Image>();
            GearImageSo imageSo = GearDataManager.Instance.GetGearData(gearPackSo.GearIds[i]).GearImage;
            image.sprite = imageSo.GearImage;
            image.color = imageSo.BodyColor;
        }
        _toggle.onValueChanged.AddListener((c) => OnPackClick?.Invoke(_index));
        _gearPackIcon.sprite = gearPackSo.GearPackIcon;
    }




#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _packNameText = FindGameObjectInChildren<TextMeshProUGUI>("PackNameText");
        _packEffectText = FindGameObjectInChildren<TextMeshProUGUI>("EffectText");
        _gearImages = FindGameObjectInChildren("GearImages").transform;
        _synergyText = FindGameObjectInChildren<TextMeshProUGUI>("SynergyText");
        _infoText = FindGameObjectInChildren<TextMeshProUGUI>("InfoText");
        _toggle = GetComponent<Toggle>();
        _gearPackIcon = FindGameObjectInChildren<Image>("GearPackIcon");
    }
#endif
}
