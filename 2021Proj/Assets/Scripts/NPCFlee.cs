using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPCFlee : MonoBehaviour
{

    [SerializeField]
    private GameObject _startPos;

    [SerializeField]
    private GameObject[] _desinations;

    [SerializeField]
    private GameObject _fleeNPC;

    private Vector3 _pos;
    private BoxCollider _fleeCollider;
    public bool spooked;
    public bool caught;

    void Start()
    {
        DOTween.Init();
        _fleeCollider = GetComponent<BoxCollider>();
        _fleeNPC.transform.position = _startPos.transform.position;
        spooked = false;
        caught = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character") && spooked && !caught)
        {
            _pos = _desinations[Random.Range(0, _desinations.Length)].transform.position;

            if (_fleeNPC.transform.position == _startPos.transform.position)
            {
                _fleeNPC.transform.DOMove(_pos, 1f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            StartCoroutine(Return());
        }
    }

    IEnumerator Return()
    {
        yield return new WaitForSeconds(2);
        if (_fleeNPC.transform.position != _startPos.transform.position)
        {
            _fleeNPC.transform.DOMove(_startPos.transform.position, 1f);
        }
    }    
}
