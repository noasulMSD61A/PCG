using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TreeData
{
    public GameObject treeMesh;
    public float minHeight;
    public float maxHeight;
}
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

    [SerializeField]
    private List<TreeData> treeDataList;

    [SerializeField]
    private int maxTrees = 2000;

    [SerializeField]
    private int treeSpacing = 10;

    

    [SerializeField]
    private float randomXRange = 5.0f;

    [SerializeField]
    private float randomZRange = 5.0f;

    [SerializeField]
    private int terrainLayerIndex = 8;
    void Start()
    {
        
        terrainData = Terrain.activeTerrain.terrainData;
        AddWater();
        AddMist();
        Addclouds();
        AddTree();

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
            waterHeight * terrainData.size.y * 3.5f,
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
            waterHeight * terrainData.size.y,
            terrainData.size.z / 2);

        MistGameObject.transform.localScale = new Vector3(terrainData.size.x,100 , terrainData.size.z);

    }

    void AddTree()
    {
        TreePrototype[] trees = new TreePrototype[treeDataList.Count];

        for (int i = 0; i < treeDataList.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeDataList[i].treeMesh;
        }

        terrainData.treePrototypes = trees;

        List<TreeInstance> treeInstanceList = new List<TreeInstance>();

        
            for (int z = 0; z < terrainData.size.z; z += treeSpacing)
            {
                for (int x = 0; x < terrainData.size.x; x += treeSpacing)
                {
                    for (int treePrototypeIndex = 0; treePrototypeIndex < trees.Length; treePrototypeIndex++)
                    {
                        if (treeInstanceList.Count < maxTrees)
                        {
                            float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y;

                            if (currentHeight >= treeDataList[treePrototypeIndex].minHeight &&
                               currentHeight <= treeDataList[treePrototypeIndex].maxHeight)
                            {
                                float randomX = (x + Random.Range(-randomXRange, randomXRange)) / terrainData.size.x;
                                float randomZ = (z + Random.Range(-randomZRange, randomZRange)) / terrainData.size.z;

                                TreeInstance treeInstance = new TreeInstance();

                                treeInstance.position = new Vector3(randomX, currentHeight, randomZ);

                                Vector3 treePosition = new Vector3(treeInstance.position.x * terrainData.size.x,
                                                                    treeInstance.position.y * terrainData.size.y,
                                                                    treeInstance.position.z * terrainData.size.z) + this.transform.position;



                                RaycastHit raycastHit;
                                int layerMask = 1 << terrainLayerIndex;

                                if (Physics.Raycast(treePosition, Vector3.down, out raycastHit, 100, layerMask) ||
                                    Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                                {
                                    float treeHeight = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                    treeInstance.position = new Vector3(treeInstance.position.x, treeHeight, treeInstance.position.z);

                                    treeInstance.rotation = Random.Range(0, 360);
                                    treeInstance.prototypeIndex = treePrototypeIndex;
                                    treeInstance.color = Color.white;
                                    treeInstance.lightmapColor = Color.white;
                                    treeInstance.heightScale = 0.95f;
                                    treeInstance.widthScale = 0.95f;

                                    treeInstanceList.Add(treeInstance);
                                }
                            }
                        }
                    }
                }
            
        }

        terrainData.treeInstances = treeInstanceList.ToArray();
    }

}
