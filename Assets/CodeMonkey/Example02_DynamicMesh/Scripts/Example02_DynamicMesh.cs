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

        #region ����һ��������
        //Vector3[] vertices = new Vector3[3];    //��������
        //Vector2[] uv = new Vector2[3];          //����UV
        //int[] triangles = new int[3];           //һ�������Σ�Ҳ��������

        //vertices[0] = new Vector3(0, 0);        //���ö����������������
        //vertices[1] = new Vector3(0, 100);
        //vertices[2] = new Vector3(100, 100);

        //uv[0] = new Vector2(0, 0);              //����UV����������
        //uv[1] = new Vector2(0, 1);
        //uv[2] = new Vector2(1, 1);

        //triangles[0] = 0;                       //����������������˳��ע������Ҫ����˳ʱ�룬������ʱ��Ļ�ֻ����Ⱦ������
        //triangles[1] = 1;
        //triangles[2] = 2;

        //mesh.vertices = vertices;
        //mesh.uv = uv;
        //mesh.triangles = triangles;
        #endregion

        #region �����ı���
        Vector3[] vertices = new Vector3[4];    //�ı��ο϶��ĸ�������
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[3 * 2];       //������Ҫ�������������һ���ı��Σ�������Զ��3�ı���

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

        // �����ǵڶ��������� ����˳ʱ��ķ��������
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        #endregion

        GetComponent<MeshFilter>().mesh = mesh;
    }

    // �Զ����������
    private void CreateTileMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "CreateTileMesh";

        #region �����ı���
        int width = 4;          //��Ҫ���ƵĿ�
        int height = 4;         //��Ҫ���Ƶĸ�
        float tileSize = 10;    //���Ӵ�С
        Vector3[] vertices = new Vector3[4 * (width * height)];    //���ݳ�����ƶ�Ӧ�������ı���
        Vector2[] uv = new Vector2[4 * (width * height)];
        int[] triangles = new int[6 * (width * height)];          //���ݳ�����ƶ�Ӧ��������������������ɵ��ı��Σ�������Զ��3�ı���

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

                // �����ǵڶ��������� ����˳ʱ��ķ��������
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
