using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    public float moveSpeed;
    public float rotationMaxDegree;
    public float jumpHeight;

    private float yGravity;
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
    private readonly string ANIMATION_IDLE = "IsIdle";
    private readonly string ANIMATION_IS_FALLING = "IsFalling";
    private readonly string ANIMATION_IS_GROUNDED = "IsGrounded";


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
        float inputMagnitude = moveDirection.magnitude;

        moveDirection.Normalize();



        yGravity += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            yGravity = -0.5f;
            animator.SetBool(ANIMATION_IS_GROUNDED, true);
            isGrounded = true;
            animator.SetBool(ANIMATION_JUMPING, false);
            isJumping = false;
            animator.SetBool(ANIMATION_IS_FALLING, false);

            if (Input.GetButtonDown("Jump"))
            {
                animator.SetBool(ANIMATION_JUMPING, true);
                yGravity = jumpHeight;
                isJumping = true;
            }
        }
        else
        {
            animator.SetBool(ANIMATION_IS_GROUNDED, false);
            isGrounded = false;

            if ((isJumping && yGravity < 0) || yGravity < -3.5f)
            {
                animator.SetBool(ANIMATION_IS_FALLING, true);
            }
        }



        if (moveDirection != Vector3.zero)
        {
            animator.SetBool(ANIMATION_RUNNING, true);
            Quaternion toRotation = Quaternion.LookRotation(moveDirection,
                Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                toRotation, rotationMaxDegree * Time.deltaTime);
        }
        else
        {
            animator.SetBool(ANIMATION_RUNNING, false);
        }

        if (!isGrounded)
        {
            Vector3 velocity = moveDirection * inputMagnitude * jumpHeight;
            velocity.y = yGravity;
            characterController.Move(velocity * Time.deltaTime);
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition;
        velocity.y = yGravity * Time.deltaTime;
        characterController.Move(velocity);
    }
}
