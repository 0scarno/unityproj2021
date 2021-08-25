using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private enum CURRENT_TERRAIN { DIRT, GRASS, ROCK, WATER, WOOD };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    private FMOD.Studio.EventInstance footsteps;

    // Update is called once per frame
    private void Update()
    {
        DetermineTerrain();
    }

    private void DetermineTerrain()
    {
        RaycastHit[] hit;

        hit = Physics.RaycastAll(transform.position, Vector3.down, 10.0f);

        foreach (RaycastHit rayhit in hit)
        {
            if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Dirt"))
            {
                currentTerrain = CURRENT_TERRAIN.DIRT;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Grass"))
            {
                currentTerrain = CURRENT_TERRAIN.GRASS;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Rock"))
            {
                currentTerrain = CURRENT_TERRAIN.ROCK;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                currentTerrain = CURRENT_TERRAIN.WATER;
            }
            else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                currentTerrain = CURRENT_TERRAIN.WOOD;
            }
        }
    }

    public void SelectAndPlayFootstep()
    {
        switch (currentTerrain)
        {
            case CURRENT_TERRAIN.DIRT:
                PlayFootstep(0);
                break;
            
            case CURRENT_TERRAIN.GRASS:
                PlayFootstep(1);
                break;
            
            case CURRENT_TERRAIN.ROCK:
                PlayFootstep(2);
                break;
            
            case CURRENT_TERRAIN.WATER:
                PlayFootstep(3);
                break;
            
            case CURRENT_TERRAIN.WOOD:
                PlayFootstep(4);
                break;

            default:
                PlayFootstep(0);
                break;
        }
    }
    private void PlayFootstep(int terrain)
    {
        footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Footsteps");
        footsteps.setParameterByName("Terrain", terrain);
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        footsteps.start();
        footsteps.release();
    }
}
