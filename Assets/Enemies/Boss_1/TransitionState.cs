using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TransitionState : State
{

    protected StateMachine stateMachine;

    [SerializeField] private GameObject _attachment;
    [SerializeField] private GameObject _target;
    [SerializeField] private int transitionDuration = 1;
    [SerializeField] private EnemyHealth enemyHealth;

    private float timer;
    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    public override void Enter()
    {
        print("Entering Phase 2");
        timer = transitionDuration;

        _attachment.SetActive(true);
        
        _target.transform.DOMoveY(transform.position.y + 1.2f, transitionDuration)
            .SetEase(Ease.InOutSine);
    }

    public override void Update()
    {
        // Count down the timer
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Transition to the next state (e.g., Phase 2 Combat State)
            print("transition ends");
            stateMachine.ChangeState<SecondRotate>();
        }
    }
}
