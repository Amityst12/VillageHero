using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Rigidbody2D rb;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Transform groundCheckL;
    public float groundCheckRadius = 0.05f;



    [Header("Wall Check")]

    public Transform wallCheckLeft, wallCheckLeftH;
    public Transform wallCheckRight, wallCheckRightH;
    public float wallCheckDistance = 0.05f;
    public LayerMask wallLayer;



    [Header("Combat")]
    public Transform axePivot;
    public GameObject attackAreaPrefab;
    public Transform attackPoint;
    public float attackDuration = 0.2f;
}
