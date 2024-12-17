using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State CurrentState => _currentState; // Expose current state
    private State _currentState;
    private bool inTransition;

    private void Start()
    {
        ChangeState<SpreadState>(); // Set initial state
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
            StartCoroutine(TransitionToState(targetState));
        }
    }

    private System.Collections.IEnumerator TransitionToState(State newState)
    {
        inTransition = true;

        _currentState?.Exit(); // Exit current state
        yield return null; // Ensure smooth transition between frames
        _currentState = newState;
        _currentState?.Enter(); // Enter new state

        inTransition = false;
    }

    private void Update()
    {
        if (_currentState != null && !inTransition)
        {
            _currentState.Update(); // Call the state's Update method
        }
    }
}
