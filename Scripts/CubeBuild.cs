using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class CubeBuild : MonoBehaviour
{
    [SerializeField]
    private Vector3 size = Vector3.one;

    [SerializeField]
    private int meshSize = 6;

    private List<Material> materialsList;

    Vector3 t0;
    Vector3 t1;
    Vector3 t2;
    Vector3 t3;

    Vector3 b0;
    Vector3 b1;
    Vector3 b2;
    Vector3 b3;

    // Update is called once per frame
    private void Start()
    {
        makeCube();
        cubeMesh();
        AddMaterials();
    }

    private void makeCube()
    {
        //top vertices
        t0 = new Vector3(size.x, size.y, -size.z); // top left point
        t1 = new Vector3(-size.x, size.y, -size.z); // top right point
        t2 = new Vector3(-size.x, size.y, size.z); //bottom right point of top square
        t3 = new Vector3(size.x, size.y, size.z); // bottom left point of top square

        //bottom vertices
        b0 = new Vector3(size.x, -size.y, -size.z); // bottom left point
        b1 = new Vector3(-size.x, -size.y, -size.z); // bottom right point
        b2 = new Vector3(-size.x, -size.y, size.z); //bottom right point of bottom square
        b3 = new Vector3(size.x, -size.y, size.z); // bottom left point of bottom square
    }

    private void cubeMesh()
    {
        //1. initialise MeshFilter
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        //2. initialise MeshBuilder
        helper meshBuilder = new helper(meshSize);

        //top square
        meshBuilder.BuildTriangle(t0, t1, t2, 0);
        meshBuilder.BuildTriangle(t0, t2, t3, 0);

        //bottom square
        meshBuilder.BuildTriangle(b2, b1, b0, 1);
        meshBuilder.BuildTriangle(b3, b2, b0, 1);


        //back square
        meshBuilder.BuildTriangle(b0, t1, t0, 2);
        meshBuilder.BuildTriangle(b0, b1, t1, 2);


        //left-side square
        meshBuilder.BuildTriangle(b1, t2, t1, 3);
        meshBuilder.BuildTriangle(b1, b2, t2, 3);


        //right-side square
        meshBuilder.BuildTriangle(b2, t3, t2, 4);
        meshBuilder.BuildTriangle(b2, b3, t3, 4);

        //front square
        meshBuilder.BuildTriangle(b3, t0, t3, 5);
        meshBuilder.BuildTriangle(b3, b0, t0, 5);


        //3. set mesh filter's mesh to the mesh generated from our mesh builder
        meshFilter.mesh = meshBuilder.CreateMesh();

    }

    private void AddMaterials()
    {
        materialsList = new List<Material>();

        for(int j = 0; j <= 6; j++)
        {
            Material redMat = new Material(Shader.Find("Specular"));
            redMat.color = Color.red;
            materialsList.Add(redMat);
        }
       
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = materialsList.ToArray();
    }
}
