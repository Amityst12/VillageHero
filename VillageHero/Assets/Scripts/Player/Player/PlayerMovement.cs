using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerReferences refs;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private int jumps;
    private Vector2 particleStartPos;
    [SerializeField] private Animator animator;
    [SerializeField] public Transform visual;
    [SerializeField] private ParticleSystem dustParticles;
    void Start()
    {
        refs = GetComponent<PlayerReferences>();
        particleStartPos = dustParticles.transform.localPosition;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = 0;

        isGrounded = Physics2D.OverlapCircle(refs.groundCheck.position, refs.groundCheckRadius, refs.groundLayer) || Physics2D.OverlapCircle(refs.groundCheckL.position, refs.groundCheckRadius, refs.groundLayer);

        StartStopParticles();

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
        }
        //Move left
        if (moveInput.x < 0)
        {
            isTouchingWall =
                Physics2D.Raycast(refs.wallCheckLeft.position, Vector2.left, refs.wallCheckDistance, refs.wallLayer) ||
                Physics2D.Raycast(refs.wallCheckLeftH.position, Vector2.left, refs.wallCheckDistance, refs.wallLayer);

            visual.localScale = new Vector3(-1, 1, 1);
            if (animator != null)
                animator.SetBool("isRunning", true);

            dustParticles.transform.localPosition = particleStartPos;
            dustParticles.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveInput.x > 0) //Move right
        {
            isTouchingWall =
                Physics2D.Raycast(refs.wallCheckRight.position, Vector2.right, refs.wallCheckDistance, refs.wallLayer) ||
                Physics2D.Raycast(refs.wallCheckRightH.position, Vector2.right, refs.wallCheckDistance, refs.wallLayer);

            visual.localScale = new Vector3(1, 1, 1);
            if (animator != null)
                animator.SetBool("isRunning", true);

            dustParticles.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            isTouchingWall = false;
            if (animator != null)
                animator.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.W) && (isGrounded || jumps > 0))
        {
            refs.rb.velocity = new Vector2(refs.rb.velocity.x, refs.jumpForce);
            jumps--;
        }

        if (isGrounded) jumps = refs.maxJumps;
    }

    void FixedUpdate()
    {
        if (!isGrounded && isTouchingWall)
            refs.rb.velocity = new Vector2(0, refs.rb.velocity.y);
        else
            refs.rb.velocity = new Vector2(moveInput.x * refs.moveSpeed, refs.rb.velocity.y);
    }

    private void StartStopParticles()
    {
        bool isMoving = Mathf.Abs(moveInput.x) > 0.1f;

        if (isGrounded && isMoving)
        {
            if (!dustParticles.isPlaying)
                dustParticles.Play();
        }
        else
        {
            if (dustParticles.isPlaying)
                dustParticles.Stop();
        }
    }
}
