using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class TileMapManager : SingletonBehaviour<TileMapManager>
{
    public Tilemap curTileMap;

    // Start is called before the first frame update
    void Start()
    {
        //清空全部Tile
        //curTimeMap.ClearAllTiles();

        //Tile相关的坐标转换测试
        //Debug.LogError(curTimeMap.CellToLocalInterpolated(new Vector3Int(-10, 12, 0) + new Vector3(0.5f,1f)));
        //Debug.LogError(curTimeMap.CellToLocalInterpolated(new Vector3Int(-9, 12, 0) + new Vector3(0.5f, 1f)));
        //Debug.LogError(curTimeMap.CellToLocalInterpolated(new Vector3Int(-8, 12, 0) + new Vector3(0.5f, 1f)));
        //Debug.LogError(curTimeMap.WorldToCell(new Vector3(-25.5f, 34.5f, 0f)));

        //根据格子坐标获取瓦片
        //TileBase tile = curTimeMap.GetTile(new Vector3Int(-10, 11, 0));
        //Debug.Log(tile);

        //根据格子坐标设置瓦片
        //curTimeMap.SetTile(new Vector3Int(-10, 11, 0), tile);

        //根据格子坐标删除瓦片
        //curTimeMap.SetTile(new Vector3Int(-9, 11, 0), null);
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