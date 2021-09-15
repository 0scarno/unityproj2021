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
        if (_spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer not found");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator not found");
        }

    }
    // Update is called once per frame
    void Update()
    {
        float horiz = Input.GetAxisRaw("Horizontal");
        float verti = Input.GetAxisRaw("Vertical");

        if (horiz > 0)
        {
            SetFlip(true);
        }
        else if (horiz < 0)
        {
            SetFlip(false);
        }

        if (verti > 0)
        {
            SetFace(false);
        }
        else if (verti < 0)
        {
            SetFace(true);
        }

        if (horiz * horiz + verti * verti != 0)
        {
            SetWalk(true);
        }
        else
        {
            SetWalk(false);
        }
    }


    void SetFlip(bool flipBool)
    {
        _spriteRenderer.flipX = flipBool;
    }
    void SetFace(bool faceBool)
    {
        _animator.SetBool("isFace", faceBool);
    }
    void SetWalk(bool walkBool)
    {
        _animator.SetBool("isWalk", walkBool);
    }

    public void SetSprint(bool sprintBool)
    {
        _animator.SetBool("isSprint", sprintBool);
    }
    public void SetJump(bool jumpBool)
    {
        _animator.SetBool("isJump", jumpBool);
    }
    public void SetGrounded(bool groundedBool)
    {
        _animator.SetBool("isGrounded", groundedBool);
    }
}
