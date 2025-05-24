using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerReferences refs;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private int jumps;
    [SerializeField] private Animator animator;
    [SerializeField] public Transform visual;

    void Start()
    {
        refs = GetComponent<PlayerReferences>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = 0;

        isGrounded = Physics2D.OverlapCircle(refs.groundCheck.position, refs.groundCheckRadius, refs.groundLayer) || Physics2D.OverlapCircle(refs.groundCheckL.position, refs.groundCheckRadius, refs.groundLayer);


        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
        }

        if (moveInput.x < 0)
        {
            isTouchingWall =
                Physics2D.Raycast(refs.wallCheckLeft.position, Vector2.left, refs.wallCheckDistance, refs.wallLayer) ||
                Physics2D.Raycast(refs.wallCheckLeftH.position, Vector2.left, refs.wallCheckDistance, refs.wallLayer);

            visual.localScale =new Vector3(-1, 1, 1);
            animator.SetBool("isRunning", true);
        }
        else if (moveInput.x > 0)
        {
            isTouchingWall =
                Physics2D.Raycast(refs.wallCheckRight.position, Vector2.right, refs.wallCheckDistance, refs.wallLayer) ||
                Physics2D.Raycast(refs.wallCheckRightH.position, Vector2.right, refs.wallCheckDistance, refs.wallLayer);

            visual.localScale = new Vector3(1, 1, 1);
            animator.SetBool("isRunning", true);
        }
        else
        {
            isTouchingWall = false;
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
}
