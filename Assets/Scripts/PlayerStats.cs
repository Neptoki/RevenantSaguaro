using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    public HealthBar healthBar;
    public static bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        healthBar.SetSlider(currentHealth);
        
        if (currentHealth <= 0)
            Die();
    }

    public void HealPlayer(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        healthBar.SetSlider(currentHealth);
    }

    private void Die()
    {
        Debug.Log("You died!");
        isDead = true;

        DeathScreenManager deathScreen = FindObjectOfType<DeathScreenManager>();
        if (deathScreen != null)
        {
            deathScreen.ShowDeathScreen();
        }
        //... still need to work on this
    }
}