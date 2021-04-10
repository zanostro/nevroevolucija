using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killPlayer : MonoBehaviour
{



    public Transform objectTransfom;

    private float noMovementThreshold = 0.0001f;
    private const int noMovementFrames = 3;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    private bool isMoving;



    public Global global;
    public ScoreCounter sc;



    private void Update()
    {
        if (global.avgSpeed < 0.509 && global.avgSpeed > 0.47) destroyPlayer();
        
    }

    public void destroyPlayer()
    {
        global.isActive = false;
        sc.StopCounting();
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "killPlayer") destroyPlayer();

    }



    //ali se premika
    private void Start()
    {
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }
    }

 
    public bool IsMoving
    {
        get { return isMoving; }
    }

    private void movement()
    {

        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = objectTransfom.position;

        //Check the distances between the points in your previous locations
        //If for the past several updates, there are no movements smaller than the threshold,
        //you can most likely assume that the object is not moving
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= noMovementThreshold)
            {
                //The minimum movement has been detected between frames
                isMoving = true;
                break;
            }
            else
            {
                isMoving = false;
            }
        }
    }
}
