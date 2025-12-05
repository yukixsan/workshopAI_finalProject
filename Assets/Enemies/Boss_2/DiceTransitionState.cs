using UnityEngine;
using DG.Tweening;

public class DiceTransitionState : State
{

    protected BossTwoMachine stateMachine;

    [SerializeField] private GameObject cupObject;
    [SerializeField] private GameObject diceObject;
    [SerializeField] private int transitionDuration = 0;
    [SerializeField] private EnemyHealth enemyHealth;

    private float timer;

    private void Awake()
    {
        stateMachine = GetComponent<BossTwoMachine>();
    }

    public override void Enter()
    {
        print("Entering phase 2");

        timer = transitionDuration;

        diceObject.gameObject.SetActive(true);
        transform.DOMoveY(transform.position.y + 15f, transitionDuration)
            .SetEase(Ease.InOutSine);
    }

    public override void Exit()
    {
        cupObject.gameObject.SetActive(false);
    }

    public override void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Transition to the next state (e.g., Phase 2 Combat State)
            print("Transition ends");
            stateMachine.ChangeState<BurstHorizontal>();
        }
    }
}
