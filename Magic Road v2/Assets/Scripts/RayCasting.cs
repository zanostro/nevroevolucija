using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class RayCasting : MonoBehaviour
{

    public Global global;
    public bool drawLines;
    public int ime = 0;

    public static int CarCounter = 0;
    public LayerMask mask;

    public float forwardMaxDistance;
    public float normalMaxDistance;
    private int rayCount = Global.rayCount;
    

    public void setRayCount(int rayCount)
    {
        if (360 % rayCount == 0) this.rayCount = rayCount;
        else
        {
            Debug.Log("Invalid input");
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
        RayCasting.CarCounter++;
        ime = RayCasting.CarCounter;

        global.distances = new float[rayCount];


    }

    // Update is called once per frame
    void Update()
    {
        
        float angle = 0;
     //   float angle = -((360 / rayCount) / 2); za offcenter start
        for (int i = 0; i < rayCount; i++)
        {
            angle += 360 / rayCount;            //ELEMENT 0 NI NA ANGLE 0!!!!!!! AMPAK NA RAZLIKI KOTOV!!!!!!!!
            castRays(angle, i);

            //po default je infinate distance 0 - to spremeni v max float number
            if (global.distances[i] == 0) global.distances[i] = 1;
        }

     }






    void castRays(float angle, int i)
    {
        Vector3 initDir = transform.forward;
        Quaternion angleQ = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 newVector = angleQ * initDir;

        Ray ray1 = new Ray(transform.position, newVector);

        RaycastHit hitInfo;

        //za sprednji ray
        if (i == rayCount - 1)
        {
            if (Physics.Raycast(ray1, out hitInfo, forwardMaxDistance, mask))
            {
                if (drawLines) Debug.DrawLine(ray1.origin, hitInfo.point, Color.red);
            }
            else
            {
                if (drawLines) Debug.DrawLine(ray1.origin, ray1.origin + ray1.direction * forwardMaxDistance, Color.green);
            }
            global.distances[i] = hitInfo.distance / normalMaxDistance;

        }

        //za ostale
        else
        {
            if (Physics.Raycast(ray1, out hitInfo, normalMaxDistance, mask))
            {
                if (drawLines) Debug.DrawLine(ray1.origin, hitInfo.point, Color.red);
            }
            else
            {
                if (drawLines) Debug.DrawLine(ray1.origin, ray1.origin + ray1.direction * normalMaxDistance, Color.green);
            }
            global.distances[i] = hitInfo.distance / normalMaxDistance;
        }
    }
}

