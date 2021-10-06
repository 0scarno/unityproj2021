using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLight : MonoBehaviour
{
    float range = 1.0f;
    float scale = 1.0f;
    void Start()
    {
        
        
    }
    void Update()
    {
        //make the light wibble randomly around a general spot
        float dx = range * Mathf.PerlinNoise(Time.deltaTime * scale, 0.0f);
        float dy = range * Mathf.PerlinNoise(Time.deltaTime * scale, 0.0f);
        Vector3 pos = transform.position;
        pos.x = dx;
        pos.y = dy;
        transform.position = pos;

        //fluctuate in size

        //get brighter when the player jumps.

    }
}
