using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerReferences refs;
    private Vector2 moveInput;
    private bool isGrounded;
    private LayerMask wallLayer;

    private bool isTouchingWall;

    void Start()
    {
        refs = GetComponent<PlayerReferences>();
        wallLayer = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = 0;

        isGrounded = Physics2D.OverlapCircle(refs.groundCheck.position, refs.groundCheckRadius, refs.groundLayer) || Physics2D.OverlapCircle(refs.groundCheckL.position, refs.groundCheckRadius, refs.groundLayer);

        if (moveInput.x < 0)
        {
            isTouchingWall =
                Physics2D.Raycast(refs.wallCheckLeft.position, Vector2.left, refs.wallCheckDistance, refs.wallLayer) ||
                Physics2D.Raycast(refs.wallCheckLeftH.position, Vector2.left, refs.wallCheckDistance, refs.wallLayer);
        }
        else if (moveInput.x > 0)
        {
            isTouchingWall =
                Physics2D.Raycast(refs.wallCheckRight.position, Vector2.right, refs.wallCheckDistance, refs.wallLayer) ||
                Physics2D.Raycast(refs.wallCheckRightH.position, Vector2.right, refs.wallCheckDistance, refs.wallLayer);
        }
        else
        {
            isTouchingWall = false;
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            refs.rb.velocity = new Vector2(refs.rb.velocity.x, refs.jumpForce);
        }
    }

    void FixedUpdate()
    {
        if (!isGrounded && isTouchingWall)
            refs.rb.velocity = new Vector2(0, refs.rb.velocity.y);
        else
            refs.rb.velocity = new Vector2(moveInput.x * refs.moveSpeed, refs.rb.velocity.y);
    }
}
