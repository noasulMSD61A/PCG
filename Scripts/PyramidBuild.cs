using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PyramidBuild : MonoBehaviour
{
    [SerializeField]
    private float pyramidSize = 5f;
    private static int subMeshSize = 4;
    Vector3 top;
    Vector3 base0;
    Vector3 base2;
    Vector3 base1;
    helper meshBuilder = new helper(subMeshSize);


    // Update is called once per frame
    void Start()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        
        pointMaker();
        MeshPyramidMaker();
        MaterialsList();
        meshFilter.mesh = meshBuilder.CreateMesh();
    }

    private void MaterialsList()
    {
        List<Material> materialsList = new List<Material>();
        
        for (int j =0; j <= 3; j++)
        {
            Material greenMat = new Material(Shader.Find("Specular"));
            greenMat.color = Color.green;
            materialsList.Add(greenMat);
        }
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = materialsList.ToArray();
        
    }

    private void pointMaker()
    {
        top = new Vector3(0, pyramidSize, 0);
        base0 = Quaternion.AngleAxis(0f, Vector3.up) * Vector3.forward * pyramidSize;
        base1 = Quaternion.AngleAxis(240f, Vector3.up) * Vector3.forward * pyramidSize;
        base2 = Quaternion.AngleAxis(120f, Vector3.up) * Vector3.forward * pyramidSize;
    }

    private void MeshPyramidMaker()
    {
        meshBuilder.BuildTriangle(base0, base1, base2, 0);
        meshBuilder.BuildTriangle(base1, base0, top, 1);
        meshBuilder.BuildTriangle(base2, top, base0, 2);
        meshBuilder.BuildTriangle(top, base2, base1, 3);
    }
}
