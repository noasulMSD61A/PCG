using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTerrain : MonoBehaviour
{
    public GameObject Terrain;
    
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Terrain, this.transform.position, this.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
