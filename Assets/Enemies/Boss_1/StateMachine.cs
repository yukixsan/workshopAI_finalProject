using TMPro;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State CurrentState => _currentState; // Expose current state
    private State _currentState;
    private bool inTransition;

    [Header("UI debug")]
    [SerializeField] private TMP_Text _debugText;

    [Header("Phase Settings")]
    private bool phase2Triggered = false; // Tracks if Phase 2 has been triggered
    [SerializeField] private EnemyHealth enemyHealth; // Reference to the EnemyHealth component
    [SerializeField] private float threshold = 5f; // Threshold to trigger Phase 2
    private void Start()
    {
        ChangeState<IntroState>(); // Set initial state
    }

    public void ChangeState<T>() where T : State
    {
        T targetState = GetComponent<T>();
        if (targetState == null)
        {
            Debug.LogWarning("Target state is null!");
            return;
        }

        if (_currentState != targetState && !inTransition)
        {
            StopAllCoroutines();
            StartCoroutine(TransitionToState(targetState));
        }
    }

    private System.Collections.IEnumerator TransitionToState(State newState)
    {
        inTransition = true;

        _currentState?.Exit(); // Exit current state
        if(_currentState != null)
        {
            _currentState.enabled = false;
        }
       
        yield return null; // Ensure smooth transition between frames
        
        _currentState = newState;
        _currentState.enabled = true;
        _currentState?.Enter(); // Enter new state

        //_debugText.text = $"Current State: {_currentState.GetType().Name}";

        inTransition = false;
    }

    private void Update()
    {
        if (_currentState != null && !inTransition)
        {
            _currentState.Update(); // Call the state's Update method
        }
        CheckPhase();
    }

    private void CheckPhase()
    {
        if (phase2Triggered) return;

        if (enemyHealth.currentHealth <= enemyHealth.maxHealth * (threshold / 10f))
        {
            phase2Triggered = true;
            
            print("Enter phase 2");
            ChangeState<TransitionState>();
        }
    }
}
