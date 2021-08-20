using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Serialised Fields
    [SerializeField]
    private CharacterController _controller;

    [SerializeField]
    private Transform cam;

    [SerializeField]
    private float _baseSpd = 6f;

    [SerializeField]
    private float _sprintMod = 2.5f;

    [SerializeField]
    private CapsuleCollider _capsuleCollider;

    [SerializeField]
    private float _gravityValue = 9.81f;

    [SerializeField]
    private float _jumpHeight = 2f;

    [SerializeField]
    private float _jumpSpd = 1f;

    //Variable Statements
    private Vector3 inputDir;
    private float moveSpd;
    private bool jumpOK;
    private Vector3 verticalInfluence;
    private bool isGrounded;
    private bool isBonk;
    private float jumpMax;
    private SpriteHandler _spriteHandler;

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = false;
        _spriteHandler = GameObject.Find("Quad").GetComponent<SpriteHandler>();
        if (_spriteHandler == null)
        {
            Debug.LogError("SpriteHandler is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Axis Assignment
        float horiz = Input.GetAxisRaw("Horizontal");
        float verti = Input.GetAxisRaw("Vertical");
        float jumpBtn = Input.GetAxisRaw("Jump");
        float sprintBtn = Input.GetAxisRaw("Sprint");

        // Camera Angle Assignment to Player Rotation
        float camAngle = cam.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, camAngle, 0f);

        //Ground Detector
        float radius = _capsuleCollider.radius * 0.9f;
        Vector3 gCheckPos = transform.position + (Vector3.down * 0.6f);
        LayerMask ground = LayerMask.GetMask("Ground");
        isGrounded = Physics.CheckSphere(gCheckPos, radius, ground);

        //Bonk Helmet
        Vector3 bonkCheckPos = transform.position + (Vector3.up * 0.6f);
        isBonk = Physics.CheckSphere(bonkCheckPos, radius, ground);
        
        //Stop Player from falling if they are grounded, apply fall multis if not
        if(isGrounded && verticalInfluence.y < 0f )
        {
            verticalInfluence.y = 0f;
            jumpOK = true;            
            jumpMax = gCheckPos.y + _jumpHeight;
            _spriteHandler.SetGrounded(true);
        }
        else if(!isGrounded && verticalInfluence.y < 0f)
        {
            jumpOK = false;
            _spriteHandler.SetGrounded(false);
            _spriteHandler.SetJump(false);
        }

        if (transform.position.y < jumpMax && isBonk)
        {
            jumpOK = false;
            _spriteHandler.SetJump(false);
        }

        //JUMP
        if (jumpBtn>=0.1 && jumpOK)
        {
            if (transform.position.y < jumpMax && jumpOK)
            {
                verticalInfluence.y += (jumpMax - transform.position.y)/2*(Time.deltaTime);
                _spriteHandler.SetJump(true);
                _spriteHandler.SetGrounded(false);

            }
            else
            {
                jumpOK = false;
            }
        }


        // Directional Input Monitoring 
        inputDir = new Vector3(horiz, 0f, verti);

        
        //Vector Normalisation for Keyboard Input
        if (inputDir.magnitude > 1) 
        {
            _ = inputDir.normalized;
        }

        //Sprint Check & Modify
        if (sprintBtn >= 0.1)
        {
            moveSpd = _baseSpd * _sprintMod;
            _spriteHandler.SetSprint(true);
        }
        else
        {
            moveSpd = _baseSpd;
            _spriteHandler.SetSprint(false);
        }

        // Create the Angle the player is intending to travel at
        float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + camAngle;
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * (Vector3.forward * inputDir.magnitude);

        //Apply Gravity
        verticalInfluence.y -= ((_gravityValue*_gravityValue)/3) * Time.deltaTime;

        //Apply the moves every frame
        _controller.Move((moveSpd * Time.deltaTime * moveDir) + verticalInfluence);
    }
}