using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector3 _pos;
    private BoxCollider _fleeCollider;
    private bool _spooked;
    private bool _fly;
    private bool _stick;
    private bool _return;


    // Start is called before the first frame update
    void Start()
    {
        _fleeCollider = GetComponent<BoxCollider>();

        _fleeNPC.transform.position = _startPos.transform.position;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            Debug.Log("Initiate Protocol: FLEE");
            _spooked = true;
            StartCoroutine(Flee());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            Debug.Log("Initiate Protocol: FLEE");
            _spooked = false;
            StartCoroutine(Return());
        }
    }

    IEnumerator Flee()
    {
        while(_spooked && _fleeNPC.transform.position != _launchPos.transform.position)
        {
            _fleeNPC.transform.position = Vector3.Lerp(_startPos.transform.position, _launchPos.transform.position, Time.deltaTime);
        }
        yield return null;
    }

    IEnumerator Return()
    {
        yield return null;
    }
}
