﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TerrainTextureData
{
    public Texture2D terrainTexture;
    public float minHeight;
    public float maxHeight;
    public Vector2 tileSize;
}

[System.Serializable]
class TreeData
{
    public GameObject treeMesh;
    public float minHeight;
    public float maxHeight;
}

[ExecuteInEditMode]
public class TerrainMaker : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    //options in editor
    [SerializeField]
    private bool generateTerrain = true;

    [SerializeField]
    private bool generatePerlinNoiseTerrain = false;

    [SerializeField]
    private bool flattenTerrain = false;

    [SerializeField]
    private bool addTexture = false;

    [SerializeField]
    private bool removeTexture = false;

    [SerializeField]
    private bool addTree = false;

    //variables for generating terrain using random values
    [SerializeField]
    private float minRandomHeightRange = 0f;

    [SerializeField]
    private float maxRandomHeightRange = 0.1f;

    //variables for generating terrain using perlin noise
    [SerializeField]
    private float perlinNoiseWidthScale;

    [SerializeField]
    private float perlinNoiseHeightScale;

    [SerializeField]
    private float perlinNoiseOffsetWidth = 1, perlinNosieOffsetHeight = 1;

    //variables for adding textures to our terrain
    [SerializeField]
    private List<TerrainTextureData> terrainTextureDataList;

    [SerializeField]
    private float terrainTextureBlendOffset = 0.01f;

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
        terrain = GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;

        CreateTerrain();


        AddTree();

    }
     
    

    void initialise()
    {
#if UNITY_EDITOR

        if (terrain == null)
        {
            terrain = GetComponent<Terrain>();
        }

        if (terrainData == null)
        {
            terrainData = Terrain.activeTerrain.terrainData;
        }

#endif
    }

    private void OnValidate()
    {
        initialise();

        if (flattenTerrain)
        {
            generateTerrain = false;
            generatePerlinNoiseTerrain = false;
        }

        if (generateTerrain || flattenTerrain || generatePerlinNoiseTerrain)
        {
            perlinNoiseWidthScale = Random.Range(0.01f, 0.02f);
            perlinNoiseHeightScale = Random.Range(0.01f, 0.02f);
            
            CreateTerrain();
        }

        if (removeTexture)
        {
            addTexture = false;
        }

        if (addTexture || removeTexture)
        {
            TerrainTexture();
        }

        if (addTree)
        {
            AddTree();
        }

        
    }


    void CreateTerrain()
    {
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                if (generateTerrain)
                {
                    heightMap[width, height] = Random.Range(minRandomHeightRange, maxRandomHeightRange);
                }

                if (generatePerlinNoiseTerrain)
                {
                    heightMap[width, height] = Mathf.PerlinNoise(width * perlinNoiseWidthScale, height * perlinNoiseHeightScale);
                }

                if (flattenTerrain)
                {
                    heightMap[width, height] = 0;
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    //this method is going to add textures to the terrain
    void TerrainTexture()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureDataList.Count];

        for (int i = 0; i < terrainTextureDataList.Count; i++)
        {
            if (addTexture)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = terrainTextureDataList[i].terrainTexture;
                terrainLayers[i].tileSize = terrainTextureDataList[i].tileSize;
            }
            else if (removeTexture)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = null;
            }
        }

        terrainData.terrainLayers = terrainLayers;


        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        float[,,] alphaMapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] splatmap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureDataList.Count; i++)
                {
                    float minHeight = terrainTextureDataList[i].minHeight - terrainTextureBlendOffset;
                    float maxHeight = terrainTextureDataList[i].maxHeight + terrainTextureBlendOffset;

                    if (heightMap[width, height] >= minHeight && heightMap[width, height] <= maxHeight)
                    {
                        splatmap[i] = 1;
                    }
                }

                NormaliseSplatMap(splatmap);

                for (int j = 0; j < terrainTextureDataList.Count; j++)
                {
                    alphaMapList[width, height, j] = splatmap[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMapList);
    }


    void NormaliseSplatMap(float[] splatmap)
    {
        float total = 0;

        for (int i = 0; i < splatmap.Length; i++)
        {
            total += splatmap[i];
        }

        for (int i = 0; i < splatmap.Length; i++)
        {
            splatmap[i] = splatmap[i] / total;
        }
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

        if (addTree)
        {
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
        }

        terrainData.treeInstances = treeInstanceList.ToArray();
    }

}

