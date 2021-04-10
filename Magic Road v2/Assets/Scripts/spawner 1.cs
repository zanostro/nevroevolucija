using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject avto;

    public void Spawn(Vector3 position)
    {
        Instantiate(avto).transform.position = position;

    }


   
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.F))
        {
            Spawn(new Vector3(-0.63f, 0.8823831f, -1.160174f));
        }


    }
}
