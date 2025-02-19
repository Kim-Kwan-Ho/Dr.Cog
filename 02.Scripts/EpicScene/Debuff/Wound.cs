using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Wound : MonoBehaviour
{
    private float _lifeTime;
    public Action<Vector2Int> OnDestroy;
    private bool _isGearUpon;
    private Vector2Int _pos;
    [SerializeField] private Animator _woundStartAnimator;
    [SerializeField] private GameObject _destroyedIcon;

    public void SetWound(float lifeTime)
    {
        _isGearUpon = false;
        _lifeTime = lifeTime;
        SoundManager.Instance.PlaySfxSound(Sfx.SE_Wound);
    }

    public void SetPosition(Vector2Int cellPos, Vector3 worldPos)
    {
        transform.position = worldPos;
        _pos = cellPos;
    }


    private void Update()
    {
        if (_timePaused)
            return;
        if (_isGearUpon)
        {
            _lifeTime -= (Time.deltaTime * StageSceneManager.Instance.TimeRatio);
        }

        if (_lifeTime <= 0)
        {
            SpawnIcon();
            OnDestroy?.Invoke(_pos);
            Destroy(this.gameObject);
        }
    }

    private void SpawnIcon()
    {
        GearStatIncreaseText destroyedIcon = Instantiate(_destroyedIcon, transform.position, Quaternion.identity).GetComponent<GearStatIncreaseText>();
        destroyedIcon.SetWoundIcon();
    }
    public void PutOnGear()
    {
        _isGearUpon = true;
        _woundStartAnimator.SetBool("isGear", true);
    }

    public void RemoveGear()
    {
        _isGearUpon = false;
        _woundStartAnimator.SetBool("isGear", false);
    }


    #region Event

    private bool _timePaused = false;
    private void OnEnable()
    {
        StageSceneManager.Instance.EventStageScene.OnTimePaused += Event_TimePaused;
        StageSceneManager.Instance.EventStageScene.OnTimeResumed += Event_TimeResumed;
    }

    private void OnDisable()
    {
        StageSceneManager.Instance.EventStageScene.OnTimePaused -= Event_TimePaused;
        StageSceneManager.Instance.EventStageScene.OnTimeResumed -= Event_TimeResumed;
    }

    private void Event_TimePaused()
    {
        _timePaused = true;
    }

    private void Event_TimeResumed()
    {
        _timePaused = false;
    }

    #endregion
}
