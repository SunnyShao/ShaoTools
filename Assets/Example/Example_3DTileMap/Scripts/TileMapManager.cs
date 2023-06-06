using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public Tilemap curTimeMap;

    // Start is called before the first frame update
    void Start()
    {
        //清空全部Tile
        //curTimeMap.ClearAllTiles();

        Debug.LogError(curTimeMap.CellToLocal(new Vector3Int(-8, 11, 0)));
        Debug.LogError(curTimeMap.WorldToCell(new Vector3(-25.5f, 34.5f, 0f)));

        //根据格子坐标获取瓦片
        TileBase tile = curTimeMap.GetTile(new Vector3Int(-9, 11, 0));
        Debug.LogError(tile);

        //根据格子坐标设置瓦片
        curTimeMap.SetTile(new Vector3Int(-10, 11, 0), tile);
        
        //根据格子坐标删除瓦片
        //curTimeMap.SetTile(new Vector3Int(-9, 11, 0), null);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
