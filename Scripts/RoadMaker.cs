using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class RoadMaker : MonoBehaviour
{
    [SerializeField]
    private float radius; // this defines the radius of the path

    [SerializeField]
    private float segments = 300f;

    [SerializeField]
    private float lineWidth = 0.3f; // middle white line road marker

    [SerializeField]
    private float roadWidth; // width of the road on each side of the line

    [SerializeField]
    private float edgeWidth = 1f; // widht of our road barrier at the edge of our road

    [SerializeField]
    private float edgeHeight = 1f;

    [SerializeField]
    private static int submeshSize = 6;

    [SerializeField]
    private float wavyness;

    [SerializeField]
    private float waveScale = 0.1f;

    [SerializeField]
    private Vector2 waveOffset;

    [SerializeField]
    private Vector2 waveStep;

    [SerializeField]
    private bool stripeCheck = true;

    [SerializeField]
    private GameObject car;

    private GameObject Plane;

    helper meshBuilder = new helper(submeshSize);

    Vector3 ranum;

    private List<Material> materialsList;

    void Start()
    {
        Plane = Resources.Load<GameObject>("startfinishline");
        
        AddMaterials();
        randomiseTrack();
        definePath();
        
    }

    void definePath()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        MeshCollider meshCollider = this.GetComponent<MeshCollider>();

        helper meshBuilder = new helper(submeshSize);


        //1. Divide the circular race track into segments denoted in degrees and each point is defined by each segment
        //   Create the points and store them in a list
        float segmentDegrees = 360f / segments;

        List<Vector3> points = new List<Vector3>();

        for (float degrees = 0; degrees < 360f; degrees += segmentDegrees)
        {
            Vector3 point = Quaternion.AngleAxis(degrees, Vector3.up) * Vector3.forward * radius;
            points.Add(point);
            ranum = points[Random.Range(0, points.Count)];
            
        }

        Vector2 wave = this.waveOffset;

        for (int i = 0; i < points.Count; i++)
        {
            wave += waveStep;

            Vector3 point = points[i];
            Vector3 centreDirection = point.normalized;

            float noise = Mathf.PerlinNoise(wave.x * waveScale, wave.y * waveScale);
            noise *= wavyness;

            float control = Mathf.PingPong(i, points.Count / 2f) / (points.Count / 2f);

            points[i] += centreDirection * noise * control;
        }

        //2. function to define the path - the path is defined by each segment
        for (int i = 1; i < points.Count + 1; i++)
        {
            Vector3 pPrev = points[i - 1];
            Vector3 pCurr = points[i % points.Count];
            Vector3 pNext = points[(i + 1) % points.Count];

            ExtrudeRoad(meshBuilder, pPrev, pCurr, pNext);
        }

        int rannum = Random.Range(0, points.Count - 1);
        
        car.transform.position = points[rannum];
        car.transform.LookAt(points[rannum+1]);
        Instantiate(Plane, car.transform.position + new Vector3(0f,0.01f,0f), car.transform.rotation * Quaternion.Euler(0f,90f,0f));
        

        meshFilter.mesh = meshBuilder.CreateMesh();

        meshCollider.sharedMesh = meshFilter.mesh;
    }

    //3. This method will used to create the different segments for each segment we are going to draw the road marker 
    //   (i.e. white line in the middle), draw the road on each side of the line, draw the edges - all these are going 
    //   to be placed in different positions
    private void ExtrudeRoad(helper meshBuilder, Vector3 pPrev, Vector3 pCurr, Vector3 pNext)
    {
        //Road Line
        Vector3 offset = Vector3.zero;
        Vector3 targetOffset = Vector3.forward * lineWidth;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, 0);

        //Road
        offset += targetOffset;
        targetOffset = Vector3.forward * roadWidth;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, 1);

        int stripeSubmesh = 2;

        if (stripeCheck)
        {
            stripeSubmesh = 3;
        }

        stripeCheck = !stripeCheck;

        //Edge
        offset += targetOffset;
        targetOffset = Vector3.up * edgeHeight;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

        //Edge Top
        offset += targetOffset;
        targetOffset = Vector3.forward * edgeWidth;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

        //Edge
        offset += targetOffset;
        targetOffset = -Vector3.up * edgeHeight;

        MakeRoadQuad(meshBuilder, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

    }

    //4. Create each quad
    private void MakeRoadQuad(helper meshBuilder, Vector3 pPrev, Vector3 pCurr, Vector3 pNext,
                              Vector3 offset, Vector3 targetOffset, int submesh)
    {
        Vector3 forward = (pNext - pCurr).normalized;
        Vector3 forwardPrev = (pCurr - pPrev).normalized;

        //Build Outer Track
        Quaternion perp = Quaternion.LookRotation(Vector3.Cross(forward, Vector3.up));
        Quaternion perpPrev = Quaternion.LookRotation(Vector3.Cross(forwardPrev, Vector3.up));

        Vector3 topLeft = pCurr + (perpPrev * offset);
        Vector3 topRight = pCurr + (perpPrev * (offset + targetOffset));

        Vector3 bottomLeft = pNext + (perp * offset);
        Vector3 bottomRight = pNext + (perp * (offset + targetOffset));

        meshBuilder.BuildTriangle(topLeft, topRight, bottomLeft, submesh);
        meshBuilder.BuildTriangle(topRight, bottomRight, bottomLeft, submesh);

        //Build Inner Track
        perp = Quaternion.LookRotation(Vector3.Cross(-forward, Vector3.up));
        perpPrev = Quaternion.LookRotation(Vector3.Cross(-forwardPrev, Vector3.up));

        topLeft = pCurr + (perpPrev * offset);
        topRight = pCurr + (perpPrev * (offset + targetOffset));

        bottomLeft = pNext + (perp * offset);
        bottomRight = pNext + (perp * (offset + targetOffset));

        meshBuilder.BuildTriangle(bottomLeft, bottomRight, topLeft, submesh);
        meshBuilder.BuildTriangle(bottomRight, topRight, topLeft, submesh);
    }

    void randomiseTrack()
    {
        radius = Random.Range(30f, 100f);
        waveStep = new Vector2(Random.Range(0.01f,0.09f), Random.Range(0.01f,0.09f));
        roadWidth = Random.Range(8f, 20f);
        wavyness = Random.Range(10f, 60f);
        waveOffset = new Vector2(Random.Range(0.01f, 0.09f), Random.Range(0.01f, 0.09f));
        waveScale= Random.Range(0.1f, 0.5f);

    }

    
    private void AddMaterials()
    {

        Material redMat = new Material(Shader.Find("Specular"));
        redMat.color = Color.red;

        Material greyMat = new Material(Shader.Find("Specular"));
        greyMat.color = Color.grey;

        Material whiteMat = new Material(Shader.Find("Specular"));
        whiteMat.color = Color.white;

        materialsList = new List<Material>();
        materialsList.Add(whiteMat);
        materialsList.Add(greyMat);
        materialsList.Add(redMat);
        materialsList.Add(whiteMat);

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = materialsList.ToArray();
    }

    private void LoadLevel()
    {
        
    }
}
