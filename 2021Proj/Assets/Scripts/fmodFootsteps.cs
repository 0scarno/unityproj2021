using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fmodFootsteps : MonoBehaviour
{
    //Declare and FMOD instance
    private FMOD.Studio.EventInstance _instance;

    public void PlayFootstep()
    {
        //Create the instance
        _instance = FMODUnity.RuntimeManager.CreateInstance("event:/Player/PlayerFootsteps");
        _instance.start(); //Begins the Event
        _instance.release(); //Frees the event from memory
        
    }
}
