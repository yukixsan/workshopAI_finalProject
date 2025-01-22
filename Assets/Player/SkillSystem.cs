using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;


public class SkillSystem : MonoBehaviour
{

    public int maxInputs = 5; // Maximum size of the list
    private List<string> inputList = new List<string>(); // Store the player's inputs
    public Transform firePoint;
    private Vector3 bulletDirection;
    public ParticleSystem[] rankBullets;

    [SerializeField] public  DecideCard[] tablePosition;

    [SerializeField] private PlayerHealth _health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        print(tablePosition[0]);
        bulletDirection = gameObject.GetComponent<PlayerMovement>().lookDirection;
    }

    private void OnEnable()
    {
        RouletteSpin.ColorSelected += AddToInputList; // Subscribe to roulette event
    }

    private void OnDisable()
    {
        RouletteSpin.ColorSelected -= AddToInputList; // Unsubscribe from roulette event
    }

    private void AddToInputList(string color)
    {
        if (inputList.Count <= maxInputs)
        {
            inputList.Add(color);

            int cardIndex = GetColorIndex(color);
            Debug.Log($"Color {color} mapped to index {cardIndex}");

            if (cardIndex >= 0 && inputList.Count <= tablePosition.Length)
            {
                tablePosition[inputList.Count - 1].Decide(cardIndex);
            }
            else
            {
                Debug.LogWarning($"Invalid card index: {cardIndex} for color {color}");
            }

            // Evaluate hand after each input
            int currentHandRank = EvaluateHand();
            Debug.Log($"Current Hand Rank: {currentHandRank}");
        }
        else if (inputList.Count >= maxInputs)
        {
            EvaluateHand();
            Debug.Log("Input list is full. Fire the skill or clear the list.");
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireSkill();
        }
    }
    private void FireSkill()
    {
        if (inputList.Count == 0)
        {
            Debug.LogWarning("No inputs to evaluate. Cannot fire skill.");
            return;
        }
        int handRank = EvaluateHand();
        Debug.Log($"Hand Ranking Index: {handRank}");

        if (handRank == 5) // High Card
        {
            // Get the first card in the input list
            string firstColor = inputList[0];
            int bulletIndex = GetColorIndex(firstColor);
            if(bulletIndex == 0)
            {
                healBullet(10);
            }
            else if (bulletIndex > 0 && bulletIndex < BulletPool.Instance.bulletPrefabs.Count)
            {
              Bullet bullet = BulletPool.Instance.SpawnFromPool(bulletIndex, firePoint.position, firePoint.rotation);
              if (bullet != null)
              {
                bullet.Shoot();
                Debug.Log($"Firing High Card bullet of type index: {bulletIndex} ({firstColor})");
              }
            }
            else
            {
                Debug.LogWarning($"Invalid bullet index: {bulletIndex} for color {firstColor}");
            }
            
                  
        }
        else if (handRank >= 0 && handRank < rankBullets.Length) // Other ranks
        {
            PlayParticleSystem(handRank);
        }

        foreach (var dealed in tablePosition)
        {
            dealed.HideCurrent();
        }
        inputList.Clear();
    }

    private int EvaluateHand()
    {
        // Group values by their occurrences
        var grouped = inputList.GroupBy(x => x).ToList();
        var counts = grouped.Select(g => g.Count()).OrderByDescending(x => x).ToList();

        // Adjusted conditions for different list sizes
        if (inputList.Count >= 5 && counts.SequenceEqual(new List<int> { 5 }))
        {
            Debug.Log("Flush"); // All values are the same
            return 0;
        }
        else if (inputList.Count >= 5 && counts.SequenceEqual(new List<int> { 3, 2 }))
        {
            Debug.Log("Full House"); // Three of a kind and a pair
            return 1;
        }
        else if (counts.Contains(3))
        {
            Debug.Log("Three of a Kind"); // Three of one kind
            return 2;
        }
        else if (counts.Count(c => c == 2) == 2)
        {
            Debug.Log("Two Pair"); // Two pairs
            return 3;
        }
        else if (counts.Contains(2))
        {
            Debug.Log("Pair"); // One pair
            return 4;
        }
        else
        {
            Debug.Log("High Card"); // No special hand
            return 5;
        }
    }


    private int GetColorIndex(string color)
    {
        switch (color.ToLower())
        {
            case "yellow": return 0;
            case "blue": return 1;
            case "red": return 2;
            default: return -1; // Unknown color
        }
    }

    private void healBullet(int healAmount) 
    {
        _health.currentHealth = Mathf.Min(_health.currentHealth + healAmount, _health.maxHealth);
    }

    private void PlayParticleSystem(int handRank) 
    {
        ParticleSystem selectedParticle = rankBullets[handRank];

        if (selectedParticle == null)
        {
            Debug.LogWarning($"No particle system assigned for hand rank {handRank}");
            return;
        }
        if (!selectedParticle.gameObject.activeSelf)
        {
            selectedParticle.gameObject.SetActive(true);
        }
        // Position the particle system at the firePoint
        selectedParticle.transform.position = firePoint.position;
        //selectedParticle.transform.rotation = firePoint.rotation;

        // Play the particle system
        selectedParticle.Play();
        Debug.Log($"Playing particle system for hand rank {handRank}");
    }
}

