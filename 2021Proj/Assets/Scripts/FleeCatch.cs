using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeCatch : MonoBehaviour
{
    private NPCFlee _fleescript;

    void Start()
    {
        _fleescript = GameObject.Find("FleeTrigger").GetComponent<NPCFlee>();
        if (_fleescript == null)
        {
            Debug.LogError("FleeScript is null");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_fleescript.spooked)
        {
            _fleescript.caught = true;
            Debug.Log("You caught me!");
        }
        else
        {
            Debug.Log("Go away you are scary");
        }
    }
}
