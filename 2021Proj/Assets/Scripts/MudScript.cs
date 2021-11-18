using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MudScript : MonoBehaviour
{
    private PlayerControl _player;
    [SerializeField]
    private float _mudModAmnt = 3;
    private float _mud1 = 1;

    public float mudSpdMod = 1;
    
    
    private void Start()
    {
        DOTween.Init();
        _player = GameObject.Find("Player").GetComponent<PlayerControl>();
        if (_player == null)
        {
            Debug.LogError("MudScript cannot find Player");
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"));
        {
            DOTween.To(() => mudSpdMod, x => mudSpdMod = x, 1f / _mudModAmnt, 1);
        }
    }
    private void OnTriggerExit (Collider other)
    {
        if (other.CompareTag("Player"));
        {
            DOTween.To(() => mudSpdMod, x => mudSpdMod = x, 1f, 0.5f);
        }
    }
}
