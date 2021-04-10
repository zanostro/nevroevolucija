using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{

    public static int speedSample = 30;


    //distance
    Vector3 oldPos;
    float totalDistance = 0;



    public List<float> speedList;

    public Global global;

    public static float startingScore = 0;
    public static float defaultScoreSpeed = 0.005f;
    public static float noRoadScoreSpeed = 0;

    int numOfRoadsTouching;

    public float score;

    bool counting;

    HUDScript hud;

    Camera[] cameras;

    // Start is called before the first frame update
    void Start()
    {
        oldPos = transform.position;
        counting = false;
        numOfRoadsTouching = 0;
        score = startingScore;

        hud = FindObjectOfType<HUDScript>();

        cameras = FindObjectsOfType<Camera>();

        new WaitForSeconds(Global.sleep);
    }

    // Update is called once per frame
    void Update()
    {   
        counting = global.isActive;

        Vector3 distanceVector = transform.position - oldPos;
        float distanceThisFrame = distanceVector.magnitude;
        totalDistance += distanceThisFrame;
        oldPos = transform.position;

    }



    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Road")
        {
            numOfRoadsTouching++;
        }

        if (other.tag == "Finish")
        {
            StopCounting();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Road")
        {
            numOfRoadsTouching--;
        }
    }

    public void StartCounting()
    {
        counting = true;
    }

    public void StopCounting()
    {
        score = 0;
        counting = false;
        score += totalDistance;
        
    }

    public float getScore()
    {
        return score;
    }

    public void addScore(float valueToAdd)
    {
        score += valueToAdd;
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(1f);
        switchCameras();
        yield return new WaitForSeconds(4f);
        Application.Quit();
    }

    public void switchCameras()
    {
        foreach(Camera camera in cameras)
        {
            if (camera.tag == "MainCamera") camera.enabled = false;
        }
    }
}
