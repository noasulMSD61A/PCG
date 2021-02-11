using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNaturalElements : MonoBehaviour
{
    [SerializeField]
    private GameObject water;

    [SerializeField]
    private float waterHeight = 0.1f;

    [SerializeField]
    private bool addWater;

    [SerializeField]
    private GameObject clouds;

    [SerializeField]
    private GameObject Mist;

    private TerrainData terrainData;
    void Start()
    {
        terrainData = Terrain.activeTerrain.terrainData;
        AddWater();
        AddMist();
        Addclouds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddWater()
    {
        GameObject waterGameObject = GameObject.Find("water");

        if (!waterGameObject)
        {
            waterGameObject = Instantiate(water, this.transform.position, this.transform.rotation);
            waterGameObject.name = "water";
        }

        waterGameObject.transform.position = this.transform.position + new Vector3(
            terrainData.size.x / 2,
            waterHeight * terrainData.size.y/1.2f,
            terrainData.size.z / 2);

        waterGameObject.transform.localScale = new Vector3(terrainData.size.x, 1, terrainData.size.z);
    }

    void Addclouds()
    {
        GameObject cloudGameObject = GameObject.Find("clouds");

        if (!cloudGameObject)
        {
            cloudGameObject = Instantiate(clouds, this.transform.position, this.transform.rotation);
            cloudGameObject.name = "cloud";
        }

        cloudGameObject.transform.position = this.transform.position + new Vector3(
            terrainData.size.x / 2,
            waterHeight * terrainData.size.y * 5,
            terrainData.size.z / 2);

        cloudGameObject.transform.localScale = new Vector3(terrainData.size.x, 420, terrainData.size.z);

    }

    void AddMist()
    {
        GameObject MistGameObject = GameObject.Find("Mist");

        if (!MistGameObject)
        {
            MistGameObject = Instantiate(Mist, this.transform.position, this.transform.rotation);
            MistGameObject.name = "Mist";
        }

        MistGameObject.transform.position = this.transform.position + new Vector3(
            terrainData.size.x / 2,
            waterHeight * terrainData.size.y /3,
            terrainData.size.z / 2);

        MistGameObject.transform.localScale = new Vector3(terrainData.size.x,100 , terrainData.size.z);

    }

}
