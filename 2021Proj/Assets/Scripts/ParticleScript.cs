using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    private ParticleSystem _ps;
    private ParticleSystemRenderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        _renderer = GetComponent<ParticleSystemRenderer>();
    }

    public void Emit()
    {

        _ps.Emit(50);
    }

    //be less dark when the player jumps.
   // public void LightIntensity(float jumpMax, float playerY)
   // {
   //    var lights = _ps.lights;
   //    lights.intensityMultiplier = 1 - ((jumpMax - playerY) / 2);
   //    _renderer.forceRenderingOff = false;
   //    _renderer.maxParticleSize = 0.1f;
   //    _renderer.minParticleSize = 0.1f;
   // }
    //public void LightDim()
    //{
    //    var lights = _ps.lights;
    //    if (lights.intensityMultiplier > 0)
    //    {
    //        lights.intensityMultiplier -= Random.Range(0.5f,1.7f) * Time.deltaTime;
    //    }
    //    if (lights.intensityMultiplier <= 0)
    //    {
    //        _renderer.forceRenderingOff = true;
    //    }
    //}
}
