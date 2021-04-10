using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    public float maxDelta = .1f;
    public float maxDegreesDelta = 5f;

    Transform targetPosition;
    Transform targetLookTowards;

    // Start is called before the first frame update
    void Start()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetPosition = target.Find("Camera Ideal Position");
            targetLookTowards = target.Find("Camera Look Towards");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, maxDelta);

            Quaternion lookRotation = Quaternion.LookRotation(targetLookTowards.position-transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, maxDegreesDelta);
        }
    }
}
