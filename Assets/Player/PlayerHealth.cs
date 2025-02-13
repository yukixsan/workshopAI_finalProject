using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public event Action OnDie;
    [SerializeField]public float currentHealth { get; private set; }
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image fillImage;

    [SerializeField] private GameObject playerModel;
    [SerializeField] private float duration;
    [SerializeField] private int flashCount;

    private Collider playerCollider;
    [SerializeField] private bool isInvulnerable = false;

    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Camera")]
    [SerializeField] private CinemachineImpulseSource impulseSource;
   

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
        impulseSource.GenerateImpulse();
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
            float healthPercent = currentHealth / maxHealth;
            healthBar.value = Mathf.Clamp01(healthPercent * 1.3f) ; // Update the slider value
           
            if (healthBar.value <= 0.5f)
            {
                fillImage.color = new Color32(132, 0, 54, 255);
            }
            else
            {
                fillImage.color = new Color32(132, 219, 54, 255);
            }
        }
        
       
    }

    private void Die()
    {
        playerModel.SetActive(false);
        OnDie?.Invoke();
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
