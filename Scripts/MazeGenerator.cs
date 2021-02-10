using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private GameObject cube;
    private GameObject Longcube;
    private GameObject pyramid;
    private GameObject player;
    private GameObject Plane;
    private GameObject Plane2;
    private GameObject borders;
    private GameObject shortPlane;
    Vector3 position;
    Vector3 position2;
    Vector3 position3;
    Vector3 position4;
    private bool spawned = false;
    private bool spawned2 = false;
    private bool spawned3 = false;
    private bool spawned4 = false;
    private bool canspawn = false;
    // Start is called before the first frame update
    void Start()
    {
        cube = Resources.Load<GameObject>("Cube");
        pyramid = Resources.Load<GameObject>("Pyramid");
        Plane = Resources.Load<GameObject>("Plane");
        Plane2 = Resources.Load<GameObject>("Plane");
        Longcube = Resources.Load<GameObject>("LongCube");
        player = Resources.Load<GameObject>("FPScontrollerProper");
        borders = Resources.Load<GameObject>("BorderWalls");

        spawnsmallCube();
        spawnlargeCube();
        spawnStartingPoint();
        spawnEndingPoint();
        spawnBorders();
        
    }
    void Update()
    {

        spawnPyramid();
        float dist = Vector3.Distance(Plane.transform.position, Plane2.transform.position);
        

    }

    void spawnBorders()
    {
        Instantiate(borders, new Vector3(50f, 0f, -1f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(borders, new Vector3(-50f, 0f, -1f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(borders, new Vector3(0f, 0f, -50f), Quaternion.identity);
        Instantiate(borders, new Vector3(0f, 0f, 50f), Quaternion.identity);

    }

    void spawnStartingPoint()
    {
        position2 = new Vector3(Random.Range(38.5F, 47.7F), 0.2f, Random.Range(-40.0F, 40.0F));
        Instantiate(Plane, position2, Quaternion.identity);
        Instantiate(player, position2, Quaternion.identity);
    }

    void spawnEndingPoint()
    {   
        
            position2 = new Vector3(Random.Range(-37.4F, -48F), 0.2f, Random.Range(-40.0F, 40.0F));
            Instantiate(Plane2, position2, Quaternion.identity);
        
        
    }

    void spawnPyramid()
    {
        if (spawned2 == false)
        {
            for (int i = 0; i < 20; i++)
            {
                position2 = new Vector3(Random.Range(-49.0F, 49.0F), 0, Random.Range(-49.0F, 49.0F));
                Instantiate(pyramid, position2, Quaternion.identity);
            }
        }
        spawned2 = true;
    }

    void spawnsmallCube()
    {
        Instantiate(cube, new Vector3(3f,0f,3f) , Quaternion.identity);
        Instantiate(cube, new Vector3(-3f, 0f, -3f), Quaternion.identity);
        Instantiate(cube, new Vector3(7.1f, 0f, -5.9f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(-6f, 0f, 7.7f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(-12.5f, 0f, -4.8f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(13.5f, 0f, 8.2f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(-22.3f, 0f, 3.4f), Quaternion.identity);
        Instantiate(cube, new Vector3(-7.1f, 0f, 17.6f), Quaternion.identity);
        Instantiate(cube, new Vector3(-33.5f, 0f, -5.9f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(-20.1f, 0f, 15.5f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(2.9f, 0f, 27.7f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(33.4f, 0f, 20f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(33.7f, 0f, 3f), Quaternion.identity);
        Instantiate(cube, new Vector3(7.1f, 0f, -25.2f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(-0.2f, 0f, -39.2f), Quaternion.identity);
        Instantiate(cube, new Vector3(33.1f, 0f, -9.2f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(38.5f, 0f, -21.1f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(28.2f, 0f, -37.9f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(11.6f, 0f, 39.3f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(20.8f, 0f, 34.7f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(30.2f, 0f, 42.2f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(cube, new Vector3(40.1f, 0f, 35.1f), Quaternion.Euler(new Vector3(0, 90, 0)));
    }

    void spawnlargeCube()
    {
        Instantiate(Longcube, new Vector3(21.8f, 0f, -11.6f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(Longcube, new Vector3(7.5f, 0f, -20.3f), Quaternion.identity);
        Instantiate(Longcube, new Vector3(21f, 0f, 22.7f), Quaternion.identity);
        Instantiate(Longcube, new Vector3(-22.6f, 0f, -19.7f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(Longcube, new Vector3(-32.5f, 0f, 15.3f), Quaternion.Euler(new Vector3(0, 90, 0)));
        Instantiate(Longcube, new Vector3(-38.4f, 0f, 27.4f), Quaternion.identity);
        Instantiate(Longcube, new Vector3(-23.8f, 0f, -32.2f), Quaternion.identity);
        Instantiate(Longcube, new Vector3(25.7f, 0f, -33.5f), Quaternion.identity);
        Instantiate(Longcube, new Vector3(-11.5f, 0f, 30.3f), Quaternion.Euler(new Vector3(0, 90, 0)));
    }


}
