using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : SingletonBehaviour<TileMapManager>
{
    public Tilemap curTileMap;

    public TileBase curTileBase;

    [SerializeField]
    private int height_Bounds = 43; //高度边界值
    [SerializeField]
    private int height = 40;
    [SerializeField]
    private int width = 30;

    // Start is called before the first frame update
    void Start()
    {
        //清空全部Tile
        //curTileMap.ClearAllTiles();

        //Tile相关的坐标转换测试
        //Debug.LogError(curTileMap.CellToLocalInterpolated(new Vector3Int(-10, 12, 0) + new Vector3(0.5f,1f)));
        //Debug.LogError(curTileMap.CellToLocalInterpolated(new Vector3Int(-9, 12, 0) + new Vector3(0.5f, 1f)));
        //Debug.LogError(curTileMap.CellToLocalInterpolated(new Vector3Int(-8, 12, 0) + new Vector3(0.5f, 1f)));
        //Debug.LogError(curTileMap.WorldToCell(new Vector3(-25.5f, 34.5f, 0f)));

        //根据格子坐标获取瓦片
        //TileBase tile = curTileMap.GetTile(new Vector3Int(-10, 11, 0));
        //Debug.Log(tile);

        //根据格子坐标设置瓦片
        //curTileMap.SetTile(new Vector3Int(-10, 11, 0), tile);

        //根据格子坐标删除瓦片
        //curTileMap.SetTile(new Vector3Int(-9, 11, 0), null);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        curTileMap.ClearAllTiles();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                curTileMap.SetTile(new Vector3Int(i, j, 0), curTileBase);
            }
        }
    }

    //是否触碰边界
    public bool IsTouchBounds(Vector3Int targetCell)
    {
        if (0 <= targetCell.x && targetCell.x < width && 0 <= targetCell.y && targetCell.y < height_Bounds)
        {
            return false;
        }

        return true;
    }

    // 获得当前世界坐标对应的格子校正后的世界坐标
    public Vector3 GetWorldPosByWorldPos(Vector3 worldPos)
    {
        var cellPos = GetCellPosByWorldPos(worldPos);
        return GetWorldPosByCellPos(cellPos);
    }

    // 获得当前世界坐标对应的格子坐标
    public Vector3Int GetCellPosByWorldPos(Vector3 worldPos)
    {
        return curTileMap.WorldToCell(worldPos);
    }

    // 获得当前格子坐标对应的世界坐标
    public Vector3 GetWorldPosByCellPos(Vector3Int cellPos)
    {
        return curTileMap.CellToWorld(cellPos);
    }

    // 获得当前格子坐标对应的世界坐标(配合主角坐标偏移)
    public Vector3 GetWorldPosByCellPosWithOffest(Vector3Int cellPos)
    {
        return curTileMap.CellToLocalInterpolated(cellPos + new Vector3(0.5f, 0.5f));
    }

    // 通过格子坐标判断当前位置是否存在格子数据
    public bool IsCellDataByCellPos(Vector3Int cellPos)
    {
        TileBase tile = curTileMap.GetTile(cellPos);
        return tile != null;
    }

    // 删除格子
    public void DeleteCell(Vector3Int cellPos)
    {
        if (curTileMap.GetTile(cellPos) != null)
            curTileMap.SetTile(cellPos, null);
    }
}