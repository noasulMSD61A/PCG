using System.Collections;
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


public class TerrainMaker : MonoBehaviour
{
    private Terrain terrain;

    private TerrainData terrainData;

    //options in editor
    [SerializeField]
    private bool generateTerrain = false;

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
    private float maxRandomHeightRange = 1f;

    //variables for generating terrain using perlin noise
    [SerializeField]
    private float perlinNoiseWidthScale;

    [SerializeField]
    private float perlinNoiseHeightScale;

    

    //variables for adding textures to our terrain
    [SerializeField]
    private List<TerrainTextureData> terrainTextureDataList;

    [SerializeField]
    private float terrainTextureBlendOffset = 0.01f;

 

    

    public GameObject player;

    void Start()
    {
        //terrain = GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;
        createPlayer();
        CreateTerrain();
        //AddTree();
        
    }

    void initialise()
    {
#if UNITY_EDITOR

        if (terrain == null)
        {
            terrain = GetComponent<Terrain>();
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
            
            //CreateTerrain();
        }

        if (removeTexture)
        {
            addTexture = false;
        }

        if (addTexture || removeTexture)
        {
            TerrainTexture();
        }

       

        
    }

    void CreateTerrain()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        //float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

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

    

    

    void createPlayer()
    {

        Instantiate(player, new Vector3(Random.Range(500f, 1000f), 300f, Random.Range(500f,1000f)), Quaternion.identity);
    }

}

