using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public int twigsOnTree;

    private void Update()
    {
        if (twigsOnTree == 0)
        {
            Debug.Log("I'm Awake!");
        }
    }
}
