using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform axePivot; 
    public float moveSpeed = 5f;
    public float attackDuration = 0.2f;
    public bool isAttacking = false;
    public GameObject attackAreaPrefab;
    public Transform attackPoint;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // קלט תנועה WASD
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // תקיפה Space
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking )
        {
            Debug.Log("Attack!");
            StartCoroutine(Attack());
        }
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
    
    //Return to original rotation
    axePivot.localRotation = originalRotation;

    GameObject attackArea = Instantiate(attackAreaPrefab, attackPoint.position, Quaternion.identity);
    attackArea.transform.parent = transform;

    yield return new WaitForSeconds(attackDuration); //Time before kill

    Destroy(attackArea);
    isAttacking = false;
    }

    void FixedUpdate()
    {
        // תזוזה פיזיקלית
        rb.velocity = moveInput * moveSpeed;
    }
}
