using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LoadHeightMap : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    [SerializeField]
    private Texture2D heightMapImage;

    [SerializeField]
    private Vector3 heightMapScale = new Vector3(1, 1, 1);

    [SerializeField]
    private bool loadHeightMap = true;

    [SerializeField]
    private bool flattenHeightMap = false;

    // Start is called before the first frame update
    void Start()
    {
        terrain = this.GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;

        UpdateHeightmap();
    }

    private void OnValidate()
    {
        if (flattenHeightMap)
        {
            loadHeightMap = false;
        }

        if (loadHeightMap || flattenHeightMap)
        {
            UpdateHeightmap();
        }
    }

    void UpdateHeightmap()
    {
        //creates a new empty 2D array of float based on the dimensions of heightmap resolution set in the settings
        //float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        //gets the height map data that already exists in the terrain and loads it into a 2D array
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                if (loadHeightMap)
                {

                    heightMap[width, height] = heightMapImage.GetPixel((int)(width * heightMapScale.x),
                                                                       (int)(height * heightMapScale.z)).grayscale
                                                                       * heightMapScale.y;
                }

                if (flattenHeightMap)
                {

                    heightMap[width, height] = 0;
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }
}
