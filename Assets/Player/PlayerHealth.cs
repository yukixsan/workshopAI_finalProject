using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth { get; private set; }
    [SerializeField] private Slider healthBar;
    


    [SerializeField] private GameObject playerModel;
    [SerializeField] private float duration;
    [SerializeField] private int flashCount;

    private Collider playerCollider;
    [SerializeField] private bool isInvulnerable = false;

    [SerializeField] private AudioClip damageSound;

    private void Awake()
    {
        currentHealth = maxHealth;
        playerCollider = GetComponent<Collider>();
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        if(isInvulnerable) return;

        currentHealth -= damage;
        SoundManager.Instance.PlaySFX(damageSound);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(Iframe());
    }

    public void HealDamage(int damage)
    {
        currentHealth += damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent overhealing
        UpdateHealthBar();
     
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth; // Update the slider value
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Handle player death (e.g., respawn or end game logic)
    }

    private IEnumerator Iframe()
    {
        isInvulnerable = true;
        

        // Flash the player model
        for (int i = 0; i < flashCount; i++)
        {
            playerModel.SetActive(false); // Hide the model
            yield return new WaitForSeconds(duration / (flashCount * 2)); // Half of the total flash duration
            playerModel.SetActive(true); // Show the model
            yield return new WaitForSeconds(duration / (flashCount * 2)); // Half of the total flash duration
        }

        // Re-enable the collider
        isInvulnerable = false;
    }

    private void Update()
    {
        
    }
}
