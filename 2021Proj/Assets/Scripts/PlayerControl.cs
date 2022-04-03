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
    private float _slopeTolerance = 0.5f;

    [SerializeField]
    private float _slideSpeed = 3f;
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private LayerMask bouncy;

    //Variable Statements
    private Vector3 inputDir;
    private Vector3 moveDir = Vector3.zero;
    private Vector3 movementDelta;
    private Vector3 gCheckPos;
    private Vector3 bonkCheckPos;


    private float moveSpd;
    private float jumpMax;
    private float _slideLimit;
    private float bounceJump;
    private float radius;
    private float horiz;
    private float verti;
    private float jumpBtn;
    private float sprintBtn;

    private bool jumpOK;
    private bool isGrounded;
    private bool isBounce;
    private bool isBonk;
    private bool isBonkBounce;
    private bool _sliding;
    private bool _playerControl;
    private bool _bouncing;

    private SpriteHandler _spriteHandler;
    private NPCFlee _fleescript;
    private MudScript _mudScript;
    private ParticleScript _particleScript;

    private RaycastHit hit;
    private Vector3 _contactPoint;


    // Start is called before the first frame update
    void Start()
    {
        isGrounded = false;
        _slideLimit = _controller.slopeLimit;
        radius = _capsuleCollider.radius * 1.5f;
        jumpMax = gCheckPos.y + _jumpHeight;
        bounceJump = jumpMax + 3;
        //ground = LayerMask.GetMask("Ground");
        //bouncy = LayerMask.GetMask("Bouncy");
        NullRefExcep();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        horiz = Input.GetAxisRaw("Horizontal");
        verti = Input.GetAxisRaw("Vertical");
        jumpBtn = Input.GetAxisRaw("Jump");
        sprintBtn = Input.GetAxisRaw("Sprint");

        gCheckPos = transform.position + (Vector3.down * 0.72f);
        bonkCheckPos = transform.position + (Vector3.up * 1f);


        Camera();

        Gravity();

        DetectGround();

        Bonk();

        Jump();

        Move();

        SmoothSlope();

        ParticleControl();

        Debuging();
      
    }
    
    void NullRefExcep()
    {
        _spriteHandler = GameObject.Find("Quad").GetComponent<SpriteHandler>();
        if (_spriteHandler == null)
        {
            Debug.LogError("SpriteHandler is null");
        }
        _particleScript = GameObject.Find("ParticleLight").GetComponent<ParticleScript>();
        if (_particleScript == null)
        {
            Debug.LogError("ParticleScript is null");
        }
        _fleescript = GameObject.Find("FleeTrigger").GetComponent<NPCFlee>();
        if (_fleescript == null)
        {
            Debug.LogError("FleeScript is null");
        }

        _mudScript = GameObject.Find("mudObj").GetComponent<MudScript>();
        if (_mudScript == null)
        {
            Debug.LogError("MudScript is null");
        }
    }

    void Camera ()
    {
        // Camera Angle Assignment to Player Rotation
        float camAngle = cam.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, camAngle, 0f);

        // Create the Angle the player is intending to travel at
        float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + camAngle;
        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * (Vector3.forward * inputDir.magnitude);
    }

    void Gravity()
    {
        movementDelta.y -= ((_gravityValue * _gravityValue) / 3) * Time.deltaTime;
    }

    void DetectGround()
    {
        isGrounded = Physics.CheckSphere(gCheckPos, radius, ground);
        isBounce = Physics.CheckSphere(gCheckPos, radius, bouncy);

        if (isGrounded && movementDelta.y < 0)
        {
            _spriteHandler.SetGrounded(true);
            movementDelta.y = -0;
           
            if (jumpBtn == 0)
            {
                jumpOK = true;
            }

            jumpMax = gCheckPos.y + _jumpHeight;

            _sliding = false;

            //check if the gound under the player is beyond the slope limit
            if (Physics.Raycast(transform.position, -Vector3.up, out hit))
            {
                if (Vector3.Angle(hit.normal, Vector3.up) > _slideLimit)
                {
                    _sliding = true;
                }
            }
        }
        else if (!isGrounded && movementDelta.y < 0)
        {
            _spriteHandler.SetGrounded(false);
            _spriteHandler.SetJump(false);
            jumpOK = false;

            SlideCheck();
        }
        
        if (isBounce)
        {
            _bouncing = true;
        }
        if (transform.position.y >= bounceJump)
        {
            _bouncing = false;
        }
        if (jumpBtn >= 0.1 && _bouncing)
        {
            if (transform.position.y < bounceJump)
            {
                movementDelta.y = 0.15f;
                movementDelta.y += (jumpMax - transform.position.y) / 2 * (Time.deltaTime);
                _spriteHandler.SetJump(true);
                _spriteHandler.SetGrounded(false);
            }
        }
        else if (isBounce && jumpBtn < 0.1)
        {
            movementDelta.y = 0.067f;
            jumpMax = gCheckPos.y + _jumpHeight;
            _spriteHandler.SetJump(true);
            _spriteHandler.SetGrounded(false);
        }

    }

    void SlideCheck()
    {

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 0.6f))
        {
            if (Vector3.Angle(hit.normal, Vector3.up) > _slideLimit)
            {
                _sliding = true;
            }
        }
        else
        {
            Physics.Raycast(_contactPoint + Vector3.up, -Vector3.up, out hit);
            if (Vector3.Angle(hit.normal, Vector3.up) > _slideLimit)
            {
                _sliding = true;
            }
        }
    }

    void Bonk()
    {
        isBonk = Physics.CheckSphere(bonkCheckPos, radius, ground);
        isBonkBounce = Physics.CheckSphere(bonkCheckPos, radius, bouncy);
    }
    
    void Jump()
    {

        if (jumpBtn >= 0.1 && jumpOK)
        {
            if (transform.position.y < jumpMax && jumpOK && !isBonk)
            {
               
                movementDelta.y += (jumpMax - transform.position.y) * (Time.deltaTime*2);
                _spriteHandler.SetJump(true);
                _spriteHandler.SetGrounded(false);
            }
            else
            {
                jumpOK = false;
            }
        }
  
    }


    void Move()
    {
        // Directional Input Monitoring 
        inputDir = new Vector3(horiz, 0f, verti);

        _controller.Move((moveSpd * Time.deltaTime * moveDir) + movementDelta);

        //Vector Normalisation for Keyboard Input
        if (inputDir.magnitude > 1)
        {
            _ = inputDir.normalized;
        }

        //Sprint Check & Modify
        if (sprintBtn >= 0.1)
        {
            moveSpd = _baseSpd * _sprintMod * _mudScript.mudSpdMod;
            _spriteHandler.SetSprint(true);
            _fleescript.spooked = true;
        }
        else
        {
            moveSpd = _baseSpd * _mudScript.mudSpdMod;
            _spriteHandler.SetSprint(false);
            _fleescript.spooked = false;
        }

        if (_sliding)
        {
            Vector3 hitNormal = hit.normal;
            moveDir = new Vector3(hit.normal.x, -hit.normal.y, hit.normal.z);
            Vector3.OrthoNormalize(ref hitNormal, ref moveDir);
            moveDir *= _slideSpeed;
        }
    }

    void SmoothSlope()
    {
        if (movementDelta.y <= 0f)
        {
            //check for slope underneath

            if (Physics.Raycast(gCheckPos, Vector3.down, out hit))
            {
                // teleport to slope
                if (hit.distance < _slopeTolerance)
                {
                    //Debug.Log("hit");
                    _controller.Move(Vector3.down * hit.distance);
                }
            }
        }
    }

    void ParticleControl()
    {
        if (movementDelta.y > 0 && _jumpHeight < jumpMax)
        {
            _particleScript.Emit();
        }
    }

    void Debuging()
    {
        //Debug.Log("Bouncing=" + _bouncing);
        Debug.Log("Grounded=" + isGrounded);
        //Debug.Log("input Direction =" + inputDir);
        //Debug.Log("Jump Btn =" + jumpBtn);
        //Debug.Log("JumpOK = " + jumpOK);
        //Debug.Log("Mov.Y = " + movementDelta.y);
        //Debug.Log(" JumpMax = " + jumpMax);
        Debug.Log("Bonk is" + isBonk);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Store point that we're in contact with for use in FixedUpdate if needed

        _contactPoint = hit.point;
    }

    
     private void OnDrawGizmos()
     {
        //DrawBounceGizmo();
        DrawSpheresGizmo(false, true);// Bools = (ground, bonk)


    }

    private void DrawSpheresGizmo(bool ground, bool bonk)
    {
        if(ground)
        {
            Gizmos.DrawSphere(gCheckPos, radius);

        }
        if (bonk)
        {
            Gizmos.DrawSphere(bonkCheckPos, radius);
        }
    }

    private void DrawBounceGizmo()
    {
        Vector3 jumpMaxPoint = new Vector3(transform.position.x, jumpMax, transform.position.z);
        Vector3 bounceJumpPoint = new Vector3(transform.position.x, bounceJump, transform.position.z);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(jumpMaxPoint, 0.3f);
    }
}