using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadlevel : MonoBehaviour
{
    bool hit = false;
    bool hit2 = false;
    Scene Scene;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        loadinglevel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "finishline")
        {
            hit = true;
            print("collided");
        }

        if (other.gameObject.tag == "finishingplane")
        {
            hit2 = true;
            print("collided");
        }


    }

    void loadinglevel()
    {
        if(hit==true & SceneManager.GetActiveScene().name == "RoadGenerator")
        {
            SceneManager.LoadScene("RoadGenLVL2");
            hit = false;
        }
        if (hit == true & SceneManager.GetActiveScene().name == "RoadGenLVL2")
        {
            SceneManager.LoadScene("RoadGeneratorLVL3");
            hit = false;
        }
        if (hit == true & SceneManager.GetActiveScene().name == "RoadGeneratorLVL3")
        {
            SceneManager.LoadScene("WinScene");
            hit = false;
        }

        if (hit2 == true && SceneManager.GetActiveScene().name == "MazeGame")
        {
            SceneManager.LoadScene("WinScene");
            hit2 = false;
        }

    }
}
