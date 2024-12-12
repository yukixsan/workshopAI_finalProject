using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections.Generic;


public class SkillSystem : MonoBehaviour
{

    public int maxInputs = 5; // Maximum size of the list
    private List<int> inputList = new List<int>(); // Store the player's inputs
    public Transform firePoint;
    private Vector3 bulletDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        bulletDirection = gameObject.GetComponent<PlayerMovement>().lookDirection;    
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnInput(InputAction.CallbackContext context)
    {
        if(context.performed && inputList.Count < maxInputs) 
        {
            int value = int.Parse(context.control.name); // Get input value (1, 2, 3, or 4)
            inputList.Add(value);
            Debug.Log($"Input {value} added. Current List: {string.Join(", ", inputList)}");
        }
        else if (inputList.Count >= maxInputs)
        {
            EvaluateHand();
            Debug.Log("Input list is full. Fire the skill or clear the list.");
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && inputList.Count == maxInputs)
        {
            FireSkill();
        }
    }
    private void FireSkill()
    {

        int handRank = EvaluateHand();
        Debug.Log($"Hand Ranking Index: {handRank}");
        if (handRank >= 0 && handRank < BulletPool.Instance.bulletPrefabs.Count)
        {
            Bullet bullet = BulletPool.Instance.SpawnFromPool(handRank, firePoint.position, firePoint.rotation);
            if (bullet != null)
            {
                bullet.Shoot();
                Debug.Log($"Firing bullet of type index: {handRank}");
            }
        }
        else
        {
            Debug.LogWarning("No bullet prefab found for this hand ranking!");
        }


        inputList.Clear();
    }

    private int EvaluateHand()
    {
        var grouped = inputList.GroupBy(x => x).ToList(); // Group values by their occurrences
        var counts = grouped.Select(g => g.Count()).OrderByDescending(x => x).ToList(); // Get the counts sorted

        if (counts.SequenceEqual(new List<int> { 5 }))
        {
            Debug.Log("Flush"); // All values are the same
            return 0;
        }
        else if (counts.SequenceEqual(new List<int> { 3, 2 }))
        {
          
            Debug.Log("Full House"); // Three of a kind and a pair
            return 1;
        }
        else if (counts.SequenceEqual(new List<int> { 3, 1, 1 }))
        {
            
            Debug.Log("Three of a Kind"); // Three of one kind
            return 2;
        }
        else if (counts.SequenceEqual(new List<int> { 2, 2, 1 }))
        {
            Debug.Log("Two Pair"); // Two pairs
            return 3;

        }
        else if (counts.SequenceEqual(new List<int> { 2, 1, 1, 1 }))
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
}

