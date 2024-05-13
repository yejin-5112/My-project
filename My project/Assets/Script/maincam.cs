using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{

    Vector3 offset;
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //백터의 합
        transform.position = playerTransform.position + offset;

        offset = transform.position - playerTransform.position;
    }
}