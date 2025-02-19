using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearRotater : BaseBehaviour
{
    [SerializeField] private GearRotationSo _gearRotationSo;
    [SerializeField] private bool _isMainGear;

    private bool _canRotate;
    private bool _isClockwise;


    protected override void Initialize()
    {
        base.Initialize();
        _canRotate = false;
    }

    public void SetRotateDirection(bool isClockwise)
    {
        _isClockwise = isClockwise;
    }

    private void Update()
    {
        RotateGear();
    }

    private void RotateGear()
    {
        if (!CanRotateGear())
            return;


        if (_isMainGear)
        {
            transform.eulerAngles = _gearRotationSo.CurrentMainGearRotation;
        }
        else
        {
            if (_isClockwise)
            {
                transform.eulerAngles = _gearRotationSo.CurrentSubGearClockwiseRotation;
            }
            else
            {
                transform.eulerAngles = _gearRotationSo.CurrentSubGearCounterClockwiseRotation;
            }
        }
    }

    public void StartRotating()
    {
        _canRotate = true;
    }

    public void StopRotating()
    {
        _canRotate = false;
    }

    private bool CanRotateGear()
    {
        return _canRotate;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _gearRotationSo = FindObjectInAsset<GearRotationSo>();
    }
#endif

}
