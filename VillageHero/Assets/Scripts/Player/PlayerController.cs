using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector2 moveInput;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;


    //Attack 

    public Transform axePivot;
    public GameObject attackAreaPrefab;
    public Transform attackPoint;
    public float attackDuration = 0.2f;
    private bool isAttacking = false;
    
    //Ground collision
     private bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Transform groundCheckL;
    public float groundCheckRadius = 0.05f;

    // Wall collision
    public Transform wallCheck;
    public float wallCheckDistance = 0.03f;
    public LayerMask wallLayer;
    private bool isTouchingWall;
    public Transform wallCheckLeft;
    public Transform wallCheckLeftH;
    public Transform wallCheckRight;
    public Transform wallCheckRightH;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // תנועה שמאלה וימינה בלבד
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = 0; // מבטל תנועה למעלה/למטה

        // בדיקת אם על הרצפה
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer)|| Physics2D.OverlapCircle(groundCheckL.position, groundCheckRadius, groundLayer);

        // בדיקה אם נוגע בקיר
        if (moveInput.x < 0)
            isTouchingWall = Physics2D.Raycast(wallCheckLeft.position, Vector2.left, wallCheckDistance, wallLayer) || Physics2D.Raycast(wallCheckLeftH.position, Vector2.left, wallCheckDistance, wallLayer);
        else if (moveInput.x > 0)
            isTouchingWall = Physics2D.Raycast(wallCheckRight.position, Vector2.right, wallCheckDistance, wallLayer) || Physics2D.Raycast(wallCheckRightH.position, Vector2.right, wallCheckDistance, wallLayer);
        else
            isTouchingWall = false;


        // קפיצה
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // תקיפה
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        Debug.Log("isGrounded: " + isGrounded);
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        Quaternion originalRotation = axePivot.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, -120f);

        float elapsed = 0f;
        float duration = attackDuration;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            axePivot.localRotation = Quaternion.Lerp(originalRotation, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        axePivot.localRotation = originalRotation;

        GameObject attackArea = Instantiate(attackAreaPrefab, attackPoint.position, Quaternion.identity);
        attackArea.transform.parent = transform;

        yield return new WaitForSeconds(attackDuration);
        Destroy(attackArea);
        isAttacking = false;
    }

    void FixedUpdate()
    {
        // אם באוויר ונוגע בקיר בכיוון התנועה – בטל את התנועה האופקית
        if (!isGrounded && (
            (isTouchingWall && moveInput.x != 0)
        ))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        }
    }
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
