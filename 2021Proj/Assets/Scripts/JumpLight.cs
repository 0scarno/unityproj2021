using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLight : MonoBehaviour
{
    [SerializeField]
    private float _wblScl = 1f;

    [SerializeField]
    private float _lerpScale = 1f;

    [SerializeField]
    private GameObject _player;

    private Vector3 destination;
    public Light _light;
    private Collider _collider;

    void Start()
    {
        StartCoroutine(Wobble(_lerpScale));

        _light = gameObject.GetComponent<Light>();

        _light.range = Random.Range(15f, 60f); ;

        _collider = GetComponentInParent<BoxCollider>();
        if (_collider == null)
        {
            Debug.LogError("LightContainer is Null");
        }
    }

    IEnumerator Wobble(float duration)
    {
        while (true)
        {
            float time = 0f;
            Vector3 startPosition = transform.position;
            Vector3 targetPos = startPosition + new Vector3(Random.Range(-_wblScl, _wblScl), Random.Range(-_wblScl, _wblScl), Random.Range(-_wblScl, _wblScl));

            while(time < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPos, time/duration);
                time += Time.deltaTime;
                yield return null;
            }
        }

    }
    
    // teleport or change direction if out of bounds
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LightKillbox"))
        {
            Debug.Log("exited");
            transform.position = _collider.ClosestPoint(transform.position);
        }
    }

    //be less dark when the player jumps.
    public void LightIntensity(float jumpMax, float playerY)

    {
        Debug.Log("Light intensify");

        _light.intensity = 1-((jumpMax-playerY)/3);
    }
    //slowly lost light after the player falls.
    public void LightDim()
    {
        Debug.Log("Light Dim");
        if(_light.intensity > 0)
        {
            _light.intensity -= 1.7f* Time.deltaTime;
        }
    }
}
