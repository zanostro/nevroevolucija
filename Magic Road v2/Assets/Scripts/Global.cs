using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public bool firstTime = true;
    public List<float> speedList;
    public float speedSum = 0;
    public float avgSpeed = 0;

    //global variables
    public int index;
    public bool isActive = false;
    public static int count = 0;
    public static float sleep = 0.7f;
    public bool render;

    //rayCasting
    public static int rayCount = 8;
    public float[] distances = new float[rayCount];

    //neuralNetwork
    public Network network = new Network(networkStructure);
    public float gas = 0.5f;
    public float stearing = 0.5f;


    public static int[] networkStructure = {rayCount, 10, 2};

    private void Start()
    {
        /*
        GameObject child = GameObject.Find("Lower Body");
        if (child == null) Debug.Log("null");
        child.GetComponent<MeshRenderer>().enabled = false;*/
              
        
        //ugasne ali prizge render mesh

        this.index = Global.count;
        Global.count++;

        while (!network.active) { } // caki da se konstruira

        isActive = true;
    }

    private void Update()
    {

        if (isActive)
        {
            double []inp = Array.ConvertAll(distances, x => (double)x);
            double[] d = network.calculate(inp);

            gas = (float)d[0];
            stearing = (float)d[1];

            speedAverage();
        }
    }

    public void cloneNetwork(Network n)
    {
        
        network = (Network) n.Clone();

    }

    private void speedAverage()
    {
        float speed = gas;
        speedList.Add(speed);
        speedSum += speed;

        if (speedList.Count > 20)
        {
            speedSum -= speedList[0];
            speedList.RemoveAt(0);

        }

        avgSpeed =  speedSum / speedList.Count;
    }
}
