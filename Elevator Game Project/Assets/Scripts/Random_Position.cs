using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Position : MonoBehaviour
{
    public Vector3[] positions;

    void Start()
    {
        int randomNumber = Random.Range(0, positions.Length);

        transform.position = positions[randomNumber];
    }

    
}
