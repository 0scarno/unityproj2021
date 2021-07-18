using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        float horiz = Input.GetAxisRaw("Horizontal");
        if (horiz < 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (horiz > 0)
        {
            _spriteRenderer.flipX = true;
        }
        
    }
}
