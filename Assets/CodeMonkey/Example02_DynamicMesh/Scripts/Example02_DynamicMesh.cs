using UnityEngine;

public class Example02_DynamicMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //CreateBasicQuadMesh();

        CreateTileMesh();
    }

    private void CreateBasicQuadMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "CreateBasicQuadMesh";

        #region 绘制一个三角形
        //Vector3[] vertices = new Vector3[3];    //三个顶点
        //Vector2[] uv = new Vector2[3];          //三个UV
        //int[] triangles = new int[3];           //一个三角形，也是三个角

        //vertices[0] = new Vector3(0, 0);        //设置顶点的三个索引坐标
        //vertices[1] = new Vector3(0, 100);
        //vertices[2] = new Vector3(100, 100);

        //uv[0] = new Vector2(0, 0);              //设置UV的三个索引
        //uv[1] = new Vector2(0, 1);
        //uv[2] = new Vector2(1, 1);

        //triangles[0] = 0;                       //设置三角形三个角顺序。注意这里要按照顺时针，否则逆时针的话只会渲染背面了
        //triangles[1] = 1;
        //triangles[2] = 2;

        //mesh.vertices = vertices;
        //mesh.uv = uv;
        //mesh.triangles = triangles;
        #endregion

        #region 绘制四边形
        Vector3[] vertices = new Vector3[4];    //四边形肯定四个顶点了
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[3 * 2];       //这里需要两个三角形组成一个四边形，这里永远是3的倍数

        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(0, 100);
        vertices[2] = new Vector3(100, 100);
        vertices[3] = new Vector3(100, 0);

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        // 这里是第二个三角形 按照顺时针的方向排序的
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        #endregion

        GetComponent<MeshFilter>().mesh = mesh;
    }

    // 自定义多重网格
    private void CreateTileMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "CreateTileMesh";

        #region 绘制四边形
        int width = 4;          //想要绘制的宽
        int height = 4;         //想要绘制的高
        float tileSize = 10;    //格子大小
        Vector3[] vertices = new Vector3[4 * (width * height)];    //根据长宽绘制对应数量的四边形
        Vector2[] uv = new Vector2[4 * (width * height)];
        int[] triangles = new int[6 * (width * height)];          //根据长宽绘制对应数量的由两个三角形组成的四边形，这里永远是3的倍数

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int index = i * height + j;

                vertices[index * 4 + 0] = new Vector3(tileSize * i, tileSize * j);
                vertices[index * 4 + 1] = new Vector3(tileSize * i, tileSize * (j + 1));
                vertices[index * 4 + 2] = new Vector3(tileSize * (i + 1), tileSize * (j + 1));
                vertices[index * 4 + 3] = new Vector3(tileSize * (i + 1), tileSize * j);

                uv[index * 4 + 0] = new Vector2(0, 0);
                uv[index * 4 + 1] = new Vector2(0, 1);
                uv[index * 4 + 2] = new Vector2(1, 1);
                uv[index * 4 + 3] = new Vector2(1, 0);

                triangles[index * 6 + 0] = index * 4+ 0;
                triangles[index * 6 + 1] = index * 4 + 1;
                triangles[index * 6 + 2] = index * 4 + 2;

                // 这里是第二个三角形 按照顺时针的方向排序的
                triangles[index * 6 + 3] = index * 4 + 0;
                triangles[index * 6 + 4] = index * 4 + 2;
                triangles[index * 6 + 5] = index * 4 + 3;

            }
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        #endregion

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
