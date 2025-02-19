using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GearStatIncreaseText : BaseBehaviour
{
    [SerializeField] private TextMeshPro _statText;
    [SerializeField] private float _lifeTime = 1.0f;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _bSyenrgySprite;
    [SerializeField] private Sprite _dSynergySprite;
    [SerializeField] private Sprite _woundSprite;

    private void Update()
    {
        UpdateLifeTime();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position += new Vector3(0, _moveSpeed * Time.deltaTime, 0);
    }
    private void UpdateLifeTime()
    {
        _lifeTime -= (Time.deltaTime * StageSceneManager.Instance.TimeRatio);
        if (_lifeTime < 0)
        {
            Destroy(this.gameObject);
        }
    }


    public void SetText(float amount, bool critical = false)
    {
        //if (!critical)
        //{
        //    if (amount >= 0)
        //    {
        //        _statText.text = "<color=green>+" + amount.ToString("F1");
        //    }
        //    else
        //    {
        //        _statText.text = "<color=red>-" + Mathf.Abs(amount).ToString("F1");
        //    }

        //}
        //else
        //{
        //    _statText.text = "<color=orange> Critical \n " + amount.ToString("F1");
        //}
    }

    public void SetDSynergyText()
    {
        _spriteRenderer.gameObject.SetActive(true);
        _spriteRenderer.sprite = _dSynergySprite;
        _statText.text = "";
    }

    public void SetBSynergyText()
    {
        _spriteRenderer.gameObject.SetActive(true);
        _spriteRenderer.sprite = _bSyenrgySprite;
        _statText.text = "";
    }

    public void SetWoundIcon()
    {
        _spriteRenderer.gameObject.SetActive(true);
        _spriteRenderer.sprite = _woundSprite;
        _spriteRenderer.color = Color.green;
        var scale = _spriteRenderer.transform.localScale;
        _spriteRenderer.transform.localScale = scale / 4;
        _statText.text = "";
    }
    public void SetMainGearText(float amount, Vector3 position)
    {
        transform.position = position;
        if (amount >= 0)
        {
            _statText.text = "<color=green>+" + amount.ToString("F1");
        }
        else
        {
            _statText.text = "<color=red>-" + Mathf.Abs(amount).ToString("F1");
        }
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _statText = GetComponent<TextMeshPro>();
    }
#endif
}
