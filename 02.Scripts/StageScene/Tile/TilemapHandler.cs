using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

public enum ETileState
{
    None = 0,
    GearBatched = 1,
    MainGearBatched = 2,
    MainGearPath = 3,
    MainGearBlocked = 4,
    BigGearPath = 5,
    BigGearBlocked = 6,
    SubGearBlocked=7,
}

public class TilemapHandler : BaseBehaviour
{
    [Header("Tiles")]
    [SerializeField] private Tilemap _tilemap;
    private ETileState[,] _tiles;
    private Vector2Int _offSet;
    [SerializeField] private Vector2Int _mainGearPos;
    private int[,] _tileDepth;

    [Header("Notification TileMap")]
    [SerializeField] private Tilemap _notificationTileMap;
    [SerializeField] private Tile _ableFeedbackTile;
    [SerializeField] private Tile _disableFeedbackTile;


    [Header("Wound")]
    [SerializeField] private Dictionary<Vector2Int, Wound> _woundsDic = new Dictionary<Vector2Int, Wound>();

    protected override void Initialize()
    {
        base.Initialize();
        ResetTiles();
        CalcTileDepth();
    }

    private void ResetTiles()
    {
        _tiles = new ETileState[_tilemap.size.x, _tilemap.size.y];
        _offSet = new Vector2Int(_tilemap.cellBounds.xMin, _tilemap.cellBounds.yMin);
        for (int x = 0; x < _tiles.GetLength(0); x++)
        {
            for (int y = 0; y < _tiles.GetLength(1); y++)
            {
                _tiles[x, y] = ETileState.None;
            }
        }
        SetMainGear();
    }

    private void CalcTileDepth()
    {
        _tileDepth = new int[_tilemap.size.x, _tilemap.size.y];
        Queue<Vector2Int> que = new Queue<Vector2Int>();
        bool[,] visited = new bool[_tileDepth.GetLength(0), _tileDepth.GetLength(1)];
        que.Enqueue(_mainGearPos);
        visited[_mainGearPos.x, _mainGearPos.y] = true;
        int depth = 0;
        _tileDepth[_mainGearPos.x, _mainGearPos.y] = depth;
        while (que.Count > 0)
        {
            int count = que.Count;
            depth++;
            for (int i = 0; i < count; i++)
            {
                var quePos = que.Dequeue();

                var list = SearchNearTiles(quePos);
                for (int j = 0; j < list.Count; j++)
                {
                    if (!visited[list[j].x, list[j].y])
                    {
                        _tileDepth[list[j].x, list[j].y] = depth;
                        visited[list[j].x, list[j].y] = true;
                        que.Enqueue(list[j]);
                    }
                }
            }
        }
    }
    public int GetCellDepth(Vector2Int pos, bool isOffSet)
    {
        if (isOffSet)
        {
            var offPos = pos - _offSet;
            return _tileDepth[offPos.x, offPos.y];
        }
        else
        {
            return _tileDepth[pos.x, pos.y];
        }
    }
    private void SetMainGear()
    {
        _mainGearPos -= _offSet;
        _tiles[_mainGearPos.x, _mainGearPos.y] = ETileState.MainGearBatched;


        for (int i = 0; i < Constants.TILE_VERTEX; i++)
        {
            int posX = _mainGearPos.x + Constants.DIRECTION_X[i];
            int posY = _mainGearPos.y + Constants.DIRECTION_Y[i];
            _tiles[posX, posY] = ETileState.MainGearBatched;
        }
        BatchBigGear(_mainGearPos, EGearType.Main);
    }

    private void BatchBigGear(Vector2Int pos, EGearType type)
    {
        for (int i = 0; i < Constants.TILE_VERTEX; i++)
        {
            int posX = pos.x + Constants.DIRECTION_X[i];
            int posY = pos.y + Constants.DIRECTION_Y[i];
            _tiles[posX, posY] = type == EGearType.Main ? ETileState.MainGearPath : ETileState.BigGearPath;
        }
        for (int i = 0; i < Constants.TILE_VERTEX; i++)
        {
            int posX = pos.x + Constants.DIRECTION_XY[i];
            int posY = pos.y + Constants.DIRECTION_YX[i];
            _tiles[posX, posY] = type == EGearType.Main ? ETileState.MainGearBlocked : ETileState.BigGearBlocked;
        }

    }

    public void DisplayFeedBack(bool isAble,Vector2Int mousePos)
    {
        ResetNotificationTile();
        mousePos += _offSet;
        Tile notifyTile;
        if (isAble)
        {
            notifyTile = _ableFeedbackTile;
        }
        else
        {
            notifyTile = _disableFeedbackTile;
        }
        _notificationTileMap.SetTile(new Vector3Int(mousePos.x, mousePos.y, 0), notifyTile);
    }

    public void ResetNotificationTile()
    {
        BoundsInt bounds = _notificationTileMap.cellBounds;
        for (int i = bounds.xMin; i < bounds.xMax; i++)
        {
            for (int j = bounds.yMin; j < bounds.yMax; j++)
            {
                _notificationTileMap.SetTile(new Vector3Int(i, j, 0), null);
            }
        }
    }


    public bool CheckIsClockwise(Vector2Int pos)
    {
        return (_mainGearPos.x + _mainGearPos.y) % 2 == (pos.x + pos.y) % 2;
    }

    public bool CheckTileMapRange(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < _tiles.GetLength(0) &&
           pos.y >= 0 && pos.y < _tiles.GetLength(1);
    }

    public bool CheckCanBatchGear(Vector2Int pos)
    {
        return _tiles[pos.x, pos.y] == ETileState.None;
    }

    public void BatchGear(Vector2Int pos)
    {
        _tiles[pos.x, pos.y] = ETileState.GearBatched;
        if (_woundsDic.ContainsKey(pos))
        {
            _woundsDic[pos].PutOnGear();
        }
    }

    public bool CheckBlockGear(Vector2Int pos)
    {
        return _tiles[pos.x, pos.y] == ETileState.SubGearBlocked;
    }

    public void BlockGear(Vector2Int pos,bool isBlock)
    {
        if(isBlock)_tiles[pos.x, pos.y] = ETileState.SubGearBlocked;
        else _tiles[pos.x, pos.y] = ETileState.GearBatched;
    }

    public bool CheckCanRemoveGear(Vector2Int pos)
    {
        return _tiles[pos.x, pos.y] == ETileState.GearBatched;
    }

    public void RemoveGear(Vector2Int pos)
    {
        _tiles[pos.x, pos.y] = ETileState.None;
        if (_woundsDic.ContainsKey(pos))
        {
            _woundsDic[pos].RemoveGear();
        }
    }

    public Vector2Int GetWorldToCellPoint(Vector2 pos)
    {
        return (Vector2Int)_tilemap.WorldToCell(pos) - _offSet;
    }

    public Vector2 GetCellToWorldPoint(Vector3Int pos)
    {
        return (Vector2)_tilemap.GetCellCenterWorld(pos + new Vector3Int(_offSet.x, _offSet.y, 0));
    }


    public List<Vector2Int> GetConnectedGearPositions()
    {
        List<Vector2Int> connectedGearList = new List<Vector2Int>();
        Queue<Vector2Int> que = new Queue<Vector2Int>();
        bool[,] visited = new bool[_tiles.GetLength(0), _tiles.GetLength(1)];
        que.Enqueue(_mainGearPos);
        visited[_mainGearPos.x, _mainGearPos.y] = true;

        while (que.Count > 0)
        {
            int count = que.Count;
            for (int i = 0; i < count; i++)
            {
                var quePos = que.Dequeue();

                var list = SearchNearGears(quePos);
                for (int j = 0; j < list.Count; j++)
                {
                    if (!visited[list[j].x, list[j].y])
                    {
                        visited[list[j].x, list[j].y] = true;
                        que.Enqueue(list[j]);
                        if (_tiles[list[j].x, list[j].y] == ETileState.GearBatched|| _tiles[list[j].x, list[j].y] == ETileState.SubGearBlocked)
                        {
                            connectedGearList.Add(list[j]);
                        }
                    }
                }
            }
        }
        return connectedGearList;
    }

    private List<Vector2Int> SearchNearGears(Vector2Int pos)
    {
        List<Vector2Int> gearList = new List<Vector2Int>();
        for (int i = 0; i < Constants.TILE_VERTEX; i++)
        {
            int posX = pos.x + Constants.DIRECTION_X[i];
            int posY = pos.y + Constants.DIRECTION_Y[i];
            if (posX < 0 || posY < 0 || posX >= _tiles.GetLength(0) || posY >= _tiles.GetLength(1))
                continue;
            if (_tiles[posX, posY] == ETileState.GearBatched || _tiles[posX, posY] == ETileState.BigGearPath || _tiles[posX, posY] == ETileState.MainGearPath|| _tiles[posX, posY] == ETileState.SubGearBlocked)
            {
                gearList.Add(new Vector2Int(posX, posY));
            }
        }
        return gearList;
    }
    private List<Vector2Int> SearchNearTiles(Vector2Int pos)
    {
        List<Vector2Int> list = new List<Vector2Int>();
        for (int i = 0; i < Constants.TILE_VERTEX; i++)
        {
            int posX = pos.x + Constants.DIRECTION_X[i];
            int posY = pos.y + Constants.DIRECTION_Y[i];
            if (posX < 0 || posY < 0 || posX >= _tiles.GetLength(0) || posY >= _tiles.GetLength(1))
                continue;
            list.Add(new Vector2Int(posX, posY));
        }
        return list;
    }

    public void BatchWound(Wound wound)
    {
        var pos = GetRandomEmptyPosition();
        wound.SetPosition(pos, GetCellToWorldPoint((Vector3Int)(pos)));
        wound.OnDestroy += RemoveWound;
        _woundsDic[pos] = wound;
    }

    private void RemoveWound(Vector2Int pos)
    {
        _woundsDic.Remove(pos);
    }
    public Vector2Int GetRandomEmptyPosition()
    {
        while (true)
        {
            int x = Random.Range(0, _tiles.GetLength(0));
            int y = Random.Range(0, _tiles.GetLength(1));

            if (_tiles[x, y] == ETileState.None && !_woundsDic.ContainsKey(new Vector2Int(x,y)))
            {
                return new Vector2Int(x, y);
            }
        }
    }

    #region Tutorial
    public void ReleaseBlocked(Vector2Int pos)
    {
        _tiles[pos.x,pos.y] = ETileState.None;
    }

    public ETileState TileType(Vector2Int pos)
    {
        return _tiles[pos.x,pos.y];
    }
    #endregion

#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _tilemap = GetComponent<Tilemap>();
        _mainGearPos = (Vector2Int)_tilemap.WorldToCell(GameObject.FindWithTag("MainGear").transform.position);
        _notificationTileMap = GameObject.Find("NotificationTilemap").GetComponent<Tilemap>();
        _ableFeedbackTile = FindObjectInAsset<Tile>("AbleTile", EDataType.asset);
        _disableFeedbackTile = FindObjectInAsset<Tile>("DisableTile", EDataType.asset);
    }
#endif
}
