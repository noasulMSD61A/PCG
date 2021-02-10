using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helper : MonoBehaviour
{
    private List<Vector3> vertices = new List<Vector3>(); //list of vertices - store our points in our mesh

    private List<int> indices = new List<int>(); //list of indices that point to the index location in our vertices list

    private List<Vector3> normals = new List<Vector3>(); // this defines the direction of each vertex

    private List<Vector2> uvs = new List<Vector2>(); // store the coordinates of our uvs

    private List<int>[] submeshIndices = new List<int>[] { }; // an array of submesh indices

    public helper(int submeshCount)
    {
        submeshIndices = new List<int>[submeshCount];

        for (int i = 0; i < submeshCount; i++)
        {
            submeshIndices[i] = new List<int>();
        }

    }

    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, int subMesh)
    {
        Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized;
        BuildTriangle(p0, p1, p2, normal, subMesh);
    }

    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 normal, int subMesh)
    {
        for (int m = 0; m < 3; m++)
        {
            indices.Add(vertices.Count + m);
        }

        for (int m = 0; m < 3; m++)
        {
            submeshIndices[subMesh].Add(vertices.Count+m);
        }

        //adding each of the points to list of vertices
        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);

        //adding the normals through a for loop
        for (int k = 0; k < 3; k++)
        {
            normals.Add(normal);
        }

        //Adding UV coordinates to the UV list
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
    }

    public Mesh CreateMesh() //this method will build the mesh
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.subMeshCount = submeshIndices.Length;

        for (int i = 0; i < submeshIndices.Length; i++)
        {
            if (submeshIndices[i].Count < 3)
            {
                mesh.SetTriangles(new int[3] { 0, 0, 0 }, i);
            }
            else
            {
                mesh.SetTriangles(submeshIndices[i].ToArray(), i);
            }
        }

        return mesh;
    }

}
