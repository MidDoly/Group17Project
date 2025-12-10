
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class PlayerControll : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5.0f;
    public float jumpStartingSpeed = 3.0f;
    public float fallingAcceleration = 0.98f;
    private const float groundPosition = 0f;
    public InputSystem_Actions playerControls;
    UnityEngine.Vector2 moveDirection = UnityEngine.Vector2.zero;
    Vector2 playerVelocity = Vector2.zero;
    private InputAction move;
    private InputAction fire;
    private InputAction jump;
    private Animator anim;
    private float ySpeed = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        anim = GetComponent<Animator>();
        playerControls = new InputSystem_Actions();
    }

    void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        fire = playerControls.Player.Attack;
        fire.Enable();
        fire.performed += OnAttack;
        jump = playerControls.Player.Jump;
        jump.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        fire.Disable();
        fire.performed -= OnAttack;
        jump.Disable();
    }
    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack performed");
        DoAttack();
    }
    private void DoAttack()
    {
        if (anim != null)
        {
            anim.SetTrigger("Fire");
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jump.ReadValue<float>() != 0 && rb.position.y <= groundPosition) ySpeed = jumpStartingSpeed;
        else if (rb.position.y > groundPosition) ySpeed -= fallingAcceleration;
        else ySpeed = 0; 
        moveDirection = move.ReadValue<UnityEngine.Vector2>();
        
        if (moveDirection.x == 0) playerVelocity = new Vector2(0, ySpeed);
        else playerVelocity = new Vector2(moveDirection.x / Math.Abs(moveDirection.x) * moveSpeed, ySpeed);
    }
    
    void FixedUpdate()
    {
        rb.linearVelocity = playerVelocity;
    }
}
