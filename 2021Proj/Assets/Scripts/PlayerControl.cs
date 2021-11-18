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

    //Variable Statements
    private Vector3 inputDir;
    private Vector3 moveDir = Vector3.zero;
    private float moveSpd;
    private bool jumpOK;
    private Vector3 movementDelta;
    private bool isGrounded;
    private bool isBonk;
    private float jumpMax;
    private SpriteHandler _spriteHandler;
    private NPCFlee _fleescript;
    private MudScript _mudScript;
    private ParticleScript _particleScript;
    private RaycastHit hit;
    private bool _sliding;
    private float _slideLimit;
    private bool _playerControl;
    private Vector3 _contactPoint;

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = false;
        _slideLimit = _controller.slopeLimit;

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
        
        // Create the Angle the player is intending to travel at
        float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + camAngle;
        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * (Vector3.forward * inputDir.magnitude);

        //Ground Detector
        float radius = _capsuleCollider.radius * 0.9f;
        Vector3 gCheckPos = transform.position + (Vector3.down * 0.6f);
        LayerMask ground = LayerMask.GetMask("Ground");
        isGrounded = Physics.CheckSphere(gCheckPos, radius, ground);
        //Debug.Log("Grounded =" + isGrounded);

        //Bonk Helmet
        Vector3 bonkCheckPos = transform.position + (Vector3.up * 0.6f);
        isBonk = Physics.CheckSphere(bonkCheckPos, _capsuleCollider.radius, ground);
        
        //Stop Player from falling if they are grounded, apply fall multis if not
        if(isGrounded && movementDelta.y < 0 )
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
        else if(!isGrounded && movementDelta.y < 0)
        {
            _spriteHandler.SetGrounded(false);
            _spriteHandler.SetJump(false);
            jumpOK = false;
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
                if (Vector3.Angle(hit.normal,Vector3.up) > _slideLimit)
                {
                    _sliding = true;
                }
            }

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
                movementDelta.y += ( jumpMax - transform.position.y )/2*(Time.deltaTime);
                _spriteHandler.SetJump(true);
                _spriteHandler.SetGrounded(false);
            }
            else
            {
                jumpOK = false;
            }
        }

        //particle Light control
        if (movementDelta.y > 0 && _jumpHeight<jumpMax)
        {
            Debug.Log("emit");
            _particleScript.Emit();
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

        //set move to slide if it needs to
        if (_sliding)
        {
            Vector3 hitNormal = hit.normal;
            moveDir = new Vector3(hit.normal.x, -hit.normal.y, hit.normal.z);
            Vector3.OrthoNormalize(ref hitNormal, ref moveDir);
            moveDir *= _slideSpeed;
            //_playerControl = false;
        }

        //Apply Gravity
        movementDelta.y -= ((_gravityValue*_gravityValue)/3) * Time.deltaTime;


        //Apply the moves every frame
        _controller.Move((moveSpd * Time.deltaTime * moveDir) + movementDelta);

        //Debug Messages
        //Debug.Log("moveDir =" + moveDir);

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
    
    // Store point that we're in contact with for use in FixedUpdate if needed
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contactPoint = hit.point;
    }
}