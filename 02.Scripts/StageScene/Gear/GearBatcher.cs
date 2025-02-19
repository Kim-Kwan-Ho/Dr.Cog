using UnityEngine;

public class GearBatcher : BaseBehaviour
{
    [SerializeField] private GearController _gearController;
    [SerializeField] private GameObject _subGearGob;
    [SerializeField] private TilemapHandler _tilemapHandler;
    [SerializeField] private InventorySystem _inventorySystem;
    [SerializeField] private Camera _camera;

    #region Event
    private bool _timePaused = true;
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


    public Vector2Int GetMousePositionToCell()
    {
        return _tilemapHandler.GetWorldToCellPoint(_camera.ScreenToWorldPoint((Input.mousePosition)));
    }

    public bool CheckCellClockWise()
    {
        var pos = GetMousePositionToCell();
        return _tilemapHandler.CheckIsClockwise(pos);
    }

    public bool CheckCanRemoveGear()
    {
        var pos = GetMousePositionToCell();

        if (!_tilemapHandler.CheckTileMapRange(pos)) return false;

        bool isAbleRemove = _tilemapHandler.CheckCanRemoveGear(pos);

        return isAbleRemove;
    }

    public bool CheckCanBatchGear()
    {
        var pos = GetMousePositionToCell();

        if (!_tilemapHandler.CheckTileMapRange(pos)) return false;

        bool isAbleBatch = _tilemapHandler.CheckCanBatchGear(pos);

        return isAbleBatch;
    }

    public void FeedBackCanBatchGear(GearContainer gearContainer)
    {
        _tilemapHandler.DisplayFeedBack(CheckCanBatchGear() || CheckCanMergeGear(gearContainer), GetMousePositionToCell());
    }

    public void DisableFeedBack()
    {
        _tilemapHandler.ResetNotificationTile();
    }


    public void BatchGear(GearContainer gearContainer)
    {
        GameObject gearGob = CreateGearGameObject(gearContainer);
        var pos = GetMousePositionToCell();
        SetGearTilePosition(gearGob, pos);
        _gearController.AddGear(pos, gearGob, _tilemapHandler.CheckIsClockwise(pos), gearContainer);
        SoundManager.Instance.PlaySfxSound(Sfx.SE_GearPut);
    }

    public void MoveGearStart(Vector2Int startPos,bool isBlock)
    {
        _tilemapHandler.BlockGear(startPos, isBlock);
    }

    public void RemoveGear(Vector2Int pos)
    {
        _tilemapHandler.RemoveGear(pos);
    }

    private void SetGearTilePosition(GameObject gob, Vector2Int pos)
    {
        Vector2 spawnPos = _tilemapHandler.GetCellToWorldPoint(new Vector3Int(pos.x, pos.y, 0));
        _tilemapHandler.BatchGear(pos);
        gob.transform.position = spawnPos;
    }
    public bool CheckCanMergeGear(GearContainer gearContainer)
    {
        var pos = GetMousePositionToCell();

        if (!_tilemapHandler.CheckTileMapRange(pos)) return false;
        if (_tilemapHandler.CheckBlockGear(pos)) return false;
        return _gearController.CheckCanMergeGear(pos, gearContainer);
    }

    public void MergeGear()
    {
        SoundManager.Instance.PlaySfxSound(Sfx.SE_GearMix);
        var pos = GetMousePositionToCell();
        _gearController.MergeGear(pos);
    }
    public void TryRemoveGear()
    {
        var pos = GetMousePositionToCell();

        if (!_tilemapHandler.CheckTileMapRange(pos) || !_inventorySystem.CanAddGear()) return;

        if (_tilemapHandler.CheckCanRemoveGear(pos))
        {
            SoundManager.Instance.PlaySfxSound(Sfx.SE_GearPull);
            _tilemapHandler.RemoveGear(pos);
            GearContainer container = new GearContainer();
            _gearController.RemoveGear(ref container, pos);
            _inventorySystem.AddGear(container);
        }
    }


    public void TryRemoveGearByPos(Vector2Int pos)
    {
        if (!_tilemapHandler.CheckTileMapRange(pos)) return;

        if (_tilemapHandler.CheckBlockGear(pos))
        {
            SoundManager.Instance.PlaySfxSound(Sfx.SE_GearPull);
            _tilemapHandler.RemoveGear(pos);
            GearContainer container = new GearContainer();
            _gearController.RemoveGear(ref container, pos);
        }
    }

    private GameObject CreateGearGameObject(GearContainer gearContainer)
    {
        SubGear subGear = Instantiate(_subGearGob).GetComponent<SubGear>();
        subGear.SetGearSprite(gearContainer.GearSo.GearImage, gearContainer.Level);
        return subGear.gameObject;
    }

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _camera = Camera.main;
        _subGearGob = FindObjectInAsset<GameObject>("SubGear", EDataType.prefab);
        _gearController = GetComponent<GearController>();
        _tilemapHandler = GameObject.FindAnyObjectByType<TilemapHandler>();
        _inventorySystem = GameObject.FindAnyObjectByType<InventorySystem>();
    }
#endif



}
