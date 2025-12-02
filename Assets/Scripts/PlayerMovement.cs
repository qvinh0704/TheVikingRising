using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    public float moveSpeed;
    public float rotationMaxDegree;
    public float jumpHeight;

    public float yGravity;
    private bool isJumping;
    private bool isGrounded;

    private readonly string ANIMATION_CHOPPING = "IsChopping";
    private readonly string ANIMATION_ATTACK = "IsAttack";
    private readonly string ANIMATION_DEFEND = "IsDefend";
    private readonly string ANIMATION_HEALING = "IsHealing";
    private readonly string ANIMATION_INJURED = "IsInjured";
    private readonly string ANIMATION_RUNNING = "IsRun";
    private readonly string ANIMATION_SAILING = "IsSailing";
    private readonly string ANIMATION_JUMPING = "IsJumping";
    private readonly string ANIMATION_IS_FALLING = "IsFalling";
    private readonly string ANIMATION_IS_GROUNDED = "IsGrounded";

    public AnimationClip jumpClip;

    private Vector2 turn;
    public float sensitivity = 0.5f;
    public Vector3 deltaMove;
    public float speed = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);


        moveDirection.Normalize();

        Run(moveDirection);
        Jump(moveDirection);
        TurnAround(moveDirection);
        // Attack();
        ChopWood();
        Defend();
    }



    private void TurnAround(Vector3 turnDirection)
    {


        if (turnDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(turnDirection,
                Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                toRotation, rotationMaxDegree * Time.deltaTime);
        }
    }

    private void Attack()
    {
        // Attack
        if (Input.GetKey(KeyCode.Mouse0))
        {
            animator.SetBool(ANIMATION_ATTACK, true);
        }
        else
        {
            animator.SetBool(ANIMATION_ATTACK, false);
        }
    }

    private void Defend()
    {
        //Defend
        if (Input.GetKey(KeyCode.Mouse1))
        {
            animator.SetBool(ANIMATION_DEFEND, true);
        }
        else
        {
            animator.SetBool(ANIMATION_DEFEND, false);
        }
    }

    private void Run(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero)
        {
            animator.SetBool(ANIMATION_RUNNING, true);
        }
        else
        {
            animator.SetBool(ANIMATION_RUNNING, false);
        }
    }

    private void Jump(Vector3 moveDirection)
    {
        yGravity += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            yGravity = -0.1f;
            animator.SetBool(ANIMATION_IS_GROUNDED, true);
            isGrounded = true;
            animator.SetBool(ANIMATION_JUMPING, false);
            isJumping = false;
            animator.SetBool(ANIMATION_IS_FALLING, false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Set animation first - animator will process this before LateUpdate
                animator.SetBool(ANIMATION_JUMPING, true);
                isJumping = true;
                yGravity = jumpHeight;
            }
        }
        else
        {
            animator.SetBool(ANIMATION_IS_GROUNDED, false);
            isGrounded = false;


            bool isFallFromSky = isJumping;
            bool isFallFromGrounding = yGravity < -4f;

            if (isFallFromGrounding || isFallFromSky)
            {
                animator.SetBool(ANIMATION_IS_FALLING, true);
            }

        }

        if (!isGrounded)
        {
            Vector3 velocity = moveDirection * jumpHeight;
            velocity.y = yGravity;
            characterController.Move(velocity * Time.deltaTime);
        }
    }


    private void ChopWood()
    {
        // ChopWood
        if (Input.GetKey(KeyCode.Mouse0))
        {
            animator.SetBool(ANIMATION_CHOPPING, true);
        }
        else
        {
            animator.SetBool(ANIMATION_CHOPPING, false);
        }
    }
    private void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition;
        velocity.y = yGravity * Time.deltaTime;
        characterController.Move(velocity);
    }




}
