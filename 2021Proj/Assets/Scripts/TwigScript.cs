using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwigScript : MonoBehaviour
{
    private Rigidbody t_rb;
    private Vector3 _forceDir;
    private Transform _tree;
    private Vector3 _home;
    [SerializeField]
    private float _resetTime;
    private Tree _treeScript;
    private void Start()
    {
        t_rb = GetComponent<Rigidbody>();
        t_rb.isKinematic = true;
        _tree = transform.parent;
        _home = _tree.position + Vector3.up * 21;
        transform.position = _home;
        _treeScript = GetComponentInParent<Tree>();
        Launch();
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.CompareTag("Character"))
        {
            StartCoroutine("Reset");
            _treeScript.twigsOnTree++;
        }
    }
    IEnumerator Reset()
    {
        //change sprite
        yield return new WaitForSeconds(1);
        //remove sprite
        Reposition();
        yield return new WaitForSeconds(_resetTime);
        Launch();
    }

    void Reposition()
    {
        t_rb.isKinematic = true;
        transform.position = _home;
    }

    void Launch()
    {
        t_rb.isKinematic = false;
        _forceDir = Random.onUnitSphere * (Random.Range(7, 15));
        _forceDir.y = 2;
        t_rb.velocity = _forceDir;
        _treeScript.twigsOnTree--;
    }

}
