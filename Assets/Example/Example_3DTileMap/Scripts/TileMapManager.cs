using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : SingletonBehaviour<TileMapManager>
{
    public Tilemap curTileMap;

    public TileBase curTileBase;

    [SerializeField]
    private int height_Bounds = 43; //�߶ȱ߽�ֵ
    [SerializeField]
    private int height = 40;
    [SerializeField]
    private int width = 30;

    // Start is called before the first frame update
    void Start()
    {
        //���ȫ��Tile
        //curTileMap.ClearAllTiles();

        //Tile��ص�����ת������
        //Debug.LogError(curTileMap.CellToLocalInterpolated(new Vector3Int(-10, 12, 0) + new Vector3(0.5f,1f)));
        //Debug.LogError(curTileMap.CellToLocalInterpolated(new Vector3Int(-9, 12, 0) + new Vector3(0.5f, 1f)));
        //Debug.LogError(curTileMap.CellToLocalInterpolated(new Vector3Int(-8, 12, 0) + new Vector3(0.5f, 1f)));
        //Debug.LogError(curTileMap.WorldToCell(new Vector3(-25.5f, 34.5f, 0f)));

        //���ݸ��������ȡ��Ƭ
        //TileBase tile = curTileMap.GetTile(new Vector3Int(-10, 11, 0));
        //Debug.Log(tile);

        //���ݸ�������������Ƭ
        //curTileMap.SetTile(new Vector3Int(-10, 11, 0), tile);

        //���ݸ�������ɾ����Ƭ
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

    //�Ƿ����߽�
    public bool IsTouchBounds(Vector3Int targetCell)
    {
        if (0 <= targetCell.x && targetCell.x < width && 0 <= targetCell.y && targetCell.y < height_Bounds)
        {
            return false;
        }

        return true;
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