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
        //���ȫ��Tile
        //curTimeMap.ClearAllTiles();

        //Tile��ص�����ת������
        //Debug.LogError(curTimeMap.CellToLocalInterpolated(new Vector3Int(-10, 12, 0) + new Vector3(0.5f,1f)));
        //Debug.LogError(curTimeMap.CellToLocalInterpolated(new Vector3Int(-9, 12, 0) + new Vector3(0.5f, 1f)));
        //Debug.LogError(curTimeMap.CellToLocalInterpolated(new Vector3Int(-8, 12, 0) + new Vector3(0.5f, 1f)));
        //Debug.LogError(curTimeMap.WorldToCell(new Vector3(-25.5f, 34.5f, 0f)));

        //���ݸ��������ȡ��Ƭ
        //TileBase tile = curTimeMap.GetTile(new Vector3Int(-10, 11, 0));
        //Debug.Log(tile);

        //���ݸ�������������Ƭ
        //curTimeMap.SetTile(new Vector3Int(-10, 11, 0), tile);

        //���ݸ�������ɾ����Ƭ
        //curTimeMap.SetTile(new Vector3Int(-9, 11, 0), null);
    }

    // ��õ�ǰ���������Ӧ�ĸ���У�������������
    public Vector3 GetWorldPosByWorldPos(Vector3 worldPos)
    {
        var cellPos = GetCellPosByWorldPos(worldPos);
        return GetWorldPosByCellPos(cellPos);
    }

    // ��õ�ǰ���������Ӧ�ĸ�������
    public Vector3Int GetCellPosByWorldPos(Vector3 worldPos)
    {
        return curTileMap.WorldToCell(worldPos);
    }

    // ��õ�ǰ���������Ӧ����������
    public Vector3 GetWorldPosByCellPos(Vector3Int cellPos)
    {
        return curTileMap.CellToWorld(cellPos);
    }

    // ��õ�ǰ���������Ӧ����������(�����������ƫ��)
    public Vector3 GetWorldPosByCellPosWithOffest(Vector3Int cellPos)
    {
        return curTileMap.CellToLocalInterpolated(cellPos + new Vector3(0.5f, 0.5f));
    }

    // ͨ�����������жϵ�ǰλ���Ƿ���ڸ�������
    public bool IsCellDataByCellPos(Vector3Int cellPos)
    {
        TileBase tile = curTileMap.GetTile(cellPos);
        return tile != null;
    }

    // ɾ������
    public void DeleteCell(Vector3Int cellPos)
    {
        if (curTileMap.GetTile(cellPos) != null)
            curTileMap.SetTile(cellPos, null);
    }
}