
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [Tooltip("Damage per hit")]
    public float damageAmount = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {

    if (other.CompareTag("Enemy"))
    {
        Debug.Log("It's an enemy!");
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damageAmount);
            Debug.Log("Enemy hit! Damage dealt: " + damageAmount);
        }
        else
        {
            Debug.Log("Enemy tag detected but EnemyHealth not found.");
        }
    }
}
}
