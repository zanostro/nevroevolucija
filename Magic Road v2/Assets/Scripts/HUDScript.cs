using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    public Text scoreValue;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    

    public void ShowScore(float score)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        scoreValue.text = ((int)score).ToString();  
    }
}
