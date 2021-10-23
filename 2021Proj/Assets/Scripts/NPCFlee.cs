using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPCFlee : MonoBehaviour
{

    [SerializeField]
    private GameObject _startPos;

    [SerializeField]
    private GameObject _launchPos;

    [SerializeField]
    private GameObject[] _desinations;

    [SerializeField]
    private GameObject _fleeNPC;

    Sequence fleeSequence = DOTween.Sequence();

    private Vector3 _pos;
    private BoxCollider _fleeCollider;
    private bool _spooked;
    private bool _fly;
    private bool _stick;
    private bool _return;
    //private DOTween runTween = (() => _fleeNPC.transform.position, x => _fleeNPC.transform.position = x, _launchPos, 2);
    //private TweenCallback flyTween = transform.DOMove(_pos, 2);


    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();

        _fleeCollider = GetComponent<BoxCollider>();

        _fleeNPC.transform.position = _startPos.transform.position;
        _spooked = false;
        _fly = false;
        _stick = false;
        _return = false;
    }

    // Update is called once per frame
    void Update()
    {
        // when the player enters the trigger, 
        // move to the launch point slower than a sprint, 
        // pick a spot to fly to
        // then fly to the spot
        // wait there until the player has left
        //then return to the starting position.

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            _spooked = true;
            _pos = _desinations[Random.Range(0, _desinations.Length)].transform.position;

            if (_fleeNPC.transform.position == _startPos.transform.position)
            {
                _fleeNPC.transform.DOMove(_launchPos.transform.position, 1f);
            }

            if (_fleeNPC.transform.position == _launchPos.transform.position)
            {
                _fleeNPC.transform.DOMove(_pos, 2f);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            _spooked = false;
            //StartCoroutine(Return());
        }
    }
}
