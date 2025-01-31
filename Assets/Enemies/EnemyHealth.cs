using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public event Action OnDie;
    private bool isDying = false;
    private StateMachine stateMachine;
    private BossTwoMachine bossTwoMachine;

    [SerializeField] private AudioClip damageSound;

    void Awake()
    {
        currentHealth = maxHealth;
    
        stateMachine = GetComponent<StateMachine>();
        bossTwoMachine = GetComponent<BossTwoMachine>();
    }

    public void TakeDamage(float damage)
    {
        if (isDying) return;

        currentHealth -= damage;
        SoundManager.Instance.PlaySFX(damageSound);
        //Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDying = true;
        if(bossTwoMachine = null)
        {
            stateMachine.enabled = false;
        }
        else if (stateMachine = null)
        {
            bossTwoMachine.enabled = false;
        }
        
        gameObject.SetActive(false);
        OnDie?.Invoke();
        
    }
}
