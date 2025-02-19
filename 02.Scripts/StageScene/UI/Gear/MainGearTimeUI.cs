using UnityEngine;
using UnityEngine.UI;
public class MainGearTimeUI : BaseBehaviour
{
    [SerializeField] private Image _timeImage;

    public void UpdateTimeImage(float amount)
    {
        _timeImage.fillAmount = amount;
    }
#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _timeImage = FindGameObjectInChildren<Image>("TimeImage");
    }
#endif
}
