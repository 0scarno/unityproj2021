using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        float horiz = Input.GetAxisRaw("Horizontal");
        float verti = Input.GetAxisRaw("Vertical");

        if (_spriteRenderer!=true)
        {
            Debug.LogError( "Sprite Renderer not found");
        }
        else
        {
            
            if (horiz < 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (horiz > 0)
            {
                _spriteRenderer.flipX = true;
            }
        }

        if (_animator != true)
        {
            Debug.LogError("Animator not found");
        }
        else
        {
            if (horiz != 0 || verti != 0)
            {
                _animator.SetBool("isWalk", true);
            }
           else
            {
                _animator.SetBool("isWalk", false);
            }
        }

    }
}
