using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PlaneBuild : MonoBehaviour
{
    [SerializeField]
    private float cellSize = 1f;

    [SerializeField]
    private int width = 24;

    [SerializeField]
    private int height = 24;

    [SerializeField]
    private static int subMeshSize = 6;

    Vector3[,] points;

    helper meshBuilder = new helper(subMeshSize);

    // Update is called once per frame
    void Start()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        pointCreate();
        QuadriCreate();
        MaterialsList();
        meshFilter.mesh = meshBuilder.CreateMesh();   
    }


    private void MaterialsList()
    {
        List<Material> materialsList = new List<Material>();
        for (int j = 0; j<=6; j++)
        {
            Material blueMat = new Material(Shader.Find("Specular"));
            blueMat.color = Color.blue;
            materialsList.Add(blueMat);
        }
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = materialsList.ToArray();
    }

    private void QuadriCreate()
    {
        int submesh = 0;

        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                Vector3 br = points[x, y];
                Vector3 bl = points[x + 1, y];
                Vector3 tr = points[x, y + 1];
                Vector3 tl = points[x + 1, y + 1];

                //create 2 triangles that make up a quad
                meshBuilder.BuildTriangle(bl, tr, tl, submesh % subMeshSize);
                meshBuilder.BuildTriangle(bl, br, tr, submesh % subMeshSize);
            }

            submesh++;
        }
    }

    private void pointCreate()
    {
        //create points of our plane
        points = new Vector3[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                points[x, y] = new Vector3(cellSize * x, 0, cellSize * y);
            }
        }
    }
}
