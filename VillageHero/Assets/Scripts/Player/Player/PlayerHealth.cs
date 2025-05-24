using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth = 100f;
    public float currentHealth;

    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = $"{(int)currentHealth}/{maxHealth}";
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("You Died");
        }

        if (Input.GetKeyDown(KeyCode.Return)) TakeDamage(20);

        if (Input.GetKeyDown(KeyCode.P)) Heal(20);

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;

        healthText.text = $"{(int)currentHealth}/{maxHealth}";
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, 100);

        healthBar.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{(int)currentHealth}/{maxHealth}";
    }
}
