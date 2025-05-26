using UnityEngine;
using System.Collections;


public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private Transform player;
    private EnemyHealth health;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (health.currentHealth <= 0) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Flip לפי מיקום שחקן
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.flipX = player.position.x < transform.position.x;

        // התקרבות
        if (distance < detectionRange && distance > attackRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }

        // תקיפה
        else if (distance <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(10); // או טריגר
            lastAttackTime = Time.time;
            StartCoroutine(AttackAnimation());
            Debug.Log("Enemy attacked!");
        }
    }

    private IEnumerator AttackAnimation()
    {
        Vector3 originalPos = transform.position;
        Vector3 attackPos = player.position;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 10;
            transform.position = Vector3.Lerp(originalPos, attackPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 10;
            transform.position = Vector3.Lerp(attackPos, originalPos, t);
            yield return null;
        }
    }
}
