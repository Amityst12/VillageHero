using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;
    public TextMeshProUGUI healthText;

    public float maxHealth;
    public float currentHealth;

    public PlayerReferences playerRefs;

    void Start()
    {
        playerRefs = GameObject.FindWithTag("Player").GetComponent<PlayerReferences>();
        maxHealth = playerRefs.maxHealth;
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("You Died");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(ApplyKnockback());
        UpdateHealthUI();
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{(int)currentHealth}/{(int)maxHealth}";
    }

    private IEnumerator ApplyKnockback()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 knockbackDir = (transform.position - GameObject.FindWithTag("Enemy").transform.position).normalized;
            rb.AddForce(knockbackDir * 250f);  // אפשר לשחק עם העוצמה

            yield return new WaitForSeconds(0.1f);
            rb.velocity = Vector2.zero;  // עוצר תנועה כדי שלא ימשיך לעוף
        }
    }
}
