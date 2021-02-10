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
    private bool HMloaded = true;

    [SerializeField]
    private Vector3 MapScale = new Vector3(1, 1, 1);
    // Start is called before the first frame update
    void Start()
    {
        terrain = this.GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;

        CreateHeightmap();
    }

    /*private void OnValidate()
    { 
        if (HMloaded)
        {
            CreateHeightmap();
        }
    }*/

    void CreateHeightmap()
    {
        float[,] Map = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int w = 0; w < terrainData.heightmapResolution; w++)
        {
            for (int h = 0; h < terrainData.heightmapResolution; h++)
            {
                if (HMloaded)
                {
                    Map[w, h] = heightMapImage.GetPixel((int)(w * MapScale.x),(int)(h * MapScale.z)).grayscale* MapScale.y;
                }
            }
        }

        terrainData.SetHeights(0, 0, Map);
    }
}
